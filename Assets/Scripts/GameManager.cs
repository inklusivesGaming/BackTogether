using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public bool mIsLastLevel = false; // true if this is the last level

    //public Grid mGrid;
    public Tilemap mTilemap;
    public Vector2Int mTilemapMiddlePoint = new Vector2Int(0, 0); // middle coordinates in tilemap

    private Vector2Int mTilemapMinBounds; // smallest coordinates in tilemap
    private Vector2Int mTilemapMaxBounds; // biggest coordinates in tilemap

    private TileBase mSelectedTileBase;
    private GridObject mSelectedTileBaseGridObject;

    private List<Vector3Int> mNormalEggsPositions;

    public TileBase mStoneTileBase;
    public TileBase mBoneTileBase;
    public TileBase mHoleTileBase;

    public GameObject mSelectionField;
    private bool mSelectedMode = false;

    public Material mUnselectedMaterial;
    public Material mSelectedMaterial;

    private float mSelectionFieldYPos = 0f;
    private Vector2Int mSelectionFieldGridPos;
    private Vector2Int mSelectionFieldOldGridPos; // used when tile gets moved to recognize its old position

    public float mSelectionMovementTime = 0.5f;
    private float mCurrentSelectionMovementTime = 0f;

    public float mTimeBetweenActions = 0.3f;
    private float mCurrentTimeBetweenActions = 0f;

    private Vector3 mSelectionFieldCurrentWorldPos;
    private Vector3 mSelectionFieldTargetWorldPos;

    private bool mSurpriseChestHappened = false; // used for audio feedback at end of turn
    private bool mStonifyHappened = false; // used for audio feedback at end of turn

    private List<Vector2Int> mSurpriseChestTransformGridPositions;
    private Vector2Int mStonifyGridPosition;

    private GameAudioManager mGameAudioManager;

    private IngameMenusManager mIngameMenusManager; // registers itself

    private int mNumberOfBones = 0;
    private int mNumberOfTurns = 0;

    float mTutorialWaitTime = 0f;
    bool mWaitForTutorial = false; // gets false shortly after wait time becomes 0 so that you don't open menu by clicking esc

    private EventSystem mEventSystem;

    private bool mWon = false;

    private int mTutorialStage = 1; // stage of the current tutorial for this level

    private bool mTutorial_1_1_DinjaFound = false;
    private bool mTutorial_1_1_SchnuppiFound = false;

    private bool mTutorial_1_3_FirstStonify = false;

    private bool mTutorial_2_2_FirstBone = false;
    private bool mTutorial_2_2_FirstStoneDestroyed = false;

    private bool mTutorial_3_1_FirstSurpriseChest = false;
    private bool mTutorial_3_1_SecondSurpriseChest = false;
    private bool mTutorial_3_1_FirstHoleFilled = false;

    private bool mContrastMode = false;

    public Material mDinoMaterial;
    public Material mDinoMaterialAccessible;

    public Material mObjectMaterial;
    public Material mObjectMaterialAccessible;

    public Material mHoleMaterial;
    public Material mHoleMaterialAccessible;

    public Material mGroundMaterial;
    public Material mGroundMaterialAccessible;

    public Material mDecoMaterial;
    public Material mDecoMaterialAccessible;

    public Material mDecoGroundMaterial;
    public Material mDecoGroundMaterialAccessible;

    private void Awake()
    {
        GameObject gameAudioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");
        if (gameAudioMgrObj && gameAudioMgrObj.TryGetComponent(out GameAudioManager audioMgr))
            mGameAudioManager = audioMgr;

        GameObject eventSystemObj = GameObject.Find("EventSystem");
        if (eventSystemObj && eventSystemObj.TryGetComponent(out EventSystem eventSystem))
            mEventSystem = eventSystem;
    }

    // Start is called before the first frame update
    void Start()
    {
        mSelectionFieldYPos = mSelectionField.transform.position.y;
        mSelectionFieldGridPos = mTilemapMiddlePoint;
        mSelectionFieldOldGridPos = mSelectionFieldGridPos;

        mSurpriseChestTransformGridPositions = new List<Vector2Int>();
        mStonifyGridPosition = new Vector2Int(-1, -1);

        InitializeTileMap();

        mContrastMode = GlobalVariables.mOptionContrastOn;
        SetContrasts();

        SetSelectionFieldTargetPos(true);
        PlayTutorialIntroOutroSound(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (mContrastMode != GlobalVariables.mOptionContrastOn)
        {
            mContrastMode = GlobalVariables.mOptionContrastOn;
            SetContrasts();
        }

        if (NeedToWaitForTutorial())
        {
            WaitForTutorialSound();
            return;
        }

        if (mWon)
            return;

        if (CheckTutorialConditions())
            return;

        if (Input.GetButtonDown("DebugSuperSecretWinButton"))
        {
            Win();
            return;
        }

        if (mCurrentSelectionMovementTime > 0)
            // selection is in the process of moving
            SelectionGetsMoved();
        else if (mCurrentTimeBetweenActions > 0)
        {
            // short cooldown between two actions
            mCurrentTimeBetweenActions -= Time.deltaTime;
            if (mCurrentTimeBetweenActions <= 0)
                mCurrentTimeBetweenActions = 0;
        }
        else
            // listen for a new input
            HandleActionInputs();

        HandleInfoInputs(); // can be fired without cooldown

        TurnReport();
    }

    // returns true and plays tutorial sound when a new condition is met
    private bool CheckTutorialConditions()
    {
        int chapter = GlobalVariables.mCurrentChapter;
        int level = GlobalVariables.mCurrentLevel;

        if (chapter == 1 && level == 1 && mTutorialStage == 1 && mTutorial_1_1_DinjaFound && mTutorial_1_1_SchnuppiFound)
        {
            PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds.C1L1_1, false);
            mTutorialStage++;
            return true;
        }

        if (chapter == 1 && level == 3 && mTutorialStage == 1 && mTutorial_1_3_FirstStonify)
        {
            PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds.C1L3_1, false);
            mTutorialStage++;
            return true;
        }

        if (chapter == 2 && level == 2 && mTutorialStage == 1 && mTutorial_2_2_FirstBone)
        {
            PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds.C2L2_1, false);
            mTutorialStage++;
            return true;
        }

        if (chapter == 2 && level == 2 && mTutorialStage == 2 && mTutorial_2_2_FirstStoneDestroyed)
        {
            PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds.C2L2_2, false);
            mTutorialStage++;
            return true;
        }

        if (chapter == 3 && level == 1 && mTutorialStage == 1 && mTutorial_3_1_FirstSurpriseChest)
        {
            PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds.C3L1_1, false);
            mTutorialStage++;
            return true;
        }

        if (chapter == 3 && level == 1 && mTutorialStage == 2 && mTutorial_3_1_SecondSurpriseChest)
        {
            PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds.C3L1_2, false);
            mTutorialStage++;
            return true;
        }

        if (chapter == 3 && level == 1 && mTutorialStage == 3 && mTutorial_3_1_FirstHoleFilled)
        {
            PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds.C3L1_3, false);
            mTutorialStage++;
            return true;
        }
        return false;
    }

    // if you are in chapter 1 lvl 1 and havent found Dinja and Schnuppi yet, selecting isnt unlocked
    private bool SelectionUnlocked()
    {
        return GlobalVariables.mCurrentChapter != 1 || GlobalVariables.mCurrentLevel != 1 || mTutorial_1_1_DinjaFound && mTutorial_1_1_SchnuppiFound;
    }

    private bool StonifyUnlocked()
    {
        return GlobalVariables.mCurrentChapter > 1 || GlobalVariables.mCurrentLevel >= 3;
    }

    private bool TurnInfoUnlocked()
    {
        return GlobalVariables.mCurrentChapter >= 2 || GlobalVariables.mCurrentChapter == 1 && (GlobalVariables.mCurrentLevel >= 4 || GlobalVariables.mCurrentLevel == 3 && mTutorialStage >= 2);
    }
    private bool BoneInfoUnlocked()
    {
        return GlobalVariables.mCurrentChapter >= 3 || GlobalVariables.mCurrentChapter == 2 && GlobalVariables.mCurrentLevel >= 2;
    }

    private void PlayTutorialIntroOutroSound(bool intro)
    {
        GameAudioManager.TutorialIntroOutroSounds sound = GlobalVariables.GetTutorialSound(intro);
        if (sound != GameAudioManager.TutorialIntroOutroSounds.None)
        {
            mWaitForTutorial = true;
            mTutorialWaitTime = mGameAudioManager.PlayTutorialIntroOutroSound(sound);
            if (mEventSystem)
                mEventSystem.enabled = false;
        }
    }

    // if stopAndPlay is false, it just gets played instead
    private void PlayTutorialIngameSound(GameAudioManager.TutorialIngameSounds sound, bool stopAndPlay = true)
    {
        mWaitForTutorial = true;
        mTutorialWaitTime = mGameAudioManager.PlayTutorialIngameSound(sound, stopAndPlay);
        if (mEventSystem)
            mEventSystem.enabled = false;
    }

    private void WaitForTutorialSound()
    {
        if (mTutorialWaitTime <= 0f)
        {
            // one update step later than tutorial wait time becoming 0
            mWaitForTutorial = false;
            mEventSystem.enabled = true;

            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // interrupt tutorial output by clicking esc
            mTutorialWaitTime = 0f;
            mGameAudioManager.StopAudio();
            // TODO add sound for skipping
        }
        else
        {
            mTutorialWaitTime -= Time.deltaTime;

            if (mTutorialWaitTime < 0f)
                mTutorialWaitTime = 0f;
        }
    }

    public bool NeedToWaitForTutorial()
    {
        return mWaitForTutorial;
    }

    // Ingame menus manager registers itself
    public void RegisterIngameMenusManager(IngameMenusManager ingameMenusManager)
    {
        mIngameMenusManager = ingameMenusManager;
        mIngameMenusManager.SetUITexts(mNumberOfBones, mNumberOfTurns);

        if (mIsLastLevel)
            mIngameMenusManager.SetLastLevel();
    }

    // Check where your map is and what kinds of objects are in there
    // It is assumed that the map is always 5x5
    private void InitializeTileMap()
    {
        // assuming grid is always 5x5
        mTilemapMinBounds = mTilemapMiddlePoint - new Vector2Int(2, 2);
        mTilemapMaxBounds = mTilemapMiddlePoint + new Vector2Int(2, 2);

        // initialize egg list

        mNormalEggsPositions = new List<Vector3Int>();

        for (int x = mTilemapMinBounds.x; x <= mTilemapMaxBounds.x; x++)
        {
            for (int y = mTilemapMinBounds.y; y <= mTilemapMaxBounds.y; y++)
            {
                Vector3Int targetPos = new Vector3Int(x, y, 0);
                TileBase targetTileBase = mTilemap.GetTile(targetPos);
                if (!targetTileBase)
                    continue;
                GridObject targetGridObject = mTilemap.GetInstantiatedObject(targetPos).GetComponent<GridObject>();
                if (targetGridObject is NormalEgg)
                    mNormalEggsPositions.Add(targetPos);
            }
        }
    }

    // Sets the target position your selection field should move to
    // If directMoveToPos is true, also move the selection field directly to this position (without animation) and move the selected object to that pos if there is one
    private void SetSelectionFieldTargetPos(bool directMoveToPos)
    {
        Vector3 tilemapMiddlePointWorld = mTilemap.CellToWorld(new Vector3Int(mSelectionFieldGridPos.x, mSelectionFieldGridPos.y, 0));

        mSelectionFieldTargetWorldPos = new Vector3(tilemapMiddlePointWorld.x + 0.5f, mSelectionFieldYPos, tilemapMiddlePointWorld.z + 0.5f);

        if (directMoveToPos)
            DirectMoveToTargetPos();

        mSelectionFieldCurrentWorldPos = mSelectionField.transform.position;
    }

    private void DirectMoveToTargetPos()
    {
        mSelectionField.transform.position = mSelectionFieldTargetWorldPos;

        if (mSelectedMode)
        {
            Vector3Int gridPosVector3Int = new Vector3Int(mSelectionFieldGridPos.x, mSelectionFieldGridPos.y, 0);
            Vector3Int oldGridPosVector3Int = new Vector3Int(mSelectionFieldOldGridPos.x, mSelectionFieldOldGridPos.y, 0);


            // TODO when doing it this way, the dino animation always starts anew when moving!

            if (!PerformSpecialTileAction(oldGridPosVector3Int, gridPosVector3Int))
            {
                if (mSelectedTileBaseGridObject is NormalEgg)
                {
                    RemoveEggFromList(oldGridPosVector3Int);
                    AddEggToList(gridPosVector3Int);
                }

                // move the tile
                SetTileOnMap(gridPosVector3Int, mSelectedTileBase);
                SetTileOnMap(oldGridPosVector3Int, null);

                // set the new selected tile
                mSelectedTileBase = mTilemap.GetTile(gridPosVector3Int);
                mSelectedTileBaseGridObject = mTilemap.GetInstantiatedObject(gridPosVector3Int).GetComponent<GridObject>();
            }

            CheckNeighbours();
            NextTurn();
        }

        // set the tile pos
        mSelectionFieldOldGridPos = mSelectionFieldGridPos;

    }

    // When moving a tile, checks if target tile has grid object and thus a special action gets performed
    // Returns true if a special action was performed and thus the tilebase gets deselected
    // In CanIMoveThat its already checked that selected tile and target tile are compatible (eg dino and stone while having bone)
    private bool PerformSpecialTileAction(Vector3Int oldGridPos, Vector3Int targetGridPos)
    {
        if (!mTilemap.GetTile(targetGridPos))
            // nothing on target tile
            return false;
        GridObject targetGridObject = mTilemap.GetInstantiatedObject(targetGridPos).GetComponent<GridObject>();

        if (targetGridObject is Hole)
        {
            mTutorial_3_1_FirstHoleFilled = true;
            if (mSelectedTileBaseGridObject is NormalEgg)
                RemoveEggFromList(oldGridPos);


            // both items are destroyed
            Deselect();
            SetTileOnMap(targetGridPos, null);
            SetTileOnMap(oldGridPos, null);
            mGameAudioManager.PlayHoleFilledEvent
                (GameAudioManager.EventSounds.DasLochBei,
                GetNavigationEnum(targetGridPos.x, true),
                GetNavigationEnum(targetGridPos.y, false),
                GameAudioManager.EventSounds.istVerschlossen,
                GameAudioManager.EventSounds.DuKannstDrueberlaufen);
            return true;
        }

        if (mSelectedTileBaseGridObject is Bone && targetGridObject is Dino)
        {
            // add bone and delete selection and selected object
            ChangeNumberOfBones(true);
            mTutorial_2_2_FirstBone = true;
            Deselect();
            SetTileOnMap(oldGridPos, null);
            return true;
        }

        if (mSelectedTileBaseGridObject is Dino && targetGridObject is Bone)
        {
            // add bone and perform normal tile movement action
            ChangeNumberOfBones(true);
            mTutorial_2_2_FirstBone = true;
            return false;
        }

        if (mSelectedTileBaseGridObject is Dino && targetGridObject is Stone && mNumberOfBones > 0)
        {
            // remove bone and perform normal tile movement action
            mTutorial_2_2_FirstStoneDestroyed = true;
            ChangeNumberOfBones(false);
            mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.SteinZerstoert);
            return false;
        }
        return false;
    }

    private void ChangeNumberOfBones(bool addBone)
    {
        if (addBone)
        {
            mNumberOfBones++;
            mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.KnochenGesammelt);
        }
        else
        {
            mNumberOfBones--;
            mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.KnochenGesammelt);
            if (mNumberOfBones < 0)
                // shouldnt happen
                mNumberOfBones = 0;
        }

        if (mIngameMenusManager)
            mIngameMenusManager.SetUITexts(mNumberOfBones, mNumberOfTurns);
    }


    private void HandleActionInputs()
    {
        // Horizontal or vertical selection movement

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0)
            StartSelectionMovement(new Vector2Int(1, 0));

        else if (horizontal < 0)
            StartSelectionMovement(new Vector2Int(-1, 0));

        else if (vertical > 0)
            StartSelectionMovement(new Vector2Int(0, 1));

        else if (vertical < 0)
            StartSelectionMovement(new Vector2Int(0, -1));

        // Selection button down

        if (Input.GetButtonDown("Select") && SelectionUnlocked())
            SelectButtonDown();
    }

    private void HandleInfoInputs()
    {
        if (Input.GetButtonDown("Info_GridPosition"))
            TellGridPosition();

        if (Input.GetButtonDown("Info_NumberOfBones") && BoneInfoUnlocked())
            TellNumberOfBones();

        if (Input.GetButtonDown("Info_TurnsLeft") && TurnInfoUnlocked())
            TellNumberOfTurnsLeft();
    }

    private void StartSelectionMovement(Vector2Int direction)
    {
        Vector2Int newSelectionFieldPos = mSelectionFieldGridPos + direction;

        if (!IsValidSelectionMovement(newSelectionFieldPos))
        {
            mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.Verboten);
            mCurrentTimeBetweenActions = mTimeBetweenActions;
            return;
        }

        mSelectionFieldGridPos = newSelectionFieldPos;
        mCurrentSelectionMovementTime = mSelectionMovementTime;
        if (mCurrentSelectionMovementTime == 0)
        {
            // move selection field immediately
            mCurrentTimeBetweenActions = mTimeBetweenActions;
            SetSelectionFieldTargetPos(true);
        }
        else
            // move selection field smoothly
            SetSelectionFieldTargetPos(false);

        if (mSelectedMode)
            mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.ObjektBewegen);
        else
            mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.AuswahlfeldBewegen);
    }

    private void SelectionGetsMoved()
    {
        mCurrentSelectionMovementTime -= Time.deltaTime;
        if (mCurrentSelectionMovementTime <= 0)
        {
            mCurrentSelectionMovementTime = 0;
            mCurrentTimeBetweenActions = mTimeBetweenActions;
            SetSelectionFieldTargetPos(true);
        }

        else
        {
            // move selection field a bit closer to target pos
            mSelectionField.transform.position = Vector3.Lerp(mSelectionFieldCurrentWorldPos, mSelectionFieldTargetWorldPos, 1 - mCurrentSelectionMovementTime / mSelectionMovementTime);
            if (mSelectedMode)
                // also move selected object if selection mode on
                mSelectedTileBaseGridObject.gameObject.transform.position = Vector3.Lerp(mSelectionFieldCurrentWorldPos, mSelectionFieldTargetWorldPos, 1 - mCurrentSelectionMovementTime / mSelectionMovementTime);
        }
    }

    // Check if you are still in the grid while moving selection
    // If a tile is selected, also check if you can move the tile in that direction
    private bool IsValidSelectionMovement(Vector2Int targetPos)
    {
        if (targetPos.x < mTilemapMinBounds.x || targetPos.x > mTilemapMaxBounds.x || targetPos.y < mTilemapMinBounds.y || targetPos.y > mTilemapMaxBounds.y)
            // outside of grid
            return false;
        if (mSelectedMode)
        {
            Vector3Int targetPosVector3Int = new Vector3Int(targetPos.x, targetPos.y, 0);
            if (mTilemap.GetTile(targetPosVector3Int)
                && !CanIMoveThat(mSelectedTileBaseGridObject, mTilemap.GetInstantiatedObject(targetPosVector3Int).GetComponent<GridObject>()))
                // tile is in the way and I'm not allowed to move my selected tile to that tile
                return false;
        }
        return true;
    }

    // Checks if you can move your object onto the target object when there is a target object
    private bool CanIMoveThat(GridObject myObject, GridObject targetObject)
    {
        if (myObject is Dino && targetObject is Stone && mNumberOfBones > 0
            || myObject is Dino && targetObject is Bone
            || myObject is Bone && targetObject is Dino
            || !(myObject is Dino) && targetObject is Hole)
            return true;
        return false;
    }

    private void SelectButtonDown()
    {
        mCurrentTimeBetweenActions = mTimeBetweenActions;
        if (mSelectedMode)
            Deselect();

        else
            Select();
    }

    // Select the tile under your selection field if it is selectable (no free tile and no stone)
    private void Select()
    {
        Vector3Int selectionGridPos = new Vector3Int(mSelectionFieldGridPos.x, mSelectionFieldGridPos.y, 0);

        if (!IsSelectionValid(selectionGridPos))
        {
            mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.Verboten);
            return;
        }

        mSelectedMode = true;
        mSelectionField.GetComponent<MeshRenderer>().material = mSelectedMaterial;

        mSelectedTileBase = mTilemap.GetTile(selectionGridPos);
        mSelectedTileBaseGridObject = mTilemap.GetInstantiatedObject(selectionGridPos).GetComponent<GridObject>();
        mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.AuswahlObjekt);
    }

    // Deselect the currently selected tile
    private void Deselect()
    {
        mSelectedMode = false;
        mSelectionField.GetComponent<MeshRenderer>().material = mUnselectedMaterial;

        mSelectedTileBase = null;
        mSelectedTileBaseGridObject = null;

        mGameAudioManager.PlayActionSound(GameAudioManager.ActionSounds.AbwahlObjekt);
    }

    private bool IsSelectionValid(Vector3Int gridPos)
    {
        TileBase selection = mTilemap.GetTile(gridPos);
        if (!selection)
            return false;
        GridObject selectionGridObject = mTilemap.GetInstantiatedObject(gridPos).GetComponent<GridObject>();
        if (selectionGridObject is Stone || selectionGridObject is Hole)
            return false;
        return true;
    }

    private void NextTurn()
    {
        mNumberOfTurns++;
        if (mIngameMenusManager)
            mIngameMenusManager.SetUITexts(mNumberOfBones, mNumberOfTurns);
        if (mWon)
            return;
        if (mNumberOfTurns % 5 == 0)
            Stonify();
    }

    private void RemoveEggFromList(Vector3Int pos)
    {
        int index = mNormalEggsPositions.IndexOf(pos);
        if (index == -1)
            return;
        mNormalEggsPositions.RemoveAt(index);
    }

    private void AddEggToList(Vector3Int pos)
    {
        if (mNormalEggsPositions.Contains(pos))
            return;
        mNormalEggsPositions.Add(pos);
    }

    // Turn one random egg in the level into stone
    private void Stonify()
    {
        if (mNormalEggsPositions.Count == 0 || !StonifyUnlocked())
            return;
        mTutorial_1_3_FirstStonify = true;
        int randomIndex = Random.Range(0, mNormalEggsPositions.Count);
        Vector3Int tilePos = mNormalEggsPositions[randomIndex];

        mNormalEggsPositions.RemoveAt(randomIndex);

        if (new Vector3Int(mSelectionFieldGridPos.x, mSelectionFieldGridPos.y, 0) == tilePos)
            // if the target egg is selected, we want to deselect it
            Deselect();

        SetTileOnMap(tilePos, mStoneTileBase);

        mGameAudioManager.PlayEventSound(GameAudioManager.EventSounds.VersteinerungsSound);
        mStonifyHappened = true;
        mStonifyGridPosition = new Vector2Int(tilePos.x, tilePos.y);

        GridObject gridObject = mTilemap.GetInstantiatedObject(tilePos).GetComponent<GridObject>();
        gridObject.Pouf();
    }

    // Check if game is won after player moved a dino and if surprise chests are activated
    // bool dino is true if you moved a dino and false if you moved a surprisechest
    private void CheckNeighbours()
    {
        if (!mSelectedTileBaseGridObject)
            return;

        bool dino = false;
        if (mSelectedTileBaseGridObject is Dino)
            dino = true;
        else if (!(mSelectedTileBaseGridObject is SurpriseChest))
            return;

        Vector3Int selectionFieldGridPosVector3Int = new Vector3Int(mSelectionFieldGridPos.x, mSelectionFieldGridPos.y, 0);

        Vector3Int neighbourLeftPos = selectionFieldGridPosVector3Int + new Vector3Int(-1, 0);
        Vector3Int neighbourRightPos = selectionFieldGridPosVector3Int + new Vector3Int(1, 0);
        Vector3Int neighbourUpPos = selectionFieldGridPosVector3Int + new Vector3Int(0, 1);
        Vector3Int neighbourDownPos = selectionFieldGridPosVector3Int + new Vector3Int(0, -1);

        if (dino)
        {
            // check win condition
            bool win = CheckIfDino(neighbourLeftPos)
                || CheckIfDino(neighbourRightPos)
                || CheckIfDino(neighbourUpPos)
                || CheckIfDino(neighbourDownPos);

            if (win)
            {
                Win();
                return;
            }

            if (CheckIfSurpriseChest(neighbourLeftPos))
                TransformSurpriseChest(neighbourLeftPos);

            if (CheckIfSurpriseChest(neighbourRightPos))
                TransformSurpriseChest(neighbourRightPos);

            if (CheckIfSurpriseChest(neighbourUpPos))
                TransformSurpriseChest(neighbourUpPos);

            if (CheckIfSurpriseChest(neighbourDownPos))
                TransformSurpriseChest(neighbourDownPos);
        }

        else
        {
            if (CheckIfDino(neighbourLeftPos)
                || CheckIfDino(neighbourRightPos)
                || CheckIfDino(neighbourUpPos)
                || CheckIfDino(neighbourDownPos))

            {
                // transform currently selected surprise chest
                Deselect();
                TransformSurpriseChest(selectionFieldGridPosVector3Int);
            }
        }

        if (mSurpriseChestHappened)
            mGameAudioManager.PlayEventSound(GameAudioManager.EventSounds.RaetselPuffSound);

    }

    private bool CheckIfDino(Vector3Int pos)
    {
        GameObject targetObj = mTilemap.GetInstantiatedObject(pos);
        if (!targetObj)
            return false;

        if (targetObj.GetComponent<GridObject>() is Dino)
            return true;

        return false;
    }

    private bool CheckIfSurpriseChest(Vector3Int pos)
    {
        GameObject targetObj = mTilemap.GetInstantiatedObject(pos);
        if (!targetObj)
            return false;

        if (targetObj.GetComponent<GridObject>() is SurpriseChest)
            return true;

        return false;
    }

    private void TransformSurpriseChest(Vector3Int pos)
    {
        SurpriseChest surpriseChest = (SurpriseChest)mTilemap.GetInstantiatedObject(pos).GetComponent<GridObject>();

        int transformTarget = 0;
        // dino steps on surprise chest
        if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM)
            transformTarget = Random.Range(0, 3);

        if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.HOLE || surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM && transformTarget == 0)
        {
            SetTileOnMap(pos, mHoleTileBase);
            if (mTutorial_3_1_FirstSurpriseChest)
                mTutorial_3_1_SecondSurpriseChest = true;
        }

        else if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.STONE || surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM && transformTarget == 1)
            SetTileOnMap(pos, mStoneTileBase);

        else if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.BONE || surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM && transformTarget == 2)
            SetTileOnMap(pos, mBoneTileBase);

        GridObject gridObject = mTilemap.GetInstantiatedObject(pos).GetComponent<GridObject>();
        gridObject.Pouf();

        mTutorial_3_1_FirstSurpriseChest = true;

        mSurpriseChestHappened = true;
        mSurpriseChestTransformGridPositions.Add(new Vector2Int(pos.x, pos.y));
    }

    // Set tile on map and adapt it to contrast settings
    private void SetTileOnMap(Vector3Int pos, TileBase tileBase)
    {
        mTilemap.SetTile(pos, tileBase);

        if (!tileBase)
            return;

        // change texture if contrast mode is on
        GridObject targetGridObject = mTilemap.GetInstantiatedObject(pos).GetComponent<GridObject>();
        SetContrast(targetGridObject);
    }

    private void Win()
    {
        mWon = true;
        PlayTutorialIntroOutroSound(false);
        mIngameMenusManager.Win();
    }

    // Returns speech output that tells the grid position and object at this position
    private void TellGridPosition()
    {
        int xPos = mSelectionFieldGridPos.x;
        int yPos = mSelectionFieldGridPos.y;
        Vector3Int selectionFieldGridPosVector3Int = new Vector3Int(xPos, yPos, 0);

        GameAudioManager.NavigationSounds letterSound = GetNavigationEnum(xPos, true);
        GameAudioManager.NavigationSounds numberSound = GetNavigationEnum(yPos, false);

        GameAudioManager.GridObjectSounds gridObjectSound = GetGridObjectSound(selectionFieldGridPosVector3Int);

        if (gridObjectSound == GameAudioManager.GridObjectSounds.Dinja)
        {
            // for first tutorial
            mTutorial_1_1_DinjaFound = true;
        }

        if (gridObjectSound == GameAudioManager.GridObjectSounds.Schnuppi)
        {
            // for first tutorial
            mTutorial_1_1_SchnuppiFound = true;
        }


        mGameAudioManager.PlayAudioPositionInGrid(letterSound, numberSound, gridObjectSound);
    }

    private void TellNumberOfBones()
    {
        mGameAudioManager.PlayBoneInfo(GetNumberOfSomethingEnum(mNumberOfBones), GameAudioManager.GridObjectSounds.Knochen);
    }

    private void TellNumberOfTurnsLeft()
    {
        mGameAudioManager.PlayTurnsLeft(GetNumberOfSomethingEnum(5 - mNumberOfTurns % 5));
    }

    // If letter==true, return letter, else return number
    private GameAudioManager.NavigationSounds GetNavigationEnum(int pos, bool letter)
    {

        if (letter)
        {
            if (pos == mTilemapMinBounds.x)
                return GameAudioManager.NavigationSounds.A;
            else if (pos == mTilemapMinBounds.x + 1)
                return GameAudioManager.NavigationSounds.B;
            else if (pos == mTilemapMinBounds.x + 2)
                return GameAudioManager.NavigationSounds.C;
            else if (pos == mTilemapMinBounds.x + 3)
                return GameAudioManager.NavigationSounds.D;
            else if (pos == mTilemapMinBounds.x + 4)
                return GameAudioManager.NavigationSounds.E;
        }
        else
        {
            if (pos == mTilemapMinBounds.y)
                return GameAudioManager.NavigationSounds.Fünf;
            else if (pos == mTilemapMinBounds.y + 1)
                return GameAudioManager.NavigationSounds.Vier;
            else if (pos == mTilemapMinBounds.y + 2)
                return GameAudioManager.NavigationSounds.Drei;
            else if (pos == mTilemapMinBounds.y + 3)
                return GameAudioManager.NavigationSounds.Zwei;
            else if (pos == mTilemapMinBounds.y + 4)
                return GameAudioManager.NavigationSounds.Eins;
        }

        return GameAudioManager.NavigationSounds.Null; // shouldnt happen
    }

    private GameAudioManager.NavigationSounds GetNumberOfSomethingEnum(int number)
    {
        if (number >= 5)
            return GameAudioManager.NavigationSounds.Fünf;
        else if (number == 4)
            return GameAudioManager.NavigationSounds.Vier;
        else if (number == 3)
            return GameAudioManager.NavigationSounds.Drei;
        else if (number == 2)
            return GameAudioManager.NavigationSounds.Zwei;
        else if (number == 1)
            return GameAudioManager.NavigationSounds.Ein;


        return GameAudioManager.NavigationSounds.Null;
    }

    private GameAudioManager.GridObjectSounds GetGridObjectSound(Vector3Int pos)
    {
        GameObject obj = mTilemap.GetInstantiatedObject(pos);
        if (!obj)
            return GameAudioManager.GridObjectSounds.frei;
        GridObject gridObject = obj.GetComponent<GridObject>();
        if (gridObject is Dino)
        {
            if (((Dino)gridObject).mFemale)
                return GameAudioManager.GridObjectSounds.Dinja;
            else
                return GameAudioManager.GridObjectSounds.Schnuppi;
        }

        if (gridObject is Stone)
            return GameAudioManager.GridObjectSounds.Steinhaufen;

        if (gridObject is NormalEgg)
            return GameAudioManager.GridObjectSounds.Ei;

        if (gridObject is Bone)
            return GameAudioManager.GridObjectSounds.Knochen;

        if (gridObject is SurpriseChest)
            return GameAudioManager.GridObjectSounds.Raetselkiste;

        if (gridObject is Hole)
            return GameAudioManager.GridObjectSounds.Loch;

        return GameAudioManager.GridObjectSounds.frei;

    }


    // Return audio output with turn report if turn happened and 
    // surprise chest transformation or egg tranformation happened
    private void TurnReport()
    {
        if (!mSurpriseChestHappened && !mStonifyHappened)
            return;

        // reset saved transformations
        if (mStonifyHappened)
        {
            mGameAudioManager.PlayStonifyInformation
                (
                GameAudioManager.EventSounds.EiVersteinertNachricht,
                GetNavigationEnum(mStonifyGridPosition.x, true),
                GetNavigationEnum(mStonifyGridPosition.y, false)
                );
        }

        mSurpriseChestHappened = false;
        mStonifyHappened = false;
        mSurpriseChestTransformGridPositions = new List<Vector2Int>();
        mStonifyGridPosition = new Vector2Int(-1, -1);
    }

    public void LoadNextScene()
    {
        //SceneManager.LoadScene(GlobalVariables.GetNextScene(SceneManager.GetActiveScene().name));
        SceneManager.LoadScene(GlobalVariables.GetNextScene());
    }

    public void SetContrasts()
    {
        // change colors of objects and dinos
        for (int x = mTilemapMinBounds.x; x <= mTilemapMaxBounds.x; x++)
        {
            for (int y = mTilemapMinBounds.y; y <= mTilemapMaxBounds.y; y++)
            {
                Vector3Int targetPos = new Vector3Int(x, y, 0);
                TileBase targetTileBase = mTilemap.GetTile(targetPos);
                if (!targetTileBase)
                    continue;
                GridObject targetGridObject = mTilemap.GetInstantiatedObject(targetPos).GetComponent<GridObject>();
                SetContrast(targetGridObject);
            }
        }

        // change colors of deco objects
        foreach (GameObject decoContainer in GameObject.FindGameObjectsWithTag(GlobalVariables.TAG_DECOCONTAINER))
            foreach (Renderer renderer in decoContainer.GetComponentsInChildren<Renderer>())
                renderer.material = mContrastMode ? mDecoMaterialAccessible : mDecoMaterial;

        foreach (GameObject decoGroundContainer in GameObject.FindGameObjectsWithTag(GlobalVariables.TAG_DECOGROUNDCONTAINER))
            foreach (Renderer renderer in decoGroundContainer.GetComponentsInChildren<Renderer>())
                renderer.material = mContrastMode ? mDecoGroundMaterialAccessible : mDecoGroundMaterial;

        foreach (GameObject ground in GameObject.FindGameObjectsWithTag(GlobalVariables.TAG_GROUND))
        {
            Renderer renderer = ground.GetComponent<Renderer>();
            renderer.material = mContrastMode ? mGroundMaterialAccessible : mGroundMaterial;

        }

    }

    private void SetContrast(GridObject targetGridObject)
    {
        Renderer targetRenderer = targetGridObject.gameObject.GetComponentInChildren<Renderer>();
        if (targetGridObject is Dino)
        {
            SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)targetRenderer;
            Material[] mats = skinnedMeshRenderer.materials;
            mats[mats.Length - 1] = mContrastMode ? mDinoMaterialAccessible : mDinoMaterial; // last material is material for the body
            skinnedMeshRenderer.materials = mats;
        }
        else if (targetGridObject is Hole)
            targetRenderer.material = mContrastMode ? mHoleMaterialAccessible : mHoleMaterial;
        else
            targetRenderer.material = mContrastMode ? mObjectMaterialAccessible : mObjectMaterial;
    }
}
