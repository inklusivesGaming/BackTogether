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

    public Button mPauseMenuHeadButton;
    public Button mOptionsMenuHeadButton;

    private EventSystem mEventSystem;

    private void Awake()
    {
        GameObject gameMgrObj = GameObject.FindGameObjectWithTag("GameManager");

        if (gameMgrObj && gameMgrObj.TryGetComponent(out GameManager gameMgr))
            mGameManager = gameMgr;

        GameObject eventSystemObj = GameObject.Find("EventSystem");
        if (eventSystemObj && eventSystemObj.TryGetComponent(out EventSystem eventSystem))
            mEventSystem = eventSystem;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (mPause)
                Resume();

            else
                Pause();
    }

    public void Pause()
    {
        mPauseMenu.SetActive(true);
        //TODO play  intro sound

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
        mPauseMenu.SetActive(false);
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
        if(pauseToOptions)
        {
            mPauseMenuHeadButton.interactable = true;
            mPauseMenu.SetActive(false);
            mOptionsMenu.SetActive(true);
            mEventSystem.SetSelectedGameObject(mOptionsMenuHeadButton.gameObject);
            //TODO play intro sound
        }
        else
        {
            mOptionsMenuHeadButton.interactable = true;
            mOptionsMenu.SetActive(false);
            mPauseMenu.SetActive(true);
            mEventSystem.SetSelectedGameObject(mPauseMenuHeadButton.gameObject);
            //TODO play  intro sound
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
