using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Button exitButton;
    public Button playButton;
    public Button nextCharacterButton;
    public Button previousCharacterButton;
    private GameObject[] charactersPanels;
    public Animator crossfadeAnimator;
    private int currentCharacterIndex;

    private void Start()
    {
        if (PlayerPrefs.HasKey("CharacterIndex"))
        {
            currentCharacterIndex = PlayerPrefs.GetInt("CharacterIndex");
        }
        else
        {
            currentCharacterIndex = 0;
        }

        charactersPanels = GameObject.FindGameObjectsWithTag("CharacterPanel");

        foreach (GameObject panel in charactersPanels)
        {
            panel.SetActive(false);
        }
        charactersPanels[currentCharacterIndex].SetActive(true);
    }

    public void ExitMenu()
    {
        StartCoroutine(LoadSceneCoroutine("MainMenu"));
    }

    public void PlayGame()
    {
        SaveCharacterandAbilities();
        StartCoroutine(LoadSceneCoroutine("GameScene"));
        Debug.Log("Play Game");
    }

    private void SaveCharacterandAbilities()
    {
        PlayerPrefs.SetString("Character", charactersPanels[currentCharacterIndex].name);
        PlayerPrefs.SetFloat(
            "Health",
            charactersPanels[currentCharacterIndex].GetComponentInChildren<Slider>().value
        );
    }

    public void NextCharacter()
    {
        for (int i = 0; i < charactersPanels.Length; i++)
        {
            if (charactersPanels[i].activeSelf)
            {
                charactersPanels[i].SetActive(false);
                if (i == charactersPanels.Length - 1)
                {
                    charactersPanels[0].SetActive(true);
                }
                else
                {
                    charactersPanels[i + 1].SetActive(true);
                }
                PlayerPrefs.SetInt("CharacterIndex", i);
                break;
            }
        }
    }

    public void PreviousCharacter()
    {
        for (int i = 0; i < charactersPanels.Length; i++)
        {
            if (charactersPanels[i].activeSelf)
            {
                charactersPanels[i].SetActive(false);
                if (i == 0)
                {
                    charactersPanels[charactersPanels.Length - 1].SetActive(true);
                }
                else
                {
                    charactersPanels[i - 1].SetActive(true);
                }
                PlayerPrefs.SetInt("CharacterIndex", i);
                break;
            }
        }
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        crossfadeAnimator.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(.5f);
        SceneManager.LoadScene(sceneName);
    }
}
