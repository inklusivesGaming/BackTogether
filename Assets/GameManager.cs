using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    //public Grid mGrid;
    public Tilemap mTilemap;
    public TileBase mTilebase;
    //public Vector3 mMouseClickPosition;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mouseClickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            print(mouseClickPosition);

            Vector3Int cellClickPosition = mTilemap.WorldToCell(mouseClickPosition);
            print(cellClickPosition);

            TileBase targetTile = mTilemap.GetTile(cellClickPosition);
            print(targetTile);
        }
    }
}
