using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button exitButton;
    public Button settingsButton;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(true);
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Settings()
    {
        //save current scene
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("SettingsMenu");
    }
}
