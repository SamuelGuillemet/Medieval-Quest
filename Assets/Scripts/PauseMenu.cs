using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public Animator crossfadeAnimator;
    public CanvasGroup pauseMenuCanvasGroup;
    public CanvasGroup upgradeMenuCanvasGroup;
    private bool _isPaused;

    void Awake()
    {
        _isPaused = false;
        if (PlayerPrefs.GetString("PreviousScene") == "GameScene")
        {
            Time.timeScale = 0;
            ResumeGame();
        }
        else
        {
            Time.timeScale = 1;
            PauseGame();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
            _isPaused = !_isPaused;
        }
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        upgradeMenuCanvasGroup.alpha *= 0.3f;
        upgradeMenuCanvasGroup.interactable = false;
        upgradeMenuCanvasGroup.blocksRaycasts = false;

        pauseMenuCanvasGroup.alpha = 1;
        pauseMenuCanvasGroup.interactable = true;
        pauseMenuCanvasGroup.blocksRaycasts = true;
    }

    public void ResumeGame()
    {
        upgradeMenuCanvasGroup.alpha /= 0.3f;
        upgradeMenuCanvasGroup.interactable = true;
        upgradeMenuCanvasGroup.blocksRaycasts = true;

        pauseMenuCanvasGroup.alpha = 0;
        pauseMenuCanvasGroup.interactable = false;
        pauseMenuCanvasGroup.blocksRaycasts = false;

        Time.timeScale = 1;
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
