using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuManager : MenuManager
{
    protected override void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;

        // TODO we need a "press ... to go back"
        mGameAudioManager.PlayMenuSound(GameAudioManager.CreditsMenuSounds.Mitgewirkt);
    }
}
