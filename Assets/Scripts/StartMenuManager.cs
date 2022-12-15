using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    private GameAudioManager mGameAudioManager;

    private void Awake()
    {
        GameObject audioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");

        if (audioMgrObj && audioMgrObj.TryGetComponent(out GameAudioManager mgr))
            mGameAudioManager = mgr;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(mGameAudioManager)
            mGameAudioManager.PlayMenuSound(GameAudioManager.StartMenuSounds.Intro);
    }
}
