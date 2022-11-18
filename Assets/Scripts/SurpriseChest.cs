using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurpriseChest : GridObject
{
    public enum eTargetItem
    {
        RANDOM,
        HOLE,
        STONE
    }

    public eTargetItem mTargetItem = eTargetItem.RANDOM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
