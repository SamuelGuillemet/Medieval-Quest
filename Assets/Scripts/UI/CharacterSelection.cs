using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] _charactersPanels;
    [SerializeField] private CrossFade _crossFade;
    private int _currentCharacterIndex;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;

        if (PlayerPrefs.HasKey("CharacterIndex"))
        {
            _currentCharacterIndex = PlayerPrefs.GetInt("CharacterIndex");
        }
        else
        {
            _currentCharacterIndex = 0;
        }

        _charactersPanels = GameObject.FindGameObjectsWithTag("CharacterPanel");

        for (int i = 0; i < _charactersPanels.Length; i++)
        {
            _charactersPanels[i].SetActive(false);
        }
        Debug.Log("Current Character Index: " + _currentCharacterIndex);
        _charactersPanels[_currentCharacterIndex].SetActive(true);
    }

    public void ExitMenu()
    {
        _charactersPanels[_currentCharacterIndex].SetActive(false);
        StartCoroutine(_crossFade.LoadSceneCoroutine("MainMenu"));
    }

    public void PlayGame()
    {
        SaveCharacterandAbilities();
        _charactersPanels[_currentCharacterIndex].SetActive(false);
        StartCoroutine(_crossFade.LoadSceneCoroutine("GameScene"));
        Debug.Log("Play Game");
    }

    private void SaveCharacterandAbilities()
    {
        // _gameManager.SelectedPlayer = (SelectedPlayer) currentCharacterIndex;

    }

    public void NextCharacter()
    {
        for (int i = 0; i < _charactersPanels.Length; i++)
        {
            if (_charactersPanels[i].activeSelf)
            {
                _charactersPanels[i].SetActive(false);
                if (i == _charactersPanels.Length - 1)
                {
                    _charactersPanels[0].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", 0);
                    _currentCharacterIndex = 0;
                }
                else
                {
                    _charactersPanels[i + 1].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", i + 1);
                    _currentCharacterIndex = i + 1;
                }
                break;
            }
        }
    }

    public void PreviousCharacter()
    {
        for (int i = 0; i < _charactersPanels.Length; i++)
        {
            if (_charactersPanels[i].activeSelf)
            {
                _charactersPanels[i].SetActive(false);
                if (i == 0)
                {
                    _charactersPanels[_charactersPanels.Length - 1].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", _charactersPanels.Length - 1);
                    _currentCharacterIndex = _charactersPanels.Length - 1;
                }
                else
                {
                    _charactersPanels[i - 1].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", i - 1);
                    _currentCharacterIndex = i - 1;
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
