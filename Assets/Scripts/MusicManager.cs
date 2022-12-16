using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static bool mAlreadyExists = false;
    private bool mIsOn;
    private AudioSource mAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (mAlreadyExists)
            Destroy(gameObject);
        mAlreadyExists = true;
        DontDestroyOnLoad(gameObject);
        mIsOn = OptionsMenuManager.mMusicOn;
        if (TryGetComponent(out AudioSource audioSource))
        {
            mAudioSource = audioSource;
            mAudioSource.volume = mIsOn ? 1 : 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OptionsMenuManager.mMusicOn != mIsOn)
        {
            mIsOn = OptionsMenuManager.mMusicOn;
            if (mAudioSource)
                mAudioSource.volume = mIsOn ? 1 : 0;
        }
    }
}
