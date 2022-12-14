using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSoundStartMenu : MonoBehaviour, ISelectHandler
{
    public GameAudioManager mGameAudioManager;
    public GameAudioManager.StartMenuSounds mStartMenuSound;

    public void OnSelect(BaseEventData eventData)
    {
        mGameAudioManager.PlayMenuSound(mStartMenuSound);
    }
}
