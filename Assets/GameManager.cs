using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    //public Grid mGrid;
    public Tilemap mTilemap;
    public TileBase mTilebase;
    // Start is called before the first frame update
    void Start()
    {
        //mTilemap.GetTile()
        //mTilemap.DeleteCells(new Vector3Int(0, 0, 0), new Vector3Int(5, 5, 5));
        mTilemap.BoxFill(new Vector3Int(0, 0, 0), mTilebase, 0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
