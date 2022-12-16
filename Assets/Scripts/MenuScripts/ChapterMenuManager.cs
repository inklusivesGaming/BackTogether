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

    public void LoadChapter(int number)
    {
        switch (number)
        {
            default:
                break;
            case 1:
                SceneManager.LoadScene(mChapterOneName);
                break;
            case 2:
                SceneManager.LoadScene(mChapterTwoName);
                break;
            case 3:
                SceneManager.LoadScene(mChapterThreeName);
                break;
        }
    }
}
