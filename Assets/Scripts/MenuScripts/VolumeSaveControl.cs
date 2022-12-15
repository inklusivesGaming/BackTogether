using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSaveControl : MonoBehaviour
{
    public static bool mFirstLoad = true;

    [SerializeField] private Slider volumeSlider = null;

    [SerializeField] private TextMeshProUGUI volumeTextUI = null;

    private void Start()
    {
        LoadValues();
    }

    public void VolumeSlider(float volume)
    {
        volumeTextUI.text = volume.ToString("0.0");
    }

    public void SaveVolumeButton()
    {
        float volumeValue = volumeSlider.value;
        PlayerPrefs.SetFloat("VolumeValue", volumeValue);
        LoadValues();
    }

    void LoadValues()
    {
        float volumeValue = 1f;
        if (!mFirstLoad)
            volumeValue = PlayerPrefs.GetFloat("VolumeValue");
        else
        {
            PlayerPrefs.SetFloat("VolumeValue", volumeValue);
            mFirstLoad = false;
        }
        volumeSlider.value = volumeValue;
        AudioListener.volume = volumeValue;
    }

}
