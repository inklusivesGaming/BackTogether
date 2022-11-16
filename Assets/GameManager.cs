using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    //public Grid mGrid;
    public Tilemap mTilemap;
    public Vector2Int mTilemapMiddlePoint = new Vector2Int(0,0);

    private Vector2Int mTilemapMinBounds;
    private Vector2Int mTilemapMaxBounds;

    private TileBase mSelectedTileBase;
    private Vector3Int mSelectedTilebasePosition;
    
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

    private void MouseDown()
    {
        Vector3 mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        print(mouseClickPosition);

        Vector3Int cellClickPosition = mTilemap.WorldToCell(mouseClickPosition);
        print(cellClickPosition);

        if (cellClickPosition.x < mTilemapMinBounds.x || cellClickPosition.y < mTilemapMinBounds.y || 
            cellClickPosition.x > mTilemapMaxBounds.x || cellClickPosition.y > mTilemapMaxBounds.y)
            // out of bounds
            return;

        TileBase newSelectedTileBase = mTilemap.GetTile(cellClickPosition);



        if (mSelectedTileBase)
        {
            Vector3Int posDifference = cellClickPosition - mSelectedTilebasePosition;
            if (posDifference.magnitude == 1 && !newSelectedTileBase)
            {
                // move object to new tile
                print("NextField!");
                mTilemap.SetTile(cellClickPosition, mSelectedTileBase);
                print("DELETE");
                print(mSelectedTilebasePosition);
                mTilemap.SetTile(mSelectedTilebasePosition, null);

                mSelectedTileBase = null;
            }
        }
        mSelectedTileBase = newSelectedTileBase;
        mSelectedTilebasePosition = cellClickPosition;
        print(mSelectedTileBase);
    }
}
