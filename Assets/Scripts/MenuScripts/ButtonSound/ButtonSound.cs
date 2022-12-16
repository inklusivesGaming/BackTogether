using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, ISelectHandler
{
    protected GameAudioManager mGameAudioManager = null;
    // define your menu sound enum


    protected virtual void Awake()
    {
        GameObject audioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");

        if (audioMgrObj && audioMgrObj.TryGetComponent(out GameAudioManager mgr))
            mGameAudioManager = mgr;
    }

    public virtual void OnSelect(BaseEventData eventData)
    {
        if (!mGameAudioManager)
            return;
        // mGameAudioManager.PlayMenuSound(yourmenusoundenum);
    }
}
