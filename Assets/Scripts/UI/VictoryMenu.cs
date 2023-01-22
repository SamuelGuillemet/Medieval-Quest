using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public GameObject victoryMenu;
    public CrossFade crossFade;
    public GameObject endGameImage;
    private TMPro.TMP_Text _finalTime;
    private TMPro.TMP_Text _bestTime;
    private TMPro.TMP_Text _newRecord;
    private UITimer _uitimer;
    private GameManager _gameManager;

    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponentInParent<GameManager>();
        _uitimer = GameObject.Find("Timer").GetComponentInParent<UITimer>();

        _finalTime = GameObject.Find("FinalTime").GetComponentInChildren<TMPro.TMP_Text>();
        _bestTime = GameObject.Find("BestTime").GetComponentInChildren<TMPro.TMP_Text>();
        _newRecord = GameObject.Find("NewRecord").GetComponentInChildren<TMPro.TMP_Text>();
        _newRecord.text = "";

        endGameImage.SetActive(false);
        victoryMenu.SetActive(false);
    }

    IEnumerator Victory()
    {
        endGameImage.SetActive(true);
        victoryMenu.SetActive(true);
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

    public void Retry()
    {
        StartCoroutine(crossFade.LoadSceneCoroutine("GameScene"));
    }

    public void Exit()
    {
        StartCoroutine(crossFade.LoadSceneCoroutine("MainMenu"));
    }
}
