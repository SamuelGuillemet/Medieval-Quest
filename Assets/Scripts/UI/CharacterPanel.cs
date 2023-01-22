using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _characterModel;

    [SerializeField]
    private Image _medalImage;

    [SerializeField]
    private TMPro.TextMeshProUGUI _wavesText;

    [SerializeField]
    private TMPro.TextMeshProUGUI _gamesText;

    [SerializeField]
    private TMPro.TextMeshProUGUI _winsText;
    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _medalImage.enabled = false;

        if (!PlayerPrefs.HasKey(_gameManager.SelectedPlayer.ToString() + "GamesPlayed"))
        {
            PlayerPrefs.SetInt(_gameManager.SelectedPlayer.ToString() + "GamesPlayed", 0);
        }

        if (!PlayerPrefs.HasKey(_gameManager.SelectedPlayer.ToString() + "MaxWave"))
        {
            PlayerPrefs.SetInt(_gameManager.SelectedPlayer.ToString() + "MaxWave", 0);
        }

        if (!PlayerPrefs.HasKey(_gameManager.SelectedPlayer.ToString() + "GamesWon"))
        {
            PlayerPrefs.SetInt(_gameManager.SelectedPlayer.ToString() + "GamesWon", 0);
        }

        PlayerPrefs.Save();

        SetGamesText(PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "GamesPlayed"));
        SetWavesText(PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "MaxWave"));
        SetWinsText(PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "GamesWon"));

        if (PlayerPrefs.GetInt(_gameManager.SelectedPlayer.ToString() + "GamesWon") > 0)
        {
            _medalImage.enabled = true;
        }
    }

    private void SetWavesText(int wave)
    {
        _wavesText.text = "Nombre de vagues maximum vaincues : " + wave;
    }

    private void SetGamesText(int games)
    {
        _gamesText.text = "Nombre de parties jouées : " + games;
    }

    private void SetWinsText(int win)
    {
        _winsText.text = "Nombre de parties gagnées : " + win;
    }

    public void SetSelectedPlayer() // Called by the Sélectionner button
    {
        Debug.Log(_characterModel.name);
        switch (_characterModel.name)
        {
            case "Warrior":
                _gameManager.SelectedPlayer = PlayerType.Guerrier;
                break;
            case "Archer":
                _gameManager.SelectedPlayer = PlayerType.Archer;
                break;
            case "Wizard":
                _gameManager.SelectedPlayer = PlayerType.Mage;
                break;
            default:
                break;
        }
    }
}
