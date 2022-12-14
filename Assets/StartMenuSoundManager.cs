using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuSoundManager : MonoBehaviour
{
    public GameAudioManager mGameAudioManager;

    // Start is called before the first frame update
    void Start()
    {
        mGameAudioManager.PlayMenuSound(GameAudioManager.StartMenuSounds.Intro);
    }

    // Update is called once per frame
    void Update()
    {

    }


    //[System.Serializable]
    //class ButtonSoundStartMenu
    //{
    //    public Button button;
    //    public GameAudioManager.StartMenuSounds sound ;
    //}

}
