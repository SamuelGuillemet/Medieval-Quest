using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificUpgrades : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text _upgrade1Text;
    [SerializeField] private TMPro.TMP_Text _upgrade2Text;
    [SerializeField] private TMPro.TMP_Text _upgrade3Text;
    [SerializeField] private TMPro.TMP_Text _upgrade4Text;
    [SerializeField] private TMPro.TMP_Text _upgrade5Text;

    private GameManager _gameManager;

    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        switch (_gameManager.SelectedPlayer)
        {
            case PlayerType.None:
                break;

            case PlayerType.Mage:
                _upgrade1Text.text = "Dégâts de l'attaque 1";
                _upgrade2Text.text = "Répulsion de l'orbe";
                _upgrade3Text.text = "Ennemis touchés par l'orbe";
                _upgrade4Text.text = "Augmentation taille du mur";
                _upgrade5Text.text = "Gain dégat à l'apparition du mur";
                break;

            case PlayerType.Archer:
                _upgrade1Text.text = "Dégâts de l'attaque 1";
                _upgrade2Text.text = "Dégâts du piège";
                _upgrade3Text.text = "Vitesse du sprint";
                _upgrade4Text.text = "Perçage de l'attaque 1";
                _upgrade5Text.text = "Capacité du piège";
                break;
            case PlayerType.Deamon:
                _upgrade1Text.text = "Dégâts de l'attaque 1";
                _upgrade2Text.text = "Dégâts de l’attaque 2";
                _upgrade3Text.text = "Repoussement de l'attaque 2";
                _upgrade4Text.text = "Delai soin mignon";
                _upgrade5Text.text = "Points de vie mignon";
                break;
        }
    }

}
