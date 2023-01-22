﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    [SerializeField] private CrossFade _crossFade;
    [SerializeField] private CanvasGroup _pauseMenuCanvasGroup;
    [SerializeField] private CanvasGroup _upgradeMenuCanvasGroup;
    [SerializeField] private CanvasGroup _endGameBackgroundCanvasGroup;
    [SerializeField] private CanvasGroup _defeatMenuCanvasGroup;
    private CanvasGroup _victoryMenuCanvasGroup;
    [SerializeField] private TMPro.TMP_Text _finalTime;
    [SerializeField] private TMPro.TMP_Text _bestTime;
    [SerializeField] private TMPro.TMP_Text _newRecord;
    [SerializeField] private UITimer _uitimer;

    void Awake()
    {
        // _crossFade = GameObject.Find("CrossFade").GetComponent<CrossFade>();
        // _pauseMenuCanvasGroup = GameObject.Find("PauseMenu").GetComponent<CanvasGroup>();
        // _upgradeMenuCanvasGroup = GameObject.Find("UpgradeMenu").GetComponent<CanvasGroup>();

        // _endGameBackgroundCanvasGroup = GameObject
        //     .Find("EndGameBackground")
        //     .GetComponent<CanvasGroup>();

        // _defeatMenuCanvasGroup = GameObject.Find("DefeatMenu").GetComponent<CanvasGroup>();
        _victoryMenuCanvasGroup = GetComponent<CanvasGroup>();

        // _uitimer = GameObject.Find("Timer").GetComponentInParent<UITimer>();

        // _finalTime = GameObject.Find("FinalTime").GetComponentInChildren<TMPro.TMP_Text>();
        // _bestTime = GameObject.Find("BestTime").GetComponentInChildren<TMPro.TMP_Text>();
        // _newRecord = GameObject.Find("NewRecord").GetComponentInChildren<TMPro.TMP_Text>();
        _newRecord.text = "";

        HideVictoryMenu();
    }

    private void ShowVictoryMenu()
    {
        _pauseMenuCanvasGroup.alpha *= 0.3f;
        _pauseMenuCanvasGroup.interactable = false;
        _pauseMenuCanvasGroup.blocksRaycasts = false;

        _upgradeMenuCanvasGroup.alpha *= 0.3f;
        _upgradeMenuCanvasGroup.interactable = false;
        _upgradeMenuCanvasGroup.blocksRaycasts = false;

        _endGameBackgroundCanvasGroup.alpha = 1;
        _endGameBackgroundCanvasGroup.interactable = true;
        _endGameBackgroundCanvasGroup.blocksRaycasts = true;

        _victoryMenuCanvasGroup.alpha = 1;
        _victoryMenuCanvasGroup.interactable = true;
        _victoryMenuCanvasGroup.blocksRaycasts = true;
    }

    private void HideVictoryMenu()
    {
        _endGameBackgroundCanvasGroup.alpha = 0;
        _endGameBackgroundCanvasGroup.interactable = false;
        _endGameBackgroundCanvasGroup.blocksRaycasts = false;

        _victoryMenuCanvasGroup.alpha = 0;
        _victoryMenuCanvasGroup.interactable = false;
        _victoryMenuCanvasGroup.blocksRaycasts = false;
    }

    public void VictoryMenuButton() // Victory button
    {
        StartCoroutine(Victory());
    }

    IEnumerator Victory()
    {
        GameManager _gameManager = GameManager.Instance;

        ShowVictoryMenu();
        Time.timeScale = 0;
        _finalTime.text = _uitimer.TimerText.text;

        if (PlayerPrefs.HasKey("BestTime"))
        {
            if (PlayerPrefs.GetFloat("BestTime") > _uitimer.timer)
            {
                PlayerPrefs.SetFloat("BestTime", _uitimer.timer);
                _bestTime.text = _uitimer.TimerText.text;
                _newRecord.text = "Nouveau record !";
            }
            else
            {
                float besttime = PlayerPrefs.GetFloat("BestTime");
                int minutes = Mathf.FloorToInt(besttime / 60F);
                int seconds = Mathf.FloorToInt(besttime % 60F);
                int milliseconds = Mathf.FloorToInt((besttime * 100F) % 100F);
                _bestTime.text =
                    minutes.ToString("00")
                    + ":"
                    + seconds.ToString("00")
                    + ":"
                    + milliseconds.ToString("00");
            }
        }
        else
        {
            PlayerPrefs.SetFloat("BestTime", _uitimer.timer);
            _bestTime.text = _uitimer.TimerText.text;
            _newRecord.text = "Nouveau record !";
        }

        PlayerPrefs.SetInt(
            _gameManager.SelectedPlayer.ToString() + "MaxWave",
            PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "MaxWave")
                + _gameManager.WaveNumber
        );
        PlayerPrefs.SetInt(
            _gameManager.SelectedPlayer.ToString() + "GamesPlayed",
            PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "GamesPlayed") + 1
        );

        PlayerPrefs.SetInt(
            _gameManager.SelectedPlayer.ToString() + "GamesWon",
            PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "GamesWon") + 1
        );

        PlayerPrefs.Save();
        yield return null;
    }

    public void Retry() // Retry button
    {
        StartCoroutine(_crossFade.LoadSceneCoroutine("CharacterSelectionMenu"));
    }

    public void Exit() // Exit button
    {
        StartCoroutine(_crossFade.LoadSceneCoroutine("MainMenu"));
    }
}
