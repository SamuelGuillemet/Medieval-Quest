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
    public UITimer timer;

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
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
        timer.playing = true;
        Debug.Log("Play Game");
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
}
