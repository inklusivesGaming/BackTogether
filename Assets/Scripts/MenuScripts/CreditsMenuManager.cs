using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuManager : MenuManager
{
    protected override void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;

        mGameAudioManager.PlayMenuSound(GameAudioManager.CreditsMenuSounds.Mitgewirkt);
        mGameAudioManager.EnqueueMenuSound(GameAudioManager.CreditsMenuSounds.ZurueckMitLeertaste);
    }
}
