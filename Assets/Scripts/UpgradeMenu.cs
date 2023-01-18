using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public GameObject upgradeMenu;
    private GameObject specificUpgrades;
    private GameManager _gameManager;
    Button[] upgradeButtons;
    Slider[] upgradeSliders;

    void Awake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponentInParent<GameManager>();

        upgradeMenu.SetActive(false);
        upgradeSliders = upgradeMenu.GetComponentsInChildren<Slider>();
        for (int i = 0; i < upgradeSliders.Length; i++)
        {
            upgradeSliders[i].value = 0;
            upgradeButtons[i] = upgradeSliders[i].GetComponentInChildren<Button>();
        }

        specificUpgrades = GameObject.Find("SpecificUpgrades");
    }

    public void Upgrade(Slider slider)
    {
        int upgradeIndex = System.Array.IndexOf(upgradeSliders, slider);
        //StartCoroutine(Upgrade(_gameManager.SelectedPlayer, upgradeIndex));
        slider.value += 1;
        if (slider.value == slider.maxValue)
        {
            slider.GetComponentInChildren<Button>().interactable = false;
        }

        StartCoroutine(ExitMenu());
    }

    IEnumerator ExitMenu()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
