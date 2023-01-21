using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] charactersPanels;
    public CrossFade crossFade;
    private int currentCharacterIndex;
    private GameManager _gameManager;

    private void Awake()
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
        charactersPanels[currentCharacterIndex].SetActive(false);
        StartCoroutine(crossFade.LoadSceneCoroutine("MainMenu"));
    }

    public void PlayGame()
    {
        SaveCharacterandAbilities();
        charactersPanels[currentCharacterIndex].SetActive(false);
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
                    PlayerPrefs.SetInt("CharacterIndex", 0);
                    currentCharacterIndex = 0;
                }
                else
                {
                    charactersPanels[i + 1].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", i + 1);
                    currentCharacterIndex = i + 1;
                }
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
                    PlayerPrefs.SetInt("CharacterIndex", charactersPanels.Length - 1);
                    currentCharacterIndex = charactersPanels.Length - 1;
                }
                else
                {
                    charactersPanels[i - 1].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", i - 1);
                    currentCharacterIndex = i - 1;
                }
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextCharacter();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PreviousCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitMenu();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            PlayGame();
        }
    }
}
