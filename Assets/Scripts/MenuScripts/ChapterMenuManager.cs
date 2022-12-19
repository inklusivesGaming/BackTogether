using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterMenuManager : MenuManager
{
    protected override void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;

        mGameAudioManager.PlayMenuSound(GameAudioManager.ChapterMenuSounds.Intro);
    }

    // Starting with 1
    public void LoadChapter(int number)
    {
        if (number <= 0 || number > GlobalVariables.ChapterSceneNames.Length)
            return;

        SceneManager.LoadScene(GlobalVariables.ChapterSceneNames[number - 1]);
    }
}
