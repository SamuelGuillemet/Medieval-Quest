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
    public PlayerType Player;

    private void Start()
    {
        _medalImage.enabled = false;

        SetSelectedPlayer();

        if (!PlayerPrefs.HasKey(Player.ToString() + "GamesPlayed"))
        {
            PlayerPrefs.SetInt(Player.ToString() + "GamesPlayed", 0);
        }

        if (!PlayerPrefs.HasKey(Player.ToString() + "MaxWave"))
        {
            PlayerPrefs.SetInt(Player.ToString() + "MaxWave", 0);
        }

        if (!PlayerPrefs.HasKey(Player.ToString() + "GamesWon"))
        {
            PlayerPrefs.SetInt(Player.ToString() + "GamesWon", 0);
        }

        PlayerPrefs.Save();

        SetGamesText(PlayerPrefs.GetInt(Player.ToString() + "GamesPlayed"));
        SetWavesText(PlayerPrefs.GetInt(Player.ToString() + "MaxWave"));
        SetWinsText(PlayerPrefs.GetInt(Player.ToString() + "GamesWon"));

        if (PlayerPrefs.GetInt(Player.ToString() + "GamesWon") > 0)
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
        switch (_characterModel.name)
        {
            case "Demon T Wiezzorek":
                Player = PlayerType.Demon;
                break;
            case "Erika Archer With Bow Arrow":
                Player = PlayerType.Archer;
                break;
            case "Ganfaul M Aure":
                Player = PlayerType.Mage;
                break;
            default:
                break;
        }
    }
}
