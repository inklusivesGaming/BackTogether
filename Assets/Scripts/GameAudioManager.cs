using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public List<AudioClip> mAudioGridObjects;
    public List<AudioClip> mAudioAlphabet;
    public List<AudioClip> mAudioNumbers;
    public List<AudioClip> mAudioOther;

    public AudioSource mIngameAudioSource;
    public AudioSource mFinalScreenAudioSource;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Track numbers beginning with one
    private void PlayGridObjectsAudio(int trackNumber)
    {
        //if (mAudioClips.Count < trackNumber)
        //    return;
        //mAudioSource.PlayOneShot(mAudioClips[trackNumber - 1]);
    }
}
