using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{

    //public Grid mGrid;
    public Tilemap mTilemap;
    public Vector2Int mTilemapMiddlePoint = new Vector2Int(0, 0);

    private int mNumberOfTurns = 0;

    private Vector2Int mTilemapMinBounds;
    private Vector2Int mTilemapMaxBounds;

    public TileBase mSelectedTileBase;
    private Vector3Int mSelectedTileBasePosition;
    private GridObject mSelectedTileBaseGridObject;

    private List<NormalEgg> mNormalEggs;
    //private List<Stone> mStones;
    //private List<Bone> mBones;
    //private List<Dino> mDinos;

    private List<Vector3Int> mNormalEggsPositions;
    //private List<Vector3Int> mStonesPositions;
    //private List<Vector3Int> mBonesPositions;
    //private List<Vector3Int> mDinosPositions;

    public TileBase mStoneTileBase;
    public TileBase mHoleTileBase;

    private int mNumberOfBones = 0;
    public TMP_Text mNumberOfBonesText;

    //public Vector3 mMouseClickPosition;
    // Start is called before the first frame update
    void Start()
    {
        InitializeTileMap();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            MouseDown();

        // restart button
        if (Input.GetKeyDown("r"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Check where your map is and what kinds of objects are in there
    // It is assumed that the map is always 5x5
    private void InitializeTileMap()
    {
        // assuming grid is always 5x5
        mTilemapMinBounds = mTilemapMiddlePoint - new Vector2Int(2, 2);
        mTilemapMaxBounds = mTilemapMiddlePoint + new Vector2Int(2, 2);

        mNormalEggs = new List<NormalEgg>();
        //mStones = new List<Stone>();
        //mBones = new List<Bone>();
        //mDinos = new List<Dino>();

        mNormalEggsPositions = new List<Vector3Int>();
        //mStonesPositions = new List<Vector3Int>();
        //mBonesPositions = new List<Vector3Int>();
        //mDinosPositions = new List<Vector3Int>();

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
                    mNormalEggs.Add(((NormalEgg)targetGridObject));
                    mNormalEggsPositions.Add(targetPos);
                }

                //else if (targetGridObject is Stone)
                //{
                //    mStones.Add(((Stone)targetGridObject));
                //    mStonesPositions.Add(targetPos);
                //}

                //else if (targetGridObject is Bone)
                //{
                //    mBones.Add(((Bone)targetGridObject));
                //    mBonesPositions.Add(targetPos);
                //}

                //else if (targetGridObject is Dino)
                //{
                //    mDinos.Add(((Dino)targetGridObject));
                //    mDinosPositions.Add(targetPos);
                //}
            }
        }
    }

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
                        mNumberOfBonesText.text = "Bones: " + mNumberOfBones;
                        MoveTile(cellClickPosition);
                        newSelectedTileBase = null;
                    }

                    else if (newSelectedGridObject is Stone && mNumberOfBones > 0)
                    {
                        // dino steps on stone with bone
                        mNumberOfBones--;
                        mNumberOfBonesText.text = "Bones: " + mNumberOfBones;
                        MoveTile(cellClickPosition);
                        newSelectedTileBase = null;
                    }

                    else if (newSelectedGridObject is SurpriseChest)
                    {
                        int transformTarget = 0;
                        // dino steps on surprise chest
                        SurpriseChest surpriseChest = (SurpriseChest)newSelectedGridObject;
                        if (((SurpriseChest)newSelectedGridObject).mTargetItem == SurpriseChest.eTargetItem.RANDOM)
                        {
                            transformTarget = Random.RandomRange(0, 2);
                        }

                        if (((SurpriseChest)newSelectedGridObject).mTargetItem == SurpriseChest.eTargetItem.HOLE || transformTarget == 0)
                        {
                            mTilemap.SetTile(cellClickPosition, mHoleTileBase);
                        }

                        else if (((SurpriseChest)newSelectedGridObject).mTargetItem == SurpriseChest.eTargetItem.STONE || transformTarget == 1)
                        {
                            mTilemap.SetTile(cellClickPosition, mStoneTileBase);
                        }

                        newSelectedTileBase = null;
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

                    }
                }
            }

            
        }

        if (newSelectedTileBase)
        {
            GridObject selectedGridObject = mTilemap.GetInstantiatedObject(cellClickPosition).GetComponent<GridObject>();
            if (selectedGridObject is Hole || selectedGridObject is Stone)
                return;
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
        RemoveSelectedTileBaseFromList();

        mTilemap.SetTile(cellClickPosition, mSelectedTileBase);
        mTilemap.SetTile(mSelectedTileBasePosition, null);

        AddNewTileBaseToList(cellClickPosition);

        mNumberOfTurns++;

        if (mNumberOfTurns % 5 == 0)
            Stonify();

        if (mSelectedTileBaseGridObject is Dino)
        {
            CheckWinCondition(cellClickPosition);
        }

        mSelectedTileBase = null;
        mSelectedTileBaseGridObject = null;
    }

    // Check if game is won after player moved a dino
    private void CheckWinCondition(Vector3Int cellClickPosition)
    {
        bool win = CheckIfDino(cellClickPosition + new Vector3Int(-1, 0)) // neighbour left
            || CheckIfDino(cellClickPosition + new Vector3Int(1, 0)) // neighbour right
            || CheckIfDino(cellClickPosition + new Vector3Int(0, -1)) // neighbour down
            || CheckIfDino(cellClickPosition + new Vector3Int(0, 1)); // neighbour up

        if (win)
        {
            print("YOU WON!");
#if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
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

    // Turn one random chest in the level into stone
    private void Stonify()
    {
        print("STONIFY!");
        int randomIndex = Random.Range(0, mNormalEggs.Count);
        print(randomIndex);
        Vector3Int tilePos = mNormalEggsPositions[randomIndex];

        mNormalEggs.RemoveAt(randomIndex);
        mNormalEggsPositions.RemoveAt(randomIndex);

        mTilemap.SetTile(tilePos, mStoneTileBase);

        //mStones.Add(mTilemap.GetInstantiatedObject(tilePos).GetComponent<Stone>());
        //mStonesPositions.Add(tilePos);
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

        //if (mSelectedTileBaseGridObject is Stone)
        //{
        //    int index = mStones.IndexOf((Stone)mSelectedTileBaseGridObject);
        //    mStones.RemoveAt(index);
        //    mStonesPositions.RemoveAt(index);
        //}

        //if (mSelectedTileBaseGridObject is Bone)
        //{
        //    int index = mBones.IndexOf((Bone)mSelectedTileBaseGridObject);
        //    mBones.RemoveAt(index);
        //    mBonesPositions.RemoveAt(index);
        //}

        //if (mSelectedTileBaseGridObject is Dino)
        //{
        //    int index = mDinos.IndexOf((Dino)mSelectedTileBaseGridObject);
        //    mDinos.RemoveAt(index);
        //    mDinosPositions.RemoveAt(index);
        //}
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

        //if (targetGridObject is Stone)
        //{
        //    mStones.Add((Stone)targetGridObject);
        //    mStonesPositions.Add(cellClickPosition);
        //}

        //if (targetGridObject is Bone)
        //{
        //    mBones.Add((Bone)targetGridObject);
        //    mBonesPositions.Add(cellClickPosition);
        //}

        //if (targetGridObject is Dino)
        //{
        //    mDinos.Add((Dino)targetGridObject);
        //    mDinosPositions.Add(cellClickPosition);
        //}
    }
}
