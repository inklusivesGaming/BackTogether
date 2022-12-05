using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{

    //public Grid mGrid;
    public Tilemap mTilemap;
    public Vector2Int mTilemapMiddlePoint = new Vector2Int(0, 0); // middle coordinates in tilemap

    private Vector2Int mTilemapMinBounds; // smallest coordinates in tilemap
    private Vector2Int mTilemapMaxBounds; // biggest coordinates in tilemap

    private TileBase mSelectedTileBase;
    private Vector3Int mSelectedTileBasePosition; // TODO REMOVE
    private GridObject mSelectedTileBaseGridObject;

    //private List<NormalEgg> mNormalEggs;
    private List<Vector3Int> mNormalEggsPositions;

    public TileBase mStoneTileBase;
    public TileBase mBoneTileBase;
    public TileBase mHoleTileBase;

    private int mNumberOfBones = 0;
    public TMP_Text mNumberOfBonesText;

    private int mNumberOfTurns = 0;
    public TMP_Text mNumberOfTurnsText;

    public GameObject mWinScreen;

    public AudioSource mAudioSource;
    public AudioSource mFinalScreenAudioSource;

    public List<AudioClip> mAudioClips;

    public GameObject mSelectionField;
    private bool mSelectedMode = false;

    public Material mUnselectedMaterial;
    public Material mSelectedMaterial;

    private bool mLastMoveWithoutDestruction = true; // true if you made a move that didnt destroy your selected tile

    private float mSelectionFieldYPos = 0f;
    private Vector2Int mSelectionFieldGridPos;
    private Vector2Int mSelectionFieldOldGridPos; // used when tile gets moved to recognize its old position

    public float mSelectionMovementTime = 0.5f;
    private float mCurrentSelectionMovementTime = 0f;

    public float mTimeBetweenActions = 0.3f;
    private float mCurrentTimeBetweenActions = 0f;

    private Vector3 mSelectionFieldCurrentWorldPos;
    private Vector3 mSelectionFieldTargetWorldPos;

    // Start is called before the first frame update
    void Start()
    {
        mSelectionFieldYPos = mSelectionField.transform.position.y;
        mSelectionFieldGridPos = mTilemapMiddlePoint;
        mSelectionFieldOldGridPos = mSelectionFieldGridPos;

        InitializeTileMap();

        SetSelectionFieldTargetPos(true);
    }

    // Update is called once per frame
    void Update()
    {
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
            HandleInputs();
    }

    // Check where your map is and what kinds of objects are in there
    // It is assumed that the map is always 5x5
    private void InitializeTileMap()
    {
        // assuming grid is always 5x5
        mTilemapMinBounds = mTilemapMiddlePoint - new Vector2Int(2, 2);
        mTilemapMaxBounds = mTilemapMiddlePoint + new Vector2Int(2, 2);

        // initialize egg list

        //mNormalEggs = new List<NormalEgg>();

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
                print(targetGridObject);
                if (targetGridObject is NormalEgg)
                {
                    //mNormalEggs.Add(((NormalEgg)targetGridObject));
                    mNormalEggsPositions.Add(targetPos);
                }
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
                mTilemap.SetTile(gridPosVector3Int, mSelectedTileBase);
                mTilemap.SetTile(oldGridPosVector3Int, null);

                // set the new selected tile
                mSelectedTileBase = mTilemap.GetTile(gridPosVector3Int);
                mSelectedTileBaseGridObject = mTilemap.GetInstantiatedObject(gridPosVector3Int).GetComponent<GridObject>();
            }

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
            if (mSelectedTileBaseGridObject is NormalEgg)
                RemoveEggFromList(oldGridPos);


            // both items are destroyed
            Deselect();
            mTilemap.SetTile(targetGridPos, null);
            mTilemap.SetTile(oldGridPos, null);
            return true;
        }

        if (mSelectedTileBaseGridObject is Bone && targetGridObject is Dino)
        {
            // add bone and delete selection and selected object
            ChangeNumberOfBones(true);
            Deselect();
            mTilemap.SetTile(oldGridPos, null);
            return true;
        }

        if (mSelectedTileBaseGridObject is Dino && targetGridObject is Bone)
        {
            // add bone and perform normal tile movement action
            ChangeNumberOfBones(true);
            return false;
        }

        if (mSelectedTileBaseGridObject is Dino && targetGridObject is Stone && mNumberOfBones > 0)
        {
            // remove bone and perform normal tile movement action
            ChangeNumberOfBones(false);
            //TODO SOUND
            return false;
        }
        return false;
    }

    private void ChangeNumberOfBones(bool addBone)
    {
        if (addBone)
        {
            mNumberOfBones++;
            //TODO SOUND
        }
        else
        {
            mNumberOfBones--;
            if (mNumberOfBones < 0)
                // shouldnt happen
                mNumberOfBones = 0;
            //TODO SOUND
        }

        mNumberOfBonesText.text = mNumberOfBones.ToString();
    }


    private void HandleInputs()
    {
        // Horizontal or vertical selection movement

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal > 0)
        {
            StartSelectionMovement(new Vector2Int(1, 0));
        }

        else if (horizontal < 0)
        {
            StartSelectionMovement(new Vector2Int(-1, 0));
        }

        else if (vertical > 0)
        {
            StartSelectionMovement(new Vector2Int(0, 1));
        }

        else if (vertical < 0)
        {
            StartSelectionMovement(new Vector2Int(0, -1));
        }

        // Selection button down

        if (Input.GetButtonDown("Select"))
        {
            SelectButtonDown();
        }

        // Restart button down
        //if (Input.GetKeyDown("r"))
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void StartSelectionMovement(Vector2Int direction)
    {
        Vector2Int newSelectionFieldPos = mSelectionFieldGridPos + direction;

        if (!IsValidSelectionMovement(newSelectionFieldPos))
        {
            //TODO SOUND
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
            //TODO SOUND
            print("MEH");
            return;
        }

        mSelectedMode = true;
        mSelectionField.GetComponent<MeshRenderer>().material = mSelectedMaterial;

        mSelectedTileBase = mTilemap.GetTile(selectionGridPos);
        mSelectedTileBaseGridObject = mTilemap.GetInstantiatedObject(selectionGridPos).GetComponent<GridObject>();
    }

    // Deselect the currently selected tile
    private void Deselect()
    {
        mSelectedMode = false;
        mSelectionField.GetComponent<MeshRenderer>().material = mUnselectedMaterial;

        mSelectedTileBase = null;
        mSelectedTileBaseGridObject = null;
    }

    private bool IsSelectionValid(Vector3Int gridPos)
    {
        TileBase selection = mTilemap.GetTile(gridPos);
        if (!selection)
            return false;
        GridObject selectionGridObject = mTilemap.GetInstantiatedObject(gridPos).GetComponent<GridObject>();
        if (selectionGridObject is Stone)
            return false;
        return true;
    }

    private void NextTurn()
    {
        mNumberOfTurns++;
        mNumberOfTurnsText.text = mNumberOfTurns.ToString();

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
        if (mNormalEggsPositions.Count == 0)
            return;
        int randomIndex = Random.Range(0, mNormalEggsPositions.Count);
        Vector3Int tilePos = mNormalEggsPositions[randomIndex];

        mNormalEggsPositions.RemoveAt(randomIndex);

        if (new Vector3Int(mSelectionFieldGridPos.x, mSelectionFieldGridPos.y, 0) == tilePos)
            // if the target egg is selected, we want to deselect it
            Deselect();

        mTilemap.SetTile(tilePos, mStoneTileBase);

        // TODO SOUND

        GridObject gridObject = mTilemap.GetInstantiatedObject(tilePos).GetComponent<GridObject>();
        gridObject.Pouf();
    }

    /// //////////////////////////////////////////////////////////////////////////////////
    /**
    // Called when mouse gets pressed
    private void MouseDown()
    {
        Vector3 mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int cellClickPosition = mTilemap.WorldToCell(mouseClickPosition);

        if (cellClickPosition.x < mTilemapMinBounds.x || cellClickPosition.y < mTilemapMinBounds.y ||
            cellClickPosition.x > mTilemapMaxBounds.x || cellClickPosition.y > mTilemapMaxBounds.y)
            // out of bounds
            return;

        TileBase newSelectedTileBase = mTilemap.GetTile(cellClickPosition);

        if (mSelectedTileBase)
        {
            Vector3Int posDifference = cellClickPosition - mSelectedTileBasePosition;

            if (posDifference.magnitude == 1)
            {
                if (!newSelectedTileBase)
                {
                    // move object to new tile
                    MoveTile(cellClickPosition);
                    return;
                }

                else if (mSelectedTileBaseGridObject is Dino)
                {

                    GridObject newSelectedGridObject = mTilemap.GetInstantiatedObject(cellClickPosition).GetComponent<GridObject>();

                    if (newSelectedGridObject is Bone)
                    {
                        // dino steps on bone
                        mNumberOfBones++;
                        mNumberOfBonesText.text = mNumberOfBones.ToString();
                        MoveTile(cellClickPosition);
                        newSelectedTileBase = null;
                        PlayAudio(12);

                    }

                    else if (newSelectedGridObject is Stone && mNumberOfBones > 0)
                    {
                        // dino steps on stone with bone
                        mNumberOfBones--;
                        mNumberOfBonesText.text = mNumberOfBones.ToString();
                        MoveTile(cellClickPosition);
                        newSelectedTileBase = null;
                        PlayAudio(5);

                    }

                    else
                    {
                        PlayAudio(10);
                    }

                }

                else
                {
                    GridObject selectedGridObject = mTilemap.GetInstantiatedObject(cellClickPosition).GetComponent<GridObject>();
                    if (selectedGridObject is Hole)
                    {
                        RemoveSelectedTileBaseFromList();
                        mTilemap.SetTile(mSelectedTileBasePosition, null);
                        mTilemap.SetTile(cellClickPosition, null);
                        mSelectedTileBase = null;
                        newSelectedTileBase = null;

                        PlayAudio(3);

                    }

                    else if (selectedGridObject is Stone)
                    {
                        PlayAudio(10);
                    }
                }
            }


        }

        if (newSelectedTileBase)
        {
            GridObject selectedGridObject = mTilemap.GetInstantiatedObject(cellClickPosition).GetComponent<GridObject>();
            if (selectedGridObject is Hole || selectedGridObject is Stone)
            {
                PlayAudio(10);
                return;
            }
            if (mSelectedTileBase)
                mSelectedTileBaseGridObject.OnDeselected();

            // select new tile base

            mSelectedTileBase = newSelectedTileBase;
            mSelectedTileBasePosition = cellClickPosition;
            mSelectedTileBaseGridObject = selectedGridObject;
            mSelectedTileBaseGridObject.OnSelected();
        }
    }

    // move currently selected tile to cellClickPosition
    private void MoveTile(Vector3Int cellClickPosition)
    {
        mLastMoveWithoutDestruction = true;

        PlayAudio(3);
        RemoveSelectedTileBaseFromList();

        mTilemap.SetTile(cellClickPosition, mSelectedTileBase);
        mTilemap.SetTile(mSelectedTileBasePosition, null);

        AddNewTileBaseToList(cellClickPosition);

        mNumberOfTurns++;
        mNumberOfTurnsText.text = mNumberOfTurns.ToString();

        if (mNumberOfTurns % 5 == 0)
            Stonify();

        if (mSelectedTileBaseGridObject is Dino)
        {
            CheckNeighbours(cellClickPosition, true);
        }

        else if (mSelectedTileBaseGridObject is SurpriseChest)
        {
            CheckNeighbours(cellClickPosition, false);
        }

        if (mLastMoveWithoutDestruction)
        {
            mSelectedTileBase = mTilemap.GetTile(cellClickPosition);
            if (mSelectedTileBase)
            {
                mSelectedTileBasePosition = cellClickPosition;
                mSelectedTileBaseGridObject = mTilemap.GetInstantiatedObject(cellClickPosition).GetComponent<GridObject>();
                if (mSelectedTileBaseGridObject is Stone)
                {
                    // could happen if you move an egg that turns into stone
                    mSelectedTileBase = null;
                    mSelectedTileBaseGridObject = null;
                }

                else
                {
                    mSelectedTileBaseGridObject.OnSelected();
                }
            }

        }
    }

    // Check if game is won after player moved a dino and if surprise chests are activated
    // bool dino is true if you moved a dino and false if you moved a surprisechest
    private void CheckNeighbours(Vector3Int cellClickPosition, bool dino)
    {
        Vector3Int neighbourLeftPos = cellClickPosition + new Vector3Int(-1, 0);
        Vector3Int neighbourRightPos = cellClickPosition + new Vector3Int(1, 0);
        Vector3Int neighbourUpPos = cellClickPosition + new Vector3Int(0, 1);
        Vector3Int neighbourDownPos = cellClickPosition + new Vector3Int(0, -1);

        if (dino)
        {
            // check win condition
            bool win = CheckIfDino(neighbourLeftPos)
                || CheckIfDino(neighbourRightPos)
                || CheckIfDino(neighbourUpPos)
                || CheckIfDino(neighbourDownPos);

            if (win)
            {
                mWinScreen.SetActive(true);
                mFinalScreenAudioSource.PlayOneShot(mAudioClips[8]);

                gameObject.SetActive(false);

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
                TransformSurpriseChest(cellClickPosition);
                mLastMoveWithoutDestruction = false;
                mSelectedTileBase = null;
                mSelectedTileBaseGridObject = null;
            }
        }

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
        PlayAudio(4);
        SurpriseChest surpriseChest = (SurpriseChest)mTilemap.GetInstantiatedObject(pos).GetComponent<GridObject>();

        int transformTarget = 0;
        // dino steps on surprise chest
        if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM)
        {
            transformTarget = Random.Range(0, 3);
        }

        if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.HOLE || surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM && transformTarget == 0)
        {
            mTilemap.SetTile(pos, mHoleTileBase);
        }

        else if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.STONE || surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM && transformTarget == 1)
        {
            mTilemap.SetTile(pos, mStoneTileBase);
        }

        else if (surpriseChest.mTargetItem == SurpriseChest.eTargetItem.BONE || surpriseChest.mTargetItem == SurpriseChest.eTargetItem.RANDOM && transformTarget == 2)
        {
            mTilemap.SetTile(pos, mBoneTileBase);
        }

        GridObject gridObject = mTilemap.GetInstantiatedObject(pos).GetComponent<GridObject>();
        gridObject.Pouf();
    }

    // Turn one random chest in the level into stone
    private void Stonify()
    {
        if (mNormalEggs.Count == 0)
            return;
        int randomIndex = Random.Range(0, mNormalEggs.Count);
        print(randomIndex);
        Vector3Int tilePos = mNormalEggsPositions[randomIndex];

        mNormalEggs.RemoveAt(randomIndex);
        mNormalEggsPositions.RemoveAt(randomIndex);

        mTilemap.SetTile(tilePos, mStoneTileBase);
        PlayAudio(11);

        GridObject gridObject = mTilemap.GetInstantiatedObject(tilePos).GetComponent<GridObject>();
        gridObject.Pouf();
    }

    private void RemoveSelectedTileBaseFromList()
    {
        if (!mSelectedTileBase)
            return;
        if (mSelectedTileBaseGridObject is NormalEgg)
        {
            int index = mNormalEggs.IndexOf((NormalEgg)mSelectedTileBaseGridObject);
            print("REMOVEAT");
            print(index);
            mNormalEggs.RemoveAt(index);
            mNormalEggsPositions.RemoveAt(index);
        }
    }

    private void AddNewTileBaseToList(Vector3Int cellClickPosition)
    {
        GameObject targetObj = mTilemap.GetInstantiatedObject(cellClickPosition);
        if (!targetObj)
            return;
        GridObject targetGridObject = targetObj.GetComponent<GridObject>();

        if (targetGridObject is NormalEgg)
        {
            mNormalEggs.Add((NormalEgg)targetGridObject);
            mNormalEggsPositions.Add(cellClickPosition);
        }
    }

    // track numbers beginning with one
    private void PlayAudio(int trackNumber)
    {
        if (mAudioClips.Count < trackNumber)
            return;
        mAudioSource.PlayOneShot(mAudioClips[trackNumber - 1]);
    }

    **/
}
