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

    private Queue<AudioClip> mAudioQueue; // for playing multiple sounds one after another

    private void Start()
    {
        mAudioQueue = new Queue<AudioClip>();
    }

    private void Update()
    {
        if (!(mAudioSource.isPlaying) && mAudioQueue.Count > 0)
            mAudioSource.PlayOneShot(mAudioQueue.Dequeue());
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
        WeiterMitLeertaste,
        LevelGeschafft
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

    public void PlayEventSound(EventSounds sound)
    {
        AudioClip clip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == sound)
                clip = clipEvent.audioClip;
        }

        if (!clip)
            return;

        mAudioSource.PlayOneShot(clip);
    }

    public void PlayStonifyInformation (EventSounds generalText, NavigationSounds letter, NavigationSounds number)
    {
        mAudioQueue.Clear();

        AudioClip generalTextClip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == generalText)
                generalTextClip = clipEvent.audioClip;
        }

        if (generalTextClip)
            mAudioSource.PlayOneShot(generalTextClip);

        AudioClip letterClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == letter)
                letterClip = clipNavigation.audioClip;
        }

        if (letterClip)
            mAudioQueue.Enqueue(letterClip);

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        if (numberClip)
            mAudioQueue.Enqueue(numberClip);


    }

    public void PlayBoneInfo(NavigationSounds number, GridObjectSounds boneGridObject)
    {
        mAudioQueue.Clear();

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        if (numberClip)
            mAudioSource.PlayOneShot(numberClip);


        AudioClip gridObjClip = null;
        foreach (AudioClipGridObject clipGridObject in mAudioClipsGridObjects)
        {
            if (clipGridObject.sound == boneGridObject)
                gridObjClip = clipGridObject.audioClip;
        }

        if (gridObjClip)
            mAudioQueue.Enqueue(gridObjClip);
    }

    // TODO add "noch... zuege" clip
    public void PlayTurnsLeft(NavigationSounds number)
    {
        mAudioQueue.Clear();

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        if (numberClip)
            mAudioSource.PlayOneShot(numberClip);
    }

    public void PlayAudioPositionInGrid(NavigationSounds letter, NavigationSounds number, GridObjectSounds gridObject)
    {
        mAudioQueue.Clear();

        AudioClip letterClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == letter)
                letterClip = clipNavigation.audioClip;
        }

        if (letterClip)
            mAudioSource.PlayOneShot(letterClip);

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        if (numberClip)
            mAudioQueue.Enqueue(numberClip);

        AudioClip gridObjClip = null;
        foreach (AudioClipGridObject clipGridObject in mAudioClipsGridObjects)
        {
            if (clipGridObject.sound == gridObject)
                gridObjClip = clipGridObject.audioClip;
        }

        if (gridObjClip)
            mAudioQueue.Enqueue(gridObjClip);
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
