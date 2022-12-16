using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuManager : MenuManager
{
    protected override void Start()
    {
        base.Start();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;

        mGameAudioManager.PlayMenuSound(GameAudioManager.StartMenuSounds.Intro);
    }
}
