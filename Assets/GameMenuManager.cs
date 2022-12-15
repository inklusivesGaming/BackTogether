using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MenuManager
{
    private GameManager mGameManager;
    private bool mPause = false;
    public GameObject pauseMenu;


    // Start is called before the first frame update
    void Start()
    {

    }

    protected override void Awake()
    {
        base.Awake();

        GameObject gameMgrObj = GameObject.FindGameObjectWithTag("GameManager");

        if (gameMgrObj && gameMgrObj.TryGetComponent(out GameManager gameMgr))
            mGameManager = gameMgr;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO maybe rather in game manager
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mPause)
            {
                Resume();
            }

            else
            {
                Pause();
            }
        }
    }

    protected override void PlayIntroSound()
    {
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        if (mGameManager)
        {
            mGameManager.PauseGame();
            mGameManager.gameObject.SetActive(false);
        }
        mPause = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        if (mGameManager)
        {
            mGameManager.gameObject.SetActive(true);
            mGameManager.ResumeGame();
        }
        mPause = false;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
