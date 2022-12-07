using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public AudioSource mAudioSource;

    public AudioClipGridObject[] mAudioClipsGridObjects;
    public AudioClipNavigation[] mAudioClipsNavigation;

    private Queue<AudioClip> mPositionInGridQueue;

    private void Start()
    {
        mPositionInGridQueue = new Queue<AudioClip>();
    }

    private void Update()
    {
        if (!(mAudioSource.isPlaying) && mPositionInGridQueue.Count > 0)
            mAudioSource.PlayOneShot(mPositionInGridQueue.Dequeue());
    }

    public enum GridObjectSounds
    {
        Schnuppi,
        Dinja,
        frei,
        Steinhaufen,
        Ei,
        Knochen,
        Raetselkiste,
        Loch
    }

    public enum NavigationSounds
    {
        A,
        B,
        C,
        D,
        E,
        Null,
        Eins,
        Zwei,
        Drei,
        Vier,
        F�nf,
        Ein,
        Oben,
        Unten,
        Rechts,
        Links
    }

    public enum EventSounds
    {
        EiVersteinert,
        VersteinerungsSound,
        RaetselPuffSound,
        DasLochBei,
        istVerschlossen,
        DuKannstDrueberlaufen,
        WeiterMitLeertaste
    }

    // Track numbers beginning with one
    public void PlayAudioGridObject(GridObjectSounds sound)
    {
        AudioClip clip = null;
        foreach (AudioClipGridObject clipGridObject in mAudioClipsGridObjects)
        {
            if (clipGridObject.sound == sound)
                clip = clipGridObject.audioClip;
        }

        if (!clip)
            return;

        mAudioSource.PlayOneShot(clip);
    }

    public void PlayAudioPositionInGrid(NavigationSounds letter, NavigationSounds number, GridObjectSounds gridObject)
    {
        mPositionInGridQueue.Clear();

        print(letter);
        print(number);
        print(gridObject);

        AudioClip letterClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == letter)
                letterClip = clipNavigation.audioClip;
        }

        if (letterClip)
            mPositionInGridQueue.Enqueue(letterClip);

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        if (numberClip)
            mPositionInGridQueue.Enqueue(numberClip);

        AudioClip gridObjClip = null;
        foreach (AudioClipGridObject clipGridObject in mAudioClipsGridObjects)
        {
            if (clipGridObject.sound == gridObject)
                gridObjClip = clipGridObject.audioClip;
        }

        if (gridObjClip)
            mPositionInGridQueue.Enqueue(gridObjClip);
    }

    [System.Serializable]
    public class AudioClipGridObject
    {
        public GridObjectSounds sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class AudioClipNavigation
    {
        public NavigationSounds sound;
        public AudioClip audioClip;
    }
}
