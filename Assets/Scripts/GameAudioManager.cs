using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoBehaviour
{
    public AudioSource mAudioSource;

    public AudioClipGridObject[] mAudioClipsGridObjects;
    public AudioClipNavigation[] mAudioClipsNavigation;
    public AudioClipEvent[] mAudioClipsEvents;
    public AudioClipAction[] mAudioClipsActions;

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
        Fünf,
        Ein,
        Oben,
        Unten,
        Rechts,
        Links
    }

    public enum EventSounds
    {
        EiVersteinertNachricht,
        VersteinerungsSound,
        RaetselPuffSound,
        DasLochBei,
        istVerschlossen,
        DuKannstDrueberlaufen,
        WeiterMitLeertaste
    }

    public enum ActionSounds
    {
        AuswahlfeldBewegen,
        AuswahlObjekt,
        AbwahlObjekt,
        Verboten,
        ObjektBewegen,
        ObjektAusgewaehltDauerhaft
    }

    // Track numbers beginning with one
    public void PlayActionSound(ActionSounds sound)
    {
        AudioClip clip = null;
        foreach (AudioClipAction clipAction in mAudioClipsActions)
        {
            if (clipAction.sound == sound)
                clip = clipAction.audioClip;
        }

        if (!clip)
            return;

        mAudioSource.PlayOneShot(clip);
    }

    public void PlayAudioPositionInGrid(NavigationSounds letter, NavigationSounds number, GridObjectSounds gridObject)
    {
        mPositionInGridQueue.Clear();

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

    [System.Serializable]
    public class AudioClipEvent
    {
        public EventSounds sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class AudioClipAction
    {
        public ActionSounds sound;
        public AudioClip audioClip;
    }
}
