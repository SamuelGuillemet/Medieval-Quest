using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private CrossFade _crossFade;

    private void Awake()
    {
        _crossFade = GameObject.Find("CrossFade").GetComponent<CrossFade>();
    }

    public void PlayGame()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(_crossFade.LoadSceneCoroutine("CharacterSelectionMenu"));
    }

    public void SettingsMenu()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(_crossFade.LoadSceneCoroutine("SettingsMenu"));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
