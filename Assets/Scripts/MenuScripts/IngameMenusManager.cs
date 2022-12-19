using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IngameMenusManager : OptionsMenuManager
{
    private enum CurrentGameMenuState
    {
        Ingame,
        PauseMenu,
        OptionsMenu,
        WinMenu
    }

    private CurrentGameMenuState mCurrentGameMenuState = CurrentGameMenuState.Ingame;

    private GameManager mGameManager;

    public GameObject mPauseMenu;
    public GameObject mOptionsMenu;
    public GameObject mWinMenu;

    public Button mPauseMenuHeadButton;
    public Button mOptionsMenuHeadButton;
    public Button mWinMenuHeadButton;

    protected override void Awake()
    {
        base.Awake();

        GameObject gameMgrObj = GameObject.FindGameObjectWithTag("GameManager");
        if (gameMgrObj && gameMgrObj.TryGetComponent(out GameManager gameMgr))
            mGameManager = gameMgr;

    }

    protected override void Start()
    {
        base.Start();
        if (mGameManager)
            mGameManager.RegisterIngameMenusManager(this);
    }

    // Don't want to play intro sound at start
    protected override void PlayIntroSound() { }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (mCurrentGameMenuState == CurrentGameMenuState.WinMenu)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            if (mCurrentGameMenuState == CurrentGameMenuState.PauseMenu
                || mCurrentGameMenuState == CurrentGameMenuState.OptionsMenu)
            {

                Resume();
            }

            else if (mCurrentGameMenuState == CurrentGameMenuState.Ingame)
                Pause();
    }

    // Don't want to do this while ingame
    protected override void HandleEventSystemSelection()
    {
        if (mCurrentGameMenuState == CurrentGameMenuState.Ingame)
            return;
        base.HandleEventSystemSelection();
    }

    public void Pause()
    {
        mPauseMenu.SetActive(true);
        mMenuHeadButton = mPauseMenuHeadButton;

        if (mGameAudioManager)
            mGameAudioManager.PlayMenuSound(GameAudioManager.PauseMenuSounds.Intro);

        Time.timeScale = 0f;

        if (mGameManager)
        {
            mGameManager.PauseGame();
            mGameManager.gameObject.SetActive(false);
        }
        mCurrentGameMenuState = CurrentGameMenuState.PauseMenu;

        mEventSystem.SetSelectedGameObject(mPauseMenuHeadButton.gameObject);
    }

    public void Resume()
    {
        mPauseMenuHeadButton.interactable = true;
        mOptionsMenuHeadButton.interactable = true;
        if (mEventSystem)
            mEventSystem.SetSelectedGameObject(mPauseMenuHeadButton.gameObject);
        mPauseMenu.SetActive(false);
        mOptionsMenu.SetActive(false);
        Time.timeScale = 1f;

        if (mGameManager)
        {
            mGameManager.gameObject.SetActive(true);
            mGameManager.ResumeGame();
        }
        mCurrentGameMenuState = CurrentGameMenuState.Ingame;
    }

    // If true, switch from pause menu to options, else from options to pause menu
    public void SwitchMenu(bool pauseToOptions)
    {
        if (pauseToOptions)
        {
            mPauseMenuHeadButton.interactable = true;
            mPauseMenu.SetActive(false);
            mOptionsMenu.SetActive(true);
            mMenuHeadButton = mOptionsMenuHeadButton;

            if (mEventSystem)
                mEventSystem.SetSelectedGameObject(mOptionsMenuHeadButton.gameObject);
            if (mGameAudioManager)
                mGameAudioManager.PlayMenuSound(GameAudioManager.OptionsMenuSounds.Intro);

            mCurrentGameMenuState = CurrentGameMenuState.OptionsMenu;
        }
        else
        {
            mOptionsMenuHeadButton.interactable = true;
            mOptionsMenu.SetActive(false);
            mPauseMenu.SetActive(true);
            mMenuHeadButton = mPauseMenuHeadButton;

            if (mEventSystem)
                mEventSystem.SetSelectedGameObject(mPauseMenuHeadButton.gameObject);
            if (mGameAudioManager)
                mGameAudioManager.PlayMenuSound(GameAudioManager.PauseMenuSounds.Intro);

            mCurrentGameMenuState = CurrentGameMenuState.PauseMenu;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Win()
    {
        mCurrentGameMenuState = CurrentGameMenuState.WinMenu;

        if (mGameAudioManager)
            mGameAudioManager.PlayEventSound(GameAudioManager.EventSounds.LevelGeschafft);

        mWinMenu.SetActive(true);
        mMenuHeadButton = mWinMenuHeadButton;
        mEventSystem.SetSelectedGameObject(mWinMenuHeadButton.gameObject);

        Time.timeScale = 0f;
    }

    public void LoadNextScene()
    {
        if (!mGameManager)
            return;
        mGameManager.LoadNextScene();
    }
}
