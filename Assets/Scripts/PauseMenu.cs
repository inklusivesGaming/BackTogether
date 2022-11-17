using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] GameObject pauseMenu;

    public GameObject gameManager;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;

        gameManager.SetActive(false);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        gameManager.SetActive(true);
    }

}
