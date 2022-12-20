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

    public AudioClipTutorialIntroOutro[] mAudioClipsTutorialIntroOutro;
    public AudioClipTutorialIngame[] mAudioClipsTutorialIngame;

    private Queue<AudioClip> mAudioQueue; // for playing multiple sounds one after another

    public bool mSwitchGridAndObjectOutput = true; // for testing purposes

    private enum AudioSourcePlayType
    {
        JustPlay,               // just play the sound, without interfering with other sounds
        StopAndPlay,            // stop the currently playing sound and play sound
        StopClearQueueAndPlay, // stop the currently playing sound, delete the queue, and play sound
        JustEnqueue,            // just enqueue the sound, without interfering with other sounds
        ClearAndEnqueue        // delete the queue and enqueue the sound
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

    public enum TutorialIntroOutroSounds
    {
        None,
        C1L1_Intro,
        C1L1_Outro,
        C1L2_Intro,
        C1L2_Outro,
        C1L3_Intro,
        C1L3_Outro,
        C2L1_Intro,
        C2L1_Outro,
        C2L2_Intro,
        C2L2_Outro,
        C2L3_Intro,
        C2L3_Outro,
        C3L1_Intro,
        C3L1_Outro,
        C3L2_Intro,
        C3L2_Outro,
        C3L3_Intro,
        C3L3_Outro
    }

    public enum TutorialIngameSounds
    {
        C1L1_1,
        C1L3_1,
        C2L2_1,
        C2L2_2,
        C3L1_1,
        C3L1_2,
        C3L1_3
    }

    private void Start()
    {
        mAudioQueue = new Queue<AudioClip>();
    }

    private void Update()
    {
        if (!(mAudioSource.isPlaying) && mAudioQueue.Count > 0)
            mAudioSource.PlayOneShot(mAudioQueue.Dequeue());

        if (Input.GetKeyDown(KeyCode.X))
            mSwitchGridAndObjectOutput = !mSwitchGridAndObjectOutput;
    }
    public void PlayActionSound(ActionSounds sound)
    {
        AudioClip clip = null;
        foreach (AudioClipAction clipAction in mAudioClipsActions)
        {
            if (clipAction.sound == sound)
                clip = clipAction.audioClip;
        }

        AudioSourcePlay(clip, AudioSourcePlayType.JustPlay);
    }

    public void PlayEventSound(EventSounds sound)
    {
        AudioClip clip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == sound)
                clip = clipEvent.audioClip;
        }

        AudioSourcePlay(clip, AudioSourcePlayType.JustPlay);
    }

    public void PlayHoleFilledEvent(EventSounds theHoleAt, NavigationSounds letter, NavigationSounds number, EventSounds isClosed, EventSounds youCanCrossIt)
    {
        StopAudio();

        AudioClip theHoleAtClip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == theHoleAt)
                theHoleAtClip = clipEvent.audioClip;
        }

        AudioSourcePlay(theHoleAtClip, AudioSourcePlayType.JustEnqueue);

        AudioClip letterClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == letter)
                letterClip = clipNavigation.audioClip;
        }

        AudioSourcePlay(letterClip, AudioSourcePlayType.JustEnqueue);

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

        AudioSourcePlay(isClosedClip, AudioSourcePlayType.JustEnqueue);

        AudioClip youCanCrossItClip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == youCanCrossIt)
                youCanCrossItClip = clipEvent.audioClip;
        }

        AudioSourcePlay(youCanCrossItClip, AudioSourcePlayType.JustEnqueue);
    }

    public void PlayStonifyInformation(EventSounds generalText, NavigationSounds letter, NavigationSounds number)
    {
        StopAudio();

        AudioClip generalTextClip = null;
        foreach (AudioClipEvent clipEvent in mAudioClipsEvents)
        {
            if (clipEvent.sound == generalText)
                generalTextClip = clipEvent.audioClip;
        }

        AudioSourcePlay(generalTextClip, AudioSourcePlayType.JustEnqueue);

        AudioClip letterClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == letter)
                letterClip = clipNavigation.audioClip;
        }

        AudioSourcePlay(letterClip, AudioSourcePlayType.JustEnqueue);

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        AudioSourcePlay(numberClip, AudioSourcePlayType.JustEnqueue);
    }

    public void PlayBoneInfo(NavigationSounds number, GridObjectSounds boneGridObject)
    {
        StopAudio();

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        AudioSourcePlay(numberClip, AudioSourcePlayType.JustEnqueue);


        AudioClip gridObjClip = null;
        foreach (AudioClipGridObject clipGridObject in mAudioClipsGridObjects)
        {
            if (clipGridObject.sound == boneGridObject)
                gridObjClip = clipGridObject.audioClip;
        }

        AudioSourcePlay(gridObjClip, AudioSourcePlayType.JustEnqueue);
    }

    // TODO add "noch... zuege" clip
    public void PlayTurnsLeft(NavigationSounds number)
    {
        StopAudio();
        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        AudioSourcePlay(numberClip, AudioSourcePlayType.JustPlay);
    }

    public void PlayAudioPositionInGrid(NavigationSounds letter, NavigationSounds number, GridObjectSounds gridObject)
    {
        StopAudio();

        if (mSwitchGridAndObjectOutput)
        {
            AudioClip gridObjClip = null;
            foreach (AudioClipGridObject clipGridObject in mAudioClipsGridObjects)
            {
                if (clipGridObject.sound == gridObject)
                    gridObjClip = clipGridObject.audioClip;
            }

            AudioSourcePlay(gridObjClip, AudioSourcePlayType.JustEnqueue);
        }

        AudioClip letterClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == letter)
                letterClip = clipNavigation.audioClip;
        }

        AudioSourcePlay(letterClip, AudioSourcePlayType.JustEnqueue);

        AudioClip numberClip = null;
        foreach (AudioClipNavigation clipNavigation in mAudioClipsNavigation)
        {
            if (clipNavigation.sound == number)
                numberClip = clipNavigation.audioClip;
        }

        AudioSourcePlay(numberClip, AudioSourcePlayType.JustEnqueue);

        if (!mSwitchGridAndObjectOutput)
        {
            AudioClip gridObjClip = null;
            foreach (AudioClipGridObject clipGridObject in mAudioClipsGridObjects)
            {
                if (clipGridObject.sound == gridObject)
                    gridObjClip = clipGridObject.audioClip;
            }

            AudioSourcePlay(gridObjClip, AudioSourcePlayType.JustEnqueue);
        }

    }

    public void PlayMenuSound(StartMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipStartMenu clip in mAudioClipsStartMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        AudioSourcePlay(soundClip, AudioSourcePlayType.StopAndPlay);
    }

    public void PlayMenuSound(ChapterMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipChapterMenu clip in mAudioClipsChapterMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        AudioSourcePlay(soundClip, AudioSourcePlayType.StopAndPlay);
    }

    public void PlayMenuSound(OptionsMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipOptionsMenu clip in mAudioClipsOptionsMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        AudioSourcePlay(soundClip, AudioSourcePlayType.StopAndPlay);
    }

    public void PlayMenuSound(CreditsMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipCreditsMenu clip in mAudioClipsCreditsMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        AudioSourcePlay(soundClip, AudioSourcePlayType.StopAndPlay);
    }

    public void PlayMenuSound(PauseMenuSounds sound)
    {
        AudioClip soundClip = null;
        foreach (AudioClipPauseMenu clip in mAudioClipsPauseMenu)
        {
            if (clip.sound == sound)
                soundClip = clip.audioClip;
        }

        AudioSourcePlay(soundClip, AudioSourcePlayType.StopAndPlay);
    }

    public float PlayTutorialIntroOutroSound(TutorialIntroOutroSounds sound)
    {
        float soundLength = -1;
        AudioClip soundClip = null;
        foreach (AudioClipTutorialIntroOutro clip in mAudioClipsTutorialIntroOutro)
        {
            if (clip.sound == sound)
            {
                soundClip = clip.audioClip;
                soundLength = soundClip.length;
            }
        }

        AudioSourcePlay(soundClip, AudioSourcePlayType.StopAndPlay);
        return soundLength;
    }

    // if stopAndPlay is false, it just gets played instead
    public float PlayTutorialIngameSound(TutorialIngameSounds sound, bool stopAndPlay = true)
    {
        float soundLength = -1;
        AudioClip soundClip = null;
        foreach (AudioClipTutorialIngame clip in mAudioClipsTutorialIngame)
        {
            if (clip.sound == sound)
            {
                soundClip = clip.audioClip;
                soundLength = soundClip.length;
            }
        }
        if (stopAndPlay)
            AudioSourcePlay(soundClip, AudioSourcePlayType.StopAndPlay);
        else
            AudioSourcePlay(soundClip, AudioSourcePlayType.JustEnqueue);
        return soundLength;
    }


    // play clip in audio source, acting depending on the given AudioSourcePlayType
    private void AudioSourcePlay(AudioClip soundClip, AudioSourcePlayType playType)
    {
        if (!soundClip)
            return;

        switch (playType)
        {
            case AudioSourcePlayType.JustPlay:
                mAudioSource.PlayOneShot(soundClip);
                break;
            case AudioSourcePlayType.StopAndPlay:
                mAudioSource.Stop();
                mAudioSource.PlayOneShot(soundClip);
                break;
            case AudioSourcePlayType.StopClearQueueAndPlay:
                mAudioSource.Stop();
                mAudioQueue.Clear();
                mAudioSource.PlayOneShot(soundClip);
                break;
            case AudioSourcePlayType.JustEnqueue:
                mAudioQueue.Enqueue(soundClip);
                break;
            case AudioSourcePlayType.ClearAndEnqueue:
                mAudioQueue.Clear();
                mAudioQueue.Enqueue(soundClip);
                break;
            default:
                break;
        }
    }

    public void StopAudio(bool stopSource = true, bool clearQueue = true)
    {
        if (stopSource)
            mAudioSource.Stop();
        if (clearQueue)
            mAudioQueue.Clear();
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

    [System.Serializable]
    public class AudioClipTutorialIntroOutro
    {
        public TutorialIntroOutroSounds sound;
        public AudioClip audioClip;
    }

    [System.Serializable]
    public class AudioClipTutorialIngame
    {
        public TutorialIngameSounds sound;
        public AudioClip audioClip;
    }
}
