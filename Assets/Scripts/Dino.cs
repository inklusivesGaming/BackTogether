using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dino : GridObject
{
    public SkinnedMeshRenderer mSkinnedMeshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Is called when the object gets selected
    public override void OnSelected()
    {
        mSkinnedMeshRenderer.material.color = Color.yellow;
    }

    // Is called when object gets deselected
    public override void OnDeselected()
    {
        mSkinnedMeshRenderer.material.color = Color.white;
    }
}
