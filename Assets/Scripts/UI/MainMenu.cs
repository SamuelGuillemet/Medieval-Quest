using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public CrossFade crossFade;

    public void PlayGame()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(crossFade.LoadSceneCoroutine("CharacterSelectionMenu"));
    }

    public void SettingsMenu()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(crossFade.LoadSceneCoroutine("SettingsMenu"));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
