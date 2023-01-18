using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    public GameObject defeatMenu;
    public Button exitButton;
    public Button retryButton;
    public Animator crossfadeAnimator;

    private TMPro.TMP_Text _finalTime;
    private TMPro.TMP_Text _wavesText;
    private UITimer _uitimer;
    private GameManager _gameManager;

    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponentInParent<GameManager>();
        _uitimer = GameObject.Find("Timer").GetComponentInParent<UITimer>();

        _finalTime = GameObject.Find("FinalTime").GetComponentInChildren<TMPro.TMP_Text>();
        _wavesText = GameObject.Find("Waves").GetComponentInChildren<TMPro.TMP_Text>();

        defeatMenu.SetActive(false);
    }

    IEnumerator Defeat()
    {
        defeatMenu.SetActive(true);
        Time.timeScale = 0;
        _finalTime.text = _uitimer.TimerText.text;
        _wavesText.text = "Vagues vaincues : " + _gameManager.WaveNumber;
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
