using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundStartMenu : ButtonSound
{
    public GameAudioManager.StartMenuSounds mStartMenuSound;

    public override void OnSelect(BaseEventData eventData)
    {
        if (!mGameAudioManager)
            return;
        mGameAudioManager.PlayMenuSound(mStartMenuSound);
    }
}
