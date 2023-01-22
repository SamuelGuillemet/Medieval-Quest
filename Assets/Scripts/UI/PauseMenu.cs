using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private CrossFade _crossFade;
    private CanvasGroup _pauseMenuCanvasGroup;
    private CanvasGroup _upgradeMenuCanvasGroup;
    private bool _isPaused;

    void Awake()
    {
        _crossFade = GameObject.Find("CrossFade").GetComponent<CrossFade>();
        _pauseMenuCanvasGroup = GameObject.Find("PauseMenu").GetComponent<CanvasGroup>();
        _upgradeMenuCanvasGroup = GameObject.Find("UpgradeMenu").GetComponent<CanvasGroup>();

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
        StartCoroutine(_crossFade.LoadSceneCoroutine("SettingsMenu"));
    }

    public void ExitGame()
    {
        //Application.Quit();
        StartCoroutine(_crossFade.LoadSceneCoroutine("MainMenu"));
    }
}
