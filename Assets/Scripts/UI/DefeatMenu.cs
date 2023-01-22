using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    [SerializeField] private CrossFade _crossFade;
    [SerializeField] private CanvasGroup _pauseMenuCanvasGroup;
    [SerializeField] private CanvasGroup _upgradeMenuCanvasGroup;
    [SerializeField] private CanvasGroup _endGameBackgroundCanvasGroup;
    private CanvasGroup _defeatMenuCanvasGroup;
    [SerializeField] private CanvasGroup _victoryMenuCanvasGroup;
    [SerializeField] private TMPro.TMP_Text _finalTime;
    [SerializeField] private TMPro.TMP_Text _wavesText;
    [SerializeField] private UITimer _uitimer;

    void Awake()
    {

        // _crossFade = GameObject.Find("CrossFade").GetComponent<CrossFade>();
        // _pauseMenuCanvasGroup = GameObject.Find("PauseMenu").GetComponent<CanvasGroup>();
        // _upgradeMenuCanvasGroup = GameObject.Find("UpgradeMenu").GetComponent<CanvasGroup>();

        // _endGameBackgroundCanvasGroup = GameObject
        //     .Find("EndGameBackground")
        //     .GetComponent<CanvasGroup>();

        _defeatMenuCanvasGroup = GetComponent<CanvasGroup>();
        // _victoryMenuCanvasGroup = GameObject.Find("VictoryMenu").GetComponent<CanvasGroup>();

        // _uitimer = GameObject.Find("Timer").GetComponentInParent<UITimer>();

        // _finalTime = GameObject.Find("FinalTime").GetComponentInChildren<TMPro.TMP_Text>();
        // _wavesText = GameObject.Find("Waves").GetComponentInChildren<TMPro.TMP_Text>();

        HideDefeatMenu();
    }

    private void ShowDefeatMenu()
    {
        _pauseMenuCanvasGroup.interactable = false;
        _upgradeMenuCanvasGroup.interactable = false;
        _victoryMenuCanvasGroup.interactable = false;

        _endGameBackgroundCanvasGroup.interactable = true;
        _defeatMenuCanvasGroup.interactable = true;

        _pauseMenuCanvasGroup.blocksRaycasts = false;
        _upgradeMenuCanvasGroup.blocksRaycasts = false;
        _victoryMenuCanvasGroup.blocksRaycasts = false;

        _endGameBackgroundCanvasGroup.blocksRaycasts = true;
        _defeatMenuCanvasGroup.blocksRaycasts = true;

        _pauseMenuCanvasGroup.alpha *= 0.3f;
        _upgradeMenuCanvasGroup.alpha *= 0.3f;
        _victoryMenuCanvasGroup.alpha = 0;

        _endGameBackgroundCanvasGroup.alpha = 1;
        _defeatMenuCanvasGroup.alpha = 1;
    }

    private void HideDefeatMenu()
    {
        _endGameBackgroundCanvasGroup.interactable = false;
        _defeatMenuCanvasGroup.interactable = false;

        _endGameBackgroundCanvasGroup.blocksRaycasts = false;
        _defeatMenuCanvasGroup.blocksRaycasts = false;

        _endGameBackgroundCanvasGroup.alpha = 0;
        _defeatMenuCanvasGroup.alpha = 0;
    }

    public void DefeatMenuButton() // Defeat button
    {
        StartCoroutine(Defeat());
    }

    private IEnumerator Defeat()
    {
        GameManager _gameManager = GameManager.Instance;
        Time.timeScale = 0;

        ShowDefeatMenu();
        _finalTime.text = _uitimer.TimerText.text;
        Debug.Log(_gameManager);
        _wavesText.text = "Vagues vaincues : " + (_gameManager.WaveNumber - 1);

        PlayerPrefs.SetInt(
            _gameManager.SelectedPlayer.ToString() + "MaxWave",
            Mathf.Max(
                PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "MaxeWave"),
                _gameManager.WaveNumber - 1
            )
        );
        PlayerPrefs.SetInt(
            _gameManager.SelectedPlayer.ToString() + "GamesPlayed",
            PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "GamesPlayed") + 1
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
