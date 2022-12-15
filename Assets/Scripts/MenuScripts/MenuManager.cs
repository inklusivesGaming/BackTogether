using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    protected GameAudioManager mGameAudioManager;

    private void Awake()
    {
        GameObject audioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");

        if (audioMgrObj && audioMgrObj.TryGetComponent(out GameAudioManager mgr))
            mGameAudioManager = mgr;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayIntroSound();
    }

    protected virtual void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
