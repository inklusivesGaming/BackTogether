using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

    public GameObject mGameManager;

    private bool mPause = false;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        mGameManager.SetActive(false);

        mPause = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        mGameManager.SetActive(true);

        mPause = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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

}
