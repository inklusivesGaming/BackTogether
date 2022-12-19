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
    public static readonly string[] ChapterSceneNames = {"Level1", "Level2", "Level3" };

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
