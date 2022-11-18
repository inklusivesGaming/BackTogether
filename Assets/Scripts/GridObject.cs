using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GridObject : MonoBehaviour
{

    public MeshRenderer mMeshRenderer;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Is called when the object gets selected
    public virtual void OnSelected()
    {
        mMeshRenderer.material.color = Color.yellow;
    }

    // Is called when object gets deselected
    public virtual void OnDeselected()
    {
        mMeshRenderer.material.color = Color.white;
    }

    public virtual void Pouf(VisualEffectAsset poufEffectAsset)
    {
        
    }

}
