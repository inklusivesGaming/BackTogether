using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSoundOptionsMenu : ButtonSound
{
    public GameAudioManager.OptionsMenuSounds mOptionsMenuSoundOnOff;
    public GameAudioManager.OptionsMenuSounds mOptionsMenuSoundOffOn;

    public bool mIsOn;

    public override void OnSelect(BaseEventData eventData)
    {
        if (!mGameAudioManager)
            return;
        mGameAudioManager.PlayMenuSound(mIsOn ? mOptionsMenuSoundOnOff : mOptionsMenuSoundOffOn);

    }

    public void SwitchOnOff(bool on, bool makeSound)
    {
        mIsOn = on;
        if (!mGameAudioManager || !makeSound)
            return;
        mGameAudioManager.PlayMenuSound(mIsOn ? mOptionsMenuSoundOnOff : mOptionsMenuSoundOffOn);
    }
}
