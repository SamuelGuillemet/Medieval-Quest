using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryMenu : MonoBehaviour
{
    public GameObject victoryMenu;
    public Button exitButton;
    public Button retryButton;
    public Animator crossfadeAnimator;

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

        victoryMenu.SetActive(false);
    }

    IEnumerator Victory()
    {
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
        yield return null;
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void Retry()
    {
        StartCoroutine(LoadSceneCoroutine("GameScene"));
    }

    public void Exit()
    {
        StartCoroutine(LoadSceneCoroutine("MainMenu"));
    }
}
