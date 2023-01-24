using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
    [SerializeField] private GameObject _characterModel;

    [SerializeField] private Image _medalImage;

    [SerializeField] private TMPro.TextMeshProUGUI _wavesText;

    [SerializeField] private TMPro.TextMeshProUGUI _gamesText;

    [SerializeField] private TMPro.TextMeshProUGUI _winsText;
    private SaveBetwenScene _saveBetwenScene;

    private void Start()
    {
        _saveBetwenScene = SaveBetwenScene.Instance;
        _medalImage.enabled = false;

        SetSelectedPlayer();

        if (!PlayerPrefs.HasKey(_saveBetwenScene.SelectedPlayer.ToString() + "GamesPlayed"))
        {
            PlayerPrefs.SetInt(_saveBetwenScene.SelectedPlayer.ToString() + "GamesPlayed", 0);
        }

        if (!PlayerPrefs.HasKey(_saveBetwenScene.SelectedPlayer.ToString() + "MaxWave"))
        {
            PlayerPrefs.SetInt(_saveBetwenScene.SelectedPlayer.ToString() + "MaxWave", 0);
        }

        if (!PlayerPrefs.HasKey(_saveBetwenScene.SelectedPlayer.ToString() + "GamesWon"))
        {
            PlayerPrefs.SetInt(_saveBetwenScene.SelectedPlayer.ToString() + "GamesWon", 0);
        }

        PlayerPrefs.Save();

        SetGamesText(PlayerPrefs.GetInt(_saveBetwenScene.SelectedPlayer.ToString() + "GamesPlayed"));
        SetWavesText(PlayerPrefs.GetInt(_saveBetwenScene.SelectedPlayer.ToString() + "MaxWave"));
        SetWinsText(PlayerPrefs.GetInt(_saveBetwenScene.SelectedPlayer.ToString() + "GamesWon"));

        if (PlayerPrefs.GetInt(_saveBetwenScene.SelectedPlayer.ToString() + "GamesWon") > 0)
        {
            _medalImage.enabled = true;
        }
    }

    private void SetWavesText(int wave)
    {
        _wavesText.text = "Meilleure vague vaincue : " + wave;
    }

    private void SetGamesText(int games)
    {
        _gamesText.text = "Parties jouées : " + games;
    }

    private void SetWinsText(int wins)
    {
        _winsText.text = "Parties gagnées : " + wins;
    }

    public void SetSelectedPlayer()
    {
        Debug.Log(_characterModel.name);
        switch (_characterModel.name)
        {
            case "Demon T Wiezzorek":
                _saveBetwenScene.SelectedPlayer = PlayerType.Demon;
                break;
            case "Erika Archer With Bow Arrow":
                _saveBetwenScene.SelectedPlayer = PlayerType.Archer;
                break;
            case "Ganfaul M Aure":
                _saveBetwenScene.SelectedPlayer = PlayerType.Mage;
                break;
            default:
                break;
        }
    }
}
