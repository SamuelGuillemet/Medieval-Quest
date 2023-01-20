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
    public CrossFade crossFade;
    private int currentCharacterIndex;
    private GameManager _gameManager;

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
        StartCoroutine(crossFade.LoadSceneCoroutine("MainMenu"));
    }

    public void PlayGame()
    {
        SaveCharacterandAbilities();
        StartCoroutine(crossFade.LoadSceneCoroutine("GameScene"));
        Debug.Log("Play Game");
    }

    private void SaveCharacterandAbilities()
    {
        PlayerPrefs.SetString("Character", charactersPanels[currentCharacterIndex].name);
        // _gameManager.SelectedPlayer = (SelectedPlayer) currentCharacterIndex;
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
