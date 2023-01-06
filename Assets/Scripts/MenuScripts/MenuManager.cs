using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    protected GameAudioManager mGameAudioManager;
    protected EventSystem mEventSystem;
    protected GameObject mSelectedUIObject = null;

    [Tooltip("This button can only be selected once")]
    public Button mMenuHeadButton;

    protected virtual void Awake()
    {
        GameObject audioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioMgrObj && audioMgrObj.TryGetComponent(out GameAudioManager audioMgr))
            mGameAudioManager = audioMgr;

        GameObject eventSystemObj = GameObject.Find("EventSystem");
        if (eventSystemObj && eventSystemObj.TryGetComponent(out EventSystem eventSystem))
            mEventSystem = eventSystem;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        PlayIntroSound();
    }


    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            mGameAudioManager.StopAudio();
        HandleEventSystemSelection();
    }

    // If you click with mouse, you dont want to deselect the current menu selection
    protected virtual void HandleEventSystemSelection()
    {
        if (!mEventSystem)
            return;

        GameObject selectedObj = mEventSystem.currentSelectedGameObject;
        if (selectedObj != mSelectedUIObject)
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                mEventSystem.SetSelectedGameObject(mSelectedUIObject);
            else if (mMenuHeadButton && mSelectedUIObject == mMenuHeadButton.gameObject)
                // menu head button can only be selected once
                mMenuHeadButton.interactable = false;

   
        mSelectedUIObject = mEventSystem.currentSelectedGameObject;
    }

    protected virtual void PlayIntroSound()
    {
        if (!mGameAudioManager)
            return;
    }

    public virtual void LoadStartMenuScene()
    {
        SceneManager.LoadScene(GlobalVariables.mStartMenuSceneName);
    }

    public virtual void LoadChapterMenuScene()
    {
        SceneManager.LoadScene(GlobalVariables.mChapterMenuSceneName);
    }

    public virtual void LoadOptionsMenuScene()
    {
        SceneManager.LoadScene(GlobalVariables.mOptionsMenuSceneName);
    }

    public virtual void LoadCreditsMenuScene()
    {
        SceneManager.LoadScene(GlobalVariables.mCreditsMenuSceneName);
    }

    public void ExitGame()

    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
