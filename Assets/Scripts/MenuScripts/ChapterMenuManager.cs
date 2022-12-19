using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChapterMenuManager : MenuManager
{
    public string mChapterOneName = "Level1";
    public string mChapterTwoName = "Level2";
    public string mChapterThreeName = "Level3";

    protected override void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;

        mGameAudioManager.PlayMenuSound(GameAudioManager.ChapterMenuSounds.Intro);
    }

    // Starting with 1
    public void LoadChapter(int number)
    {
        if (number > GlobalVariables.ChapterSceneNames.Length)
            return;

        SceneManager.LoadScene(GlobalVariables.ChapterSceneNames[number]);
    }
}
