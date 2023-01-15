using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;
    public Button exitButton;
    public Button settingsButton;
    public Animator crossfadeAnimator;

    private void Start()
    {
        if (PlayerPrefs.GetString("PreviousScene") == "GameScene")
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Settings()
    {
        Debug.Log("SAVE SCENE TO CONTINUE GAME");
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(LoadSceneCoroutine("SettingsMenu"));
    }

    public void ExitGame()
    {
        //Application.Quit();
        StartCoroutine(LoadSceneCoroutine("MainMenu"));
    }
}
