using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Is called when the object gets selected
    public void OnSelected()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
    }

    // Is called when object gets deselected
    public void OnDeselected()
    {
        gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
    }

}
