using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator crossfadeAnimator;

    public void PlayGame()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(LoadSceneCoroutine("CharacterSelect"));
    }

    public void SettingsMenu()
    {
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        StartCoroutine(LoadSceneCoroutine("SettingsMenu"));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadScene(sceneName);
    }
}
