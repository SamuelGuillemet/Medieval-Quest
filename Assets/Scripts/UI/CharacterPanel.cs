using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject _characterModel;
    private bool _isInspected;

    [SerializeField]
    private Image _medalImage;

    [SerializeField]
    private TMPro.TextMeshProUGUI _waveText;

    [SerializeField]
    private TMPro.TextMeshProUGUI _gamesText;

    [SerializeField]
    private TMPro.TextMeshProUGUI _winText;
    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _medalImage.enabled = false;

        SetGamesText(PlayerPrefs.GetInt("GamesPlayed"));
        SetWaveText(PlayerPrefs.GetInt("MaxWave"));
        SetWinText(PlayerPrefs.GetInt("GamesWon"));

        if (PlayerPrefs.GetInt("GamesWon") > 0)
        {
            _medalImage.enabled = true;
        }
    }

    private void OnEnable()
    {
        _isInspected = false;
        _characterModel.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnMouseDrag()
    {
        _isInspected = true;
        _characterModel.transform.Rotate(0, Input.GetAxis("Mouse X") * -10f, 0);
    }

    private void OnMouseUp()
    {
        _isInspected = false;
    }

    private void Update()
    {
        if (!_isInspected)
        {
            _characterModel.transform.Rotate(0, -0.2f, 0);
        }
    }

    private void SetWaveText(int wave)
    {
        _waveText.text = "Nombre de vagues maximum vaincues : " + wave;
    }

    private void SetGamesText(int games)
    {
        _gamesText.text = "Nombre de parties jouées : " + games;
    }

    private void SetWinText(int win)
    {
        _winText.text = "Nombre de parties gagnées : " + win;
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
