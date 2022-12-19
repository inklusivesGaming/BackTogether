using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IngameMenusManager : MonoBehaviour
{
    private GameManager mGameManager;
    private bool mPause = false;

    public GameObject mPauseMenu;
    public GameObject mOptionsMenu;
    public GameObject mWinMenu;

    public Button mPauseMenuHeadButton;
    public Button mOptionsMenuHeadButton;
    public Button mWinMenuHeadButton;

    private EventSystem mEventSystem;
    private GameAudioManager mGameAudioManager;

    private bool mWon = false;

    private void Awake()
    {
        GameObject audioMgrObj = GameObject.FindGameObjectWithTag("AudioManager");
        if (audioMgrObj && audioMgrObj.TryGetComponent(out GameAudioManager audioMgr))
            mGameAudioManager = audioMgr;

        GameObject gameMgrObj = GameObject.FindGameObjectWithTag("GameManager");
        if (gameMgrObj && gameMgrObj.TryGetComponent(out GameManager gameMgr))
            mGameManager = gameMgr;

        GameObject eventSystemObj = GameObject.Find("EventSystem");
        if (eventSystemObj && eventSystemObj.TryGetComponent(out EventSystem eventSystem))
            mEventSystem = eventSystem;
    }

    private void Start()
    {
        if (mGameManager)
            mGameManager.RegisterIngameMenusManager(this);
    }

    // Update is called once per frame
    private void Update()
    {
        if (mWon)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
            if (mPause)
                Resume();

            else
                Pause();
    }

    public void Pause()
    {
        mPauseMenu.SetActive(true);

        if (mGameAudioManager)
            mGameAudioManager.PlayMenuSound(GameAudioManager.PauseMenuSounds.Intro);

        Time.timeScale = 0f;

        if (mGameManager)
        {
            mGameManager.PauseGame();
            mGameManager.gameObject.SetActive(false);
        }
        mPause = true;

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
        mPause = false;
    }

    // If true, switch from pause menu to options, else from options to pause menu
    public void SwitchMenu(bool pauseToOptions)
    {
        if (pauseToOptions)
        {
            mPauseMenuHeadButton.interactable = true;
            mPauseMenu.SetActive(false);
            mOptionsMenu.SetActive(true);
            if (mOptionsMenu.TryGetComponent(out OptionsMenuManager optionsMenuMgr))
                optionsMenuMgr.SetOptionsButtons();
            if (mEventSystem)
                mEventSystem.SetSelectedGameObject(mOptionsMenuHeadButton.gameObject);
            if (mGameAudioManager)
                mGameAudioManager.PlayMenuSound(GameAudioManager.OptionsMenuSounds.Intro);
        }
        else
        {
            mOptionsMenuHeadButton.interactable = true;
            mOptionsMenu.SetActive(false);
            mPauseMenu.SetActive(true);
            if (mEventSystem)
                mEventSystem.SetSelectedGameObject(mPauseMenuHeadButton.gameObject);
            if (mGameAudioManager)
                mGameAudioManager.PlayMenuSound(GameAudioManager.PauseMenuSounds.Intro);
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Win()
    {
        mWon = true;
        mGameAudioManager.PlayEventSound(GameAudioManager.EventSounds.LevelGeschafft);
        mWinMenu.SetActive(true);
        mEventSystem.SetSelectedGameObject(mWinMenuHeadButton.gameObject);
        Time.timeScale = 0f;
    }
}
