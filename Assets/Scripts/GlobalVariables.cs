using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables
{
    // Scene names for scenes that we will switch to
    public const string mStartMenuSceneName = "StartMenu";
    public const string mChapterMenuSceneName = "ChapterMenu";
    public const string mOptionsMenuSceneName = "OptionsMenu";
    public const string mCreditsMenuSceneName = "CreditsMenu";

    // Chapters that can be loaded in chapter menu
    //public static readonly string[] ChapterSceneNames = {"Level1", "Level2", "Level3" };

    public static readonly string[][] mSceneNames =
        {
        new string[] { "Chapter1Level1", "Chapter1Level2", "Chapter1Level3" },
        new string[] { "Chapter2Level1", "Chapter2Level2", "Chapter2Level3" },
        new string[] { "Chapter3Level1", "Chapter3Level2", "Chapter3Level3" }
    };

    // Contains all intro sounds
    public static readonly GameAudioManager.TutorialIntroOutroSounds[][] mTutorialIntros =
    {
        new GameAudioManager.TutorialIntroOutroSounds[] { GameAudioManager.TutorialIntroOutroSounds.C1L1_Intro, GameAudioManager.TutorialIntroOutroSounds.C1L2_Intro, GameAudioManager.TutorialIntroOutroSounds.C1L3_Intro },
        new GameAudioManager.TutorialIntroOutroSounds[] { GameAudioManager.TutorialIntroOutroSounds.C2L1_Intro, GameAudioManager.TutorialIntroOutroSounds.C2L2_Intro, GameAudioManager.TutorialIntroOutroSounds.C2L3_Intro },
        new GameAudioManager.TutorialIntroOutroSounds[] { GameAudioManager.TutorialIntroOutroSounds.C3L1_Intro, GameAudioManager.TutorialIntroOutroSounds.C3L2_Intro, GameAudioManager.TutorialIntroOutroSounds.C3L3_Intro }
    };

    // Contains all outro sounds
    public static readonly GameAudioManager.TutorialIntroOutroSounds[][] mTutorialOutros =
    {
        new GameAudioManager.TutorialIntroOutroSounds[] { GameAudioManager.TutorialIntroOutroSounds.C1L1_Outro, GameAudioManager.TutorialIntroOutroSounds.C1L2_Outro, GameAudioManager.TutorialIntroOutroSounds.C1L3_Outro },
        new GameAudioManager.TutorialIntroOutroSounds[] { GameAudioManager.TutorialIntroOutroSounds.C2L1_Outro, GameAudioManager.TutorialIntroOutroSounds.C2L2_Outro, GameAudioManager.TutorialIntroOutroSounds.C2L3_Outro },
        new GameAudioManager.TutorialIntroOutroSounds[] { GameAudioManager.TutorialIntroOutroSounds.C3L1_Outro, GameAudioManager.TutorialIntroOutroSounds.C3L2_Outro, GameAudioManager.TutorialIntroOutroSounds.C3L3_Outro }
    };

    public static int mCurrentChapter = 0; // 0 means start menu
    public static int mCurrentLevel = 0; // 0 means start menu

    private const string mFinishedSceneString = "FINISHED"; // gets returned if there is no next scene; shouldnt happen normally

    public static string GetNextScene()
    {
        if (mCurrentChapter > mSceneNames.Length)
        {
            // shouldnt happen
            SetLevel(1, 1);
            return mSceneNames[0][0];
        }
        if (mCurrentLevel >= mSceneNames[mCurrentChapter - 1].Length)
        {
            if (mCurrentChapter == mSceneNames.Length)
                // if there is no other scene; shouldnt happen
                return mFinishedSceneString;
            else
            {
                // next chapter, first level
                SetLevel(mCurrentChapter + 1, 1);
                return mSceneNames[mCurrentChapter - 1][mCurrentLevel - 1];
            }
        }
        // next level in same chapter

        SetLevel(mCurrentChapter, mCurrentLevel + 1);
        return mSceneNames[mCurrentChapter - 1][mCurrentLevel - 1];
    }

    // obsolete
    public static string GetNextScene(string currentScene)
    {
        for (int i = 0; i < mSceneNames.Length; i++)
        {
            string[] currentChapter = mSceneNames[i];
            int lvl = -1;
            for (int j = 0; j < currentChapter.Length; j++)
            {
                if (currentChapter[j] == currentScene)
                {
                    lvl = j;
                    break;
                }
            }
            if (lvl != -1)
            // scene found in array
            {
                if (currentChapter.Length > lvl + 1)
                {
                    // next scene in same chapter
                    mCurrentLevel++;
                    return currentChapter[lvl + 1];
                }


                else if (mSceneNames.Length > i + 1)
                {
                    // next scene in next chapter
                    mCurrentLevel = 1;
                    mCurrentChapter++;
                    return mSceneNames[i + 1][0];
                }

                else
                    // no next scene
                    return mFinishedSceneString;
            }
        }
        //scene doesn't exist
        return currentScene;
    }

    // 0,0 for start menu
    public static void SetLevel(int chapter, int level)
    {
        mCurrentChapter = chapter;
        mCurrentLevel = level;
    }


    // Texts for buttons in options menu
    public const string mOptionMusicOnText = "Musik: Ein";
    public const string mOptionMusicOffText = "Musik: Aus";
    public const string mOptionAudioDescriptionOnText = "Audiobeschreibung: Ein";
    public const string mOptionAudioDescriptionOffText = "Audiobeschreibung: Aus";
    public const string mOptionContrastOnText = "Kontrastverstärkung: Ein";
    public const string mOptionContrastOffText = "Kontrastverstärkung: Aus";

    public static bool mOptionMusicOn = true;
    public static bool mOptionAudioDescriptionOn = true;
    public static bool mOptionContrastOn = false;
}
