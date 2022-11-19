using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadAudio : MonoBehaviour
{
    public static bool mAlreadyExists = false;

    // Start is called before the first frame update
    void Start()
    {
        if (mAlreadyExists)
            Destroy(gameObject);
        mAlreadyExists = true;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
