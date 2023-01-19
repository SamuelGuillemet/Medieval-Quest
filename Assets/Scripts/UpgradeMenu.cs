using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public CanvasGroup upgradeMenuCanvasGroup; //add this in the inspector
    private GameObject _specificUpgrades;
    private GameUI _gameUI;

    void Awake()
    {
        StartCoroutine(DisableUpgradePopup(0f));

        _gameUI = FindObjectOfType<GameUI>();
    }

    public void Upgrade(Slider slider)
    {
        slider.value += 1;
        //depending on the slider, do something
        switch (slider.name)
        {
            case "MovementSpeed":
                //do something
                break;
            case "Cooldown1":
                //do something
                break;
            case "Cooldown2":
                //do something
                break;
            case "Cooldown3":
                //do something
                break;
            case "MaxHP":
                //do something
                break;
            case "Upgrade1":
                //do something
                break;
            case "Upgrade2":
                //do something
                break;
            case "Upgrade3":
                //do something
                break;
            case "Upgrade4":
                //do something
                break;
            case "Upgrade5":
                //do something
                break;
        }

        if (slider.value == slider.maxValue)
        {
            slider.transform.gameObject.GetComponentInChildren<Button>().interactable = false;
        }
        _gameUI.upgradeCount--;
        Destroy(
            _gameUI.upgradeImages.transform
                .GetChild(_gameUI.upgradeImages.transform.childCount - 1)
                .gameObject
        );

        if (_gameUI.upgradeCount == 0)
        {
            StartCoroutine(DisableUpgradePopup(0.5f));
        }
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
        if (Input.GetKeyDown(KeyCode.E) && _gameUI.upgradeCount > 0)
        {
            EnableUpgradePopup();
        }
    }
}
