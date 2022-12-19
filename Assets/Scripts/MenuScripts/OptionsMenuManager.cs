using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsMenuManager : MenuManager
{
    public Button mOptionsMusicButton;
    public Button mOptionsAudioDescriptionButton;
    public Button mOptionsContrastButton;



    protected override void Start()
    {
        base.Start();
        SetOptionsButtons();
    }

    public void SetOptionsButtons()
    {
        SetButton(mOptionsMusicButton, GlobalVariables.mOptionMusicOn, false, GlobalVariables.mOptionMusicOnText, GlobalVariables.mOptionMusicOffText);
        SetButton(mOptionsAudioDescriptionButton, GlobalVariables.mOptionAudioDescriptionOn, false, GlobalVariables.mOptionAudioDescriptionOnText, GlobalVariables.mOptionAudioDescriptionOffText);
        SetButton(mOptionsContrastButton, GlobalVariables.mOptionContrastOn, false, GlobalVariables.mOptionContrastOnText, GlobalVariables.mOptionContrastOffText);
    }

    protected override void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;

        mGameAudioManager.PlayMenuSound(GameAudioManager.OptionsMenuSounds.Intro);
    }

    public void MusicButton()
    {
        GlobalVariables.mOptionMusicOn = !GlobalVariables.mOptionMusicOn;
        SetButton(mOptionsMusicButton, GlobalVariables.mOptionMusicOn, true, GlobalVariables.mOptionMusicOnText, GlobalVariables.mOptionMusicOffText);
    }

    public void AudioDescriptionButton()
    {
        GlobalVariables.mOptionAudioDescriptionOn = !GlobalVariables.mOptionAudioDescriptionOn;
        SetButton(mOptionsAudioDescriptionButton, GlobalVariables.mOptionAudioDescriptionOn, true, GlobalVariables.mOptionAudioDescriptionOnText, GlobalVariables.mOptionAudioDescriptionOffText);
    }

    public void ContrastButton()
    {
        GlobalVariables.mOptionContrastOn = !GlobalVariables.mOptionContrastOn;
        SetButton(mOptionsContrastButton, GlobalVariables.mOptionContrastOn, true, GlobalVariables.mOptionContrastOnText, GlobalVariables.mOptionContrastOffText);
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
