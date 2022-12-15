using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSoundStartMenu : MonoBehaviour, ISelectHandler
{
    private GameAudioManager mGameAudioManager = null;
    public GameAudioManager.StartMenuSounds mStartMenuSound;

    private void Awake()
    {

        GameObject audioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");

        if (audioMgrObj && audioMgrObj.TryGetComponent(out GameAudioManager mgr))
            mGameAudioManager = mgr;
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!mGameAudioManager)
            return;
        mGameAudioManager.PlayMenuSound(mStartMenuSound);
    }
}
