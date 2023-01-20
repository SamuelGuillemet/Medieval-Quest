using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    public GameObject defeatMenu;
    public GameObject endGameImage;
    public CrossFade crossFade;

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

        endGameImage.SetActive(false);
        defeatMenu.SetActive(false);
    }

    IEnumerator Defeat()
    {
        endGameImage.SetActive(true);
        defeatMenu.SetActive(true);
        Time.timeScale = 0;
        _finalTime.text = _uitimer.TimerText.text;
        _wavesText.text = "Vagues vaincues : " + _gameManager.WaveNumber;
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
