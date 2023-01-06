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

    public static bool mCurrentSceneReloaded = false; // set true if the current scene already got reloaded (no new intro)

    private const string FINISHED_SCENE_STRING = "FINISHED"; // gets returned if there is no next scene; shouldnt happen normally

    public const string TAG_GROUND = "Ground"; // tag for the ground object
    public const string TAG_DECOCONTAINER = "DecoContainer"; // tag for the deco container objects
    public const string TAG_DECOGROUNDCONTAINER = "DecoGroundsContainer"; // tag for the deco ground container objects

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
                return FINISHED_SCENE_STRING;
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

    public static void OnSceneReload()
    {
        mCurrentSceneReloaded = true;
    }

    public static string GetCurrentScene()
    {
        if (mCurrentChapter > mSceneNames.Length || mCurrentLevel > mSceneNames[mCurrentChapter].Length)
            return "";
        return mSceneNames[mCurrentChapter - 1][mCurrentLevel - 1];
    }

    // if intro == true, return intro for current level; else return outro for current level
    public static GameAudioManager.TutorialIntroOutroSounds GetTutorialSound(bool intro)
    {
        //if (intro && mCurrentSceneReloaded)
            // TODO insert special reload sound or vo!
            //return GameAudioManager.TutorialIntroOutroSounds.None;

        if (intro)
        {
            if (mCurrentChapter > mTutorialIntros.Length || mCurrentLevel > mTutorialIntros[mCurrentChapter - 1].Length)
                return GameAudioManager.TutorialIntroOutroSounds.None;
            return mTutorialIntros[mCurrentChapter - 1][mCurrentLevel - 1];
        }
        else
        {
            if (mCurrentChapter > mTutorialOutros.Length || mCurrentLevel > mTutorialOutros[mCurrentChapter - 1].Length)
                return GameAudioManager.TutorialIntroOutroSounds.None;
            return mTutorialOutros[mCurrentChapter - 1][mCurrentLevel - 1];
        }
    }

    // 0,0 for start menu
    public static void SetLevel(int chapter, int level)
    {
        mCurrentChapter = chapter;
        mCurrentLevel = level;
        mCurrentSceneReloaded = false;
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
