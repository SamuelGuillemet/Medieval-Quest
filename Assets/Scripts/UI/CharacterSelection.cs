using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    private GameObject[] _charactersPanelGameObjects;
    private CharacterPanel[] _characterPanels;
    [SerializeField] private CrossFade _crossFade;
    private int _currentCharacterIndex;
    private SaveDataBetweenScenes _saveDataBetweenScenes;

    private void Awake()
    {
        _saveDataBetweenScenes = SaveDataBetweenScenes.Instance;

        GetComponent<Canvas>().worldCamera = Camera.main;

        if (PlayerPrefs.HasKey("CharacterIndex"))
        {
            _currentCharacterIndex = PlayerPrefs.GetInt("CharacterIndex");
        }
        else
        {
            _currentCharacterIndex = 0;
        }

        _charactersPanelGameObjects = GameObject.FindGameObjectsWithTag("CharacterPanel");

        for (int i = 0; i < _charactersPanelGameObjects.Length; i++)
        {
            _charactersPanelGameObjects[i].SetActive(false);
        }
        _charactersPanelGameObjects[_currentCharacterIndex].SetActive(true);

        _characterPanels = new CharacterPanel[_charactersPanelGameObjects.Length];

        int index = 0;
        foreach (GameObject characterPanelGameObject in _charactersPanelGameObjects)
        {
            _characterPanels[index] = characterPanelGameObject.GetComponent<CharacterPanel>();
            index++;
        }
    }

    public void ExitMenu()
    {
        _charactersPanelGameObjects[_currentCharacterIndex].SetActive(false);
        StartCoroutine(_crossFade.LoadSceneCoroutine("MainMenu"));
    }

    public void PlayGame()
    {
        _saveDataBetweenScenes.SelectedPlayer = _characterPanels[_currentCharacterIndex].Player;
        _charactersPanelGameObjects[_currentCharacterIndex].SetActive(false);
        StartCoroutine(_crossFade.LoadSceneCoroutine("GameScene"));
    }

    public void NextCharacter()
    {
        for (int i = 0; i < _charactersPanelGameObjects.Length; i++)
        {
            if (_charactersPanelGameObjects[i].activeSelf)
            {
                _charactersPanelGameObjects[i].SetActive(false);
                if (i == _charactersPanelGameObjects.Length - 1)
                {
                    _charactersPanelGameObjects[0].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", 0);
                    _currentCharacterIndex = 0;
                }
                else
                {
                    _charactersPanelGameObjects[i + 1].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", i + 1);
                    _currentCharacterIndex = i + 1;
                }
                break;
            }
        }
    }

    public void PreviousCharacter()
    {
        for (int i = 0; i < _charactersPanelGameObjects.Length; i++)
        {
            if (_charactersPanelGameObjects[i].activeSelf)
            {
                _charactersPanelGameObjects[i].SetActive(false);
                if (i == 0)
                {
                    _charactersPanelGameObjects[_charactersPanelGameObjects.Length - 1].SetActive(true);
                    PlayerPrefs.SetInt("CharacterIndex", _charactersPanelGameObjects.Length - 1);
                    _currentCharacterIndex = _charactersPanelGameObjects.Length - 1;
                }
                else
                {
                    _charactersPanelGameObjects[i - 1].SetActive(true);
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

        _saveDataBetweenScenes.SelectedPlayer = _characterPanels[_currentCharacterIndex].Player;
    }
}
