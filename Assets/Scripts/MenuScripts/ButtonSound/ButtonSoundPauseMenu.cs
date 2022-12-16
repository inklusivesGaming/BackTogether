using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundPauseMenu : ButtonSound
{
    public GameAudioManager.PauseMenuSounds mPauseMenuSound;

    public override void OnSelect(BaseEventData eventData)
    {
        if (!mGameAudioManager)
            return;
        mGameAudioManager.PlayMenuSound(mPauseMenuSound);
    }
}
