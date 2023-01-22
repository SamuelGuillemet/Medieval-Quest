using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private CrossFade _crossFade;
    [SerializeField] private CanvasGroup _pauseMenuCanvasGroup;
    [SerializeField] private CanvasGroup _upgradeMenuCanvasGroup;
    private bool _isPaused;

    void Awake()
    {
        _isPaused = true;
        ResumeGame();

        // if (PlayerPrefs.GetString("PreviousScene") == "GameScene")
        // {
        //     PauseGame();
        // }
        // else
        // {
        //     ResumeGame();
        // }
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
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;

        _upgradeMenuCanvasGroup.alpha *= 0.3f;
        _upgradeMenuCanvasGroup.interactable = false;
        _upgradeMenuCanvasGroup.blocksRaycasts = false;

        _pauseMenuCanvasGroup.alpha = 1;
        _pauseMenuCanvasGroup.interactable = true;
        _pauseMenuCanvasGroup.blocksRaycasts = true;
        _isPaused = !_isPaused;
    }

    public void ResumeGame()
    {
        _upgradeMenuCanvasGroup.alpha /= 0.3f;
        _upgradeMenuCanvasGroup.interactable = true;
        _upgradeMenuCanvasGroup.blocksRaycasts = true;

        _pauseMenuCanvasGroup.alpha = 0;
        _pauseMenuCanvasGroup.interactable = false;
        _pauseMenuCanvasGroup.blocksRaycasts = false;
        _isPaused = !_isPaused;

        Time.timeScale = 1;
    }

    public void Settings()
    {
        Debug.Log("SAVE SCENE TO CONTINUE GAME");
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(_crossFade.LoadSceneCoroutine("SettingsMenu", false));
    }

    public void ExitGame()
    {
        //Application.Quit();
        StartCoroutine(_crossFade.LoadSceneCoroutine("MainMenu"));
    }
}
