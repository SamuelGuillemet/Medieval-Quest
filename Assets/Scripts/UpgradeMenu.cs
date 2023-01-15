using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public GameObject upgradeMenu;
    Button[] upgradeButtons;
    Slider[] upgradeSliders;

    private void Start()
    {
        upgradeButtons = upgradeMenu.GetComponentsInChildren<Button>();
        upgradeMenu.SetActive(false);
        upgradeSliders = upgradeMenu.GetComponentsInChildren<Slider>();
        foreach (Slider slider in upgradeSliders)
        {
            slider.value = 0;
        }
    }

    public void Upgrade(Slider slider)
    {
        slider.value += 1;
        StartCoroutine(ExitMenu());
    }

    IEnumerator ExitMenu()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        upgradeMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
