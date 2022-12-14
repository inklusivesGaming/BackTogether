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

    public AudioClipStartMenu[] mAudioClipsStartMenu;
    public AudioClipChapterMenu[] mAudioClipsChapterMenu;
    public AudioClipOptionsMenu[] mAudioClipsOptionsMenu;
    public AudioClipCreditsMenu[] mAudioClipsCreditsMenu;
    public AudioClipPauseMenu[] mAudioClipsPauseMenu;

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
        ObjektAusgewaehltDauerhaft,
        KnochenGesammelt,
        SteinZerstoert
    }

    public enum StartMenuSounds
    {
        Intro,
        Spielstart,
        Barrierefreiheit,
        Credits,
        Exit
    }

    public enum ChapterMenuSounds
    {
        Intro,
        Exit,
        Kap1,
        Kap2,
        Kap3
    }

    public enum OptionsMenuSounds
    {
        Intro,
        MusikOffOn,
        MusikOnOff,
        KontrastOffOn,
        KontrastOnOff,
        AudiobeschreibungOffOn,
        AudiobeschreibungOnOff,
        Exit
    }

    public enum CreditsMenuSounds
    {
        Mitgewirkt
    }

    public enum PauseMenuSounds
    {
        Intro,
        BackGame,
        RestartLevel,
        Barrierefreiheitsmenue,
        Exit
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

    public void PlayHoleFilledEvent(EventSounds theHoleAt, NavigationSounds letter, NavigationSounds number, EventSounds isClosed, EventSounds youCanCrossIt)
    {
        mAudioQueue.Clear();

        AudioClip theHoleAtClip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == theHoleAt)
                theHoleAtClip = clipEvent.audioClip;
        }

        if (theHoleAtClip)
            mAudioSource.PlayOneShot(theHoleAtClip);

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

        AudioClip isClosedClip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == isClosed)
                isClosedClip = clipEvent.audioClip;
        }

        if (isClosedClip)
            mAudioQueue.Enqueue(isClosedClip);

        AudioClip youCanCrossItClip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == youCanCrossIt)
                youCanCrossItClip = clipEvent.audioClip;
        }

        if (youCanCrossItClip)
            mAudioQueue.Enqueue(youCanCrossItClip);
    }

    public void PlayStonifyInformation(EventSounds generalText, NavigationSounds letter, NavigationSounds number)
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

    public void PlayMenuSound(StartMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipStartMenu clip in mAudioClipsStartMenu)
        {
            if (clip.sound == sound)
                soundClip= clip.audioClip;
        }

        if (soundClip)
            mAudioSource.PlayOneShot(soundClip);
    }

    public void PlayMenuSound(ChapterMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipChapterMenu clip in mAudioClipsChapterMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        if (soundClip)
            mAudioSource.PlayOneShot(soundClip);
    }

    public void PlayMenuSound(OptionsMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipOptionsMenu clip in mAudioClipsOptionsMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        if (soundClip)
            mAudioSource.PlayOneShot(soundClip);
    }


    public void PlayMenuSound(CreditsMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipCreditsMenu clip in mAudioClipsCreditsMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        if (soundClip)
            mAudioSource.PlayOneShot(soundClip);
    }

    public void PlayMenuSound(PauseMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipPauseMenu clip in mAudioClipsPauseMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        if (soundClip)
            mAudioSource.PlayOneShot(soundClip);
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

    [System.Serializable]
    public class AudioClipStartMenu
    {
        public StartMenuSounds sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class AudioClipChapterMenu
    {
        public ChapterMenuSounds sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class AudioClipOptionsMenu
    {
        public OptionsMenuSounds sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class AudioClipCreditsMenu
    {
        public CreditsMenuSounds sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class AudioClipPauseMenu
    {
        public PauseMenuSounds sound;
        public AudioClip audioClip;
    }
}
