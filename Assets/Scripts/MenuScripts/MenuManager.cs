using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string mNextSceneName = "Level1";
    public string mStartMenuSceneName = "StartMenu";
    public string mOptionsSceneName = "Options";
    public string mCreditsSceneName = "Credits";
    protected GameAudioManager mGameAudioManager;

    protected virtual void Awake()
    {
        GameObject audioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");

        if (audioMgrObj && audioMgrObj.TryGetComponent(out GameAudioManager audioMgr))
            mGameAudioManager = audioMgr;
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayIntroSound();
    }

    protected virtual void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(mNextSceneName);
    }

    public void LoadStartMenuScene()
    {
        SceneManager.LoadScene(mStartMenuSceneName);
    }

    public void LoadOptionsScene()
    {
        SceneManager.LoadScene(mOptionsSceneName);
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene(mCreditsSceneName);
    }

    public void ExitGame()

    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

    // Update is called once per frame
    void Update()
    {

    }
}
