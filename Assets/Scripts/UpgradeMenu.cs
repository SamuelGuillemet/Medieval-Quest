using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public CanvasGroup upgradeMenuCanvasGroup; //add this in the inspector
    private GameObject _specificUpgrades;
    private GameUI _gameUI;
    Slider[] upgradeSliders;

    void Awake()
    {
        StartCoroutine(DisableUpgradePopup(0f));

        _gameUI = FindObjectOfType<GameUI>();
        upgradeSliders = upgradeMenuCanvasGroup.GetComponentsInChildren<Slider>();
        for (int i = 0; i < upgradeSliders.Length; i++)
        {
            upgradeSliders[i].value = 0;
        }

        _specificUpgrades = GameObject.Find("SpecificUpgrades");
    }

    public void Upgrade(Slider slider)
    {
        slider.value += 1;
        if (slider.value == slider.maxValue)
        {
            slider.transform.gameObject.GetComponentInChildren<Button>().interactable = false;
        }
        _gameUI.upgradeCount--;
        Destroy(_gameUI.upgradeImages.transform.GetChild(0).gameObject);

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
