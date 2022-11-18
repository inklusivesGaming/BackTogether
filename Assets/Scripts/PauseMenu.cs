using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

    public GameObject gameManager;

    public bool mPause = false;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        gameManager.SetActive(false);

        mPause = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        gameManager.SetActive(true);

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
