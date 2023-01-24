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
                _upgrade2Text.text = "Increase damage by 1";
                _upgrade3Text.text = "Increase damage by 1";
                _upgrade4Text.text = "Increase damage by 1";
                _upgrade5Text.text = "Increase damage by 1";
                break;

            case PlayerType.Archer:
                _upgrade1Text.text = "Dégâts de l'attaque 1";
                _upgrade2Text.text = "Dégâts de l'attaque 2";
                _upgrade3Text.text = "Vitesse du sprint";
                _upgrade4Text.text = "Perçage de l'attaque 1";
                _upgrade5Text.text = "Capacité du piège";
                break;
            case PlayerType.Guerrier:
                _upgrade1Text.text = "Dégâts de l'attaque 1";
                _upgrade2Text.text = "Répulsion de l'orbe";
                _upgrade3Text.text = "Ennemis touchables par l'orbe";
                _upgrade4Text.text = "Taille du mur";
                _upgrade5Text.text = "Dégâts à l'apparition du mur";
                break;
        }
    }

}
