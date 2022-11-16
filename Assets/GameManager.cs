using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    //public Grid mGrid;
    public Tilemap mTilemap;
    public Vector2Int mTilemapMiddlePoint = new Vector2Int(0, 0);

    private Vector2Int mTilemapMinBounds;
    private Vector2Int mTilemapMaxBounds;

    private TileBase mSelectedTileBase;
    private Vector3Int mSelectedTileBasePosition;
    private GridObject mSelectedTileBaseGridObject;

    //public Vector3 mMouseClickPosition;
    // Start is called before the first frame update
    void Start()
    {
        // assuming grid is always 5x5
        mTilemapMinBounds = mTilemapMiddlePoint - new Vector2Int(2, 2);
        mTilemapMaxBounds = mTilemapMiddlePoint + new Vector2Int(2, 2);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
            MouseDown();
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
            if (posDifference.magnitude == 1 && !newSelectedTileBase)
            {
                // move object to new tile
                mTilemap.SetTile(cellClickPosition, mSelectedTileBase);
                mTilemap.SetTile(mSelectedTileBasePosition, null);

                if (mSelectedTileBaseGridObject is Dino)
                {
                    CheckWinCondition(cellClickPosition);
                }

                mSelectedTileBase = null;
                mSelectedTileBaseGridObject = null;

                return;
            }
        }

        if (newSelectedTileBase)
        {
            if (mSelectedTileBase)
                mSelectedTileBaseGridObject.OnDeselected();

            // select new tile base

            mSelectedTileBase = newSelectedTileBase;
            mSelectedTileBasePosition = cellClickPosition;
            mSelectedTileBaseGridObject = mTilemap.GetInstantiatedObject(cellClickPosition).GetComponent<GridObject>();
            mSelectedTileBaseGridObject.OnSelected();
        }



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
            Application.Quit()
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
}
