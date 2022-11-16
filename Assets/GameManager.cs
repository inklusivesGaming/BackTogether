using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    //public Grid mGrid;
    public Tilemap mTilemap;
    private TileBase mSelectedTilebase;
    private Vector3 mSelectedTilebasePosition;
    //public Vector3 mMouseClickPosition;
    // Start is called before the first frame update
    void Start()
    {
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

        TileBase newSelectedTileBase = mTilemap.GetTile(cellClickPosition);

        if (mSelectedTilebase)
        {
            Vector2 posDifference = cellClickPosition - mSelectedTilebasePosition;
            if (posDifference.magnitude == 1)
            {

            }
        }
        mSelectedTilebase = newSelectedTileBase;
        mSelectedTilebasePosition = cellClickPosition;
        print(mSelectedTilebase);
    }
}
