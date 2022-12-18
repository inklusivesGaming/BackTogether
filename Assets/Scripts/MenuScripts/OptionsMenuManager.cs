using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsMenuManager : MenuManager
{
    public Button mMusicButton;
    public Button mAudioDescriptionButton;
    public Button mContrastButton;

    [Tooltip("Set to true if this is the ingame options menu")]
    public bool mIsIngame = false;

    public static bool mMusicOn = true;
    public static bool mAudioDescriptionOn = true;
    public static bool mContrastOn = false;

    public const string mMusicOnText = "Musik: Ein";
    public const string mMusicOffText = "Musik: Aus";
    public string mAudioDescriptionOnText = "Audiobeschreibung: Ein";
    public string mAudioDescriptionOffText = "Audiobeschreibung: Aus";
    public string mContrastOnText = "Kontrastverstärkung: Ein";
    public string mContrastOffText = "Kontrastverstärkung: Aus";

    protected override void Start()
    {
        base.Start();
        SetOptionsButtons();
    }

    public void SetOptionsButtons()
    {
        SetButton(mMusicButton, mMusicOn, false, mMusicOnText, mMusicOffText);
        SetButton(mAudioDescriptionButton, mAudioDescriptionOn, false, mAudioDescriptionOnText, mAudioDescriptionOffText);
        SetButton(mContrastButton, mContrastOn, false, mContrastOnText, mContrastOffText);
    }

    protected override void PlayIntroSound()
    {
        if (!mGameAudioManager || mIsIngame)
            return;

        mGameAudioManager.PlayMenuSound(GameAudioManager.OptionsMenuSounds.Intro);
    }

    public void MusicButton()
    {
        mMusicOn = !mMusicOn;
        SetButton(mMusicButton, mMusicOn, true, mMusicOnText, mMusicOffText);
    }

    public void AudioDescriptionButton()
    {
        mAudioDescriptionOn = !mAudioDescriptionOn;
        SetButton(mAudioDescriptionButton, mAudioDescriptionOn, true, mAudioDescriptionOnText, mAudioDescriptionOffText);
    }

    public void ContrastButton()
    {
        mContrastOn = !mContrastOn;
        SetButton(mContrastButton, mContrastOn, true, mContrastOnText, mContrastOffText);
    }

    public void SetButton(Button button, bool buttonOn, bool playSound, string buttonOnText, string buttonOffText)
    {
        if (button.gameObject.TryGetComponent(out ButtonSoundOptionsMenu soundComponent))
            soundComponent.SwitchOnOff(buttonOn, playSound);

        TextMeshProUGUI text = button.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        if (text)
            text.text = buttonOn ? buttonOnText : buttonOffText;
    }
}
