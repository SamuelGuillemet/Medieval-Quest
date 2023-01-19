using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public CanvasGroup upgradeMenuCanvasGroup; //add this in the inspector
    private GameObject _specificUpgrades;
    private GameManager _gameManager;
    Slider[] upgradeSliders;

    void Awake()
    {
        StartCoroutine(DisableUpgradePopup(0f));
        _gameManager = GameObject.Find("GameManager").GetComponentInParent<GameManager>();

        upgradeSliders = upgradeMenuCanvasGroup.GetComponentsInChildren<Slider>();
        for (int i = 0; i < upgradeSliders.Length; i++)
        {
            upgradeSliders[i].value = 0;
        }

        _specificUpgrades = GameObject.Find("SpecificUpgrades");
    }

    public void Upgrade(Slider slider)
    {
        // int upgradeIndex = System.Array.IndexOf(upgradeSliders, slider);
        //StartCoroutine(Upgrade(_gameManager.SelectedPlayer, upgradeIndex));
        slider.value += 1;
        if (slider.value == slider.maxValue)
        {
            slider.GetComponentInChildren<Button>().interactable = false;
        }
        StartCoroutine(DisableUpgradePopup(0.5f));
    }

    IEnumerator DisableUpgradePopup(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        //disable the upgrade menu
        upgradeMenuCanvasGroup.alpha = 0;
        upgradeMenuCanvasGroup.interactable = false;
        upgradeMenuCanvasGroup.blocksRaycasts = false;

        Time.timeScale = 1;
    }

    public void EnableUpgradePopup()
    {
        Time.timeScale = 0;

        //enable the upgrade menu
        upgradeMenuCanvasGroup.alpha = 1;
        upgradeMenuCanvasGroup.interactable = true;
        upgradeMenuCanvasGroup.blocksRaycasts = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // && _canUpgrade)
        {
            EnableUpgradePopup();
        }
    }
}
