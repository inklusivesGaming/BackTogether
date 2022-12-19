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

    //public static readonly string[][] mSceneNames =
    //    {
    //    new string[] { "Chapter1Level1", "Chapter1Level2", "Chapter1Level3" },
    //    new string[] { "Chapter2Level1", "Chapter2Level2", "Chapter2Level3" },
    //    new string[] { "Chapter3Level1", "Chapter3Level2", "Chapter3Level3" }
    //};

    public static readonly string[][] mSceneNames =
    {
        new string[] { "Chapter1Level1"},
        new string[] { "Chapter2Level1"},
        new string[] { "Chapter3Level1"}
    };

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
                    // next scene in same chapter
                    return currentChapter[lvl + 1];

                else if (mSceneNames.Length > i + 1)
                    // next scene in next chapter
                    return mSceneNames[i + 1][0];
                else
                    // no next scene
                    return currentScene;
            }
        }
        //scene doesn't exist
        return currentScene;
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
