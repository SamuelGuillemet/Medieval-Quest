using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    private CanvasGroup _upgradeMenuCanvasGroup;
    private GameUI _gameUI;
    private GameManager _gameManager;

    Slider[] sliders;
    Button[] buttons;
    bool[] isMaxed;


    void Awake()
    {
        StartCoroutine(DisableUpgradePopup(0f));

        _gameManager = GameManager.Instance;
        _gameUI = FindObjectOfType<GameUI>();
        _upgradeMenuCanvasGroup = GetComponent<CanvasGroup>();

        sliders = GetComponentsInChildren<Slider>();
        buttons = new Button[sliders.Length];
        isMaxed = new bool[sliders.Length];
        for (int i = 0; i < sliders.Length; i++)
        {
            Debug.Log(sliders[i].GetComponentInChildren<Button>().name);
            buttons[i] = sliders[i].GetComponentInChildren<Button>();
            isMaxed[i] = false;
            Debug.Log(isMaxed[i].ToString());
        }
    }

    public void Upgrade(Slider slider) // Called by button
    {
        // int key;
        slider.value += 1;

        //get index of slider in sliders array
        int index = 0;
        for (int i = 0; i < sliders.Length; i++)
        {
            if (sliders[i] == slider)
            {
                index = i;
                break;
            }
        }
        //depending on the slider, do something
        //_gameManager.upgradeValues[key] += 1; // TODO

        if (slider.value == slider.maxValue)
        {
            slider.transform.gameObject.GetComponentInChildren<Button>().interactable = false;
            isMaxed[index] = true;
        }
        _gameUI.upgradeCount--;
        Destroy(
            _gameUI.upgradeImages.transform
                .GetChild(_gameUI.upgradeImages.transform.childCount - 1)
                .gameObject
        );

        if (_gameUI.upgradeCount <= 0)
        {
            StartCoroutine(DisableUpgradePopup(0.25f));
        }
    }

    IEnumerator DisableUpgradePopup(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        //hide the upgrade menu
        _upgradeMenuCanvasGroup.alpha = 0;
        _upgradeMenuCanvasGroup.interactable = false;
        _upgradeMenuCanvasGroup.blocksRaycasts = false;

        //disable all buttons
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }

        Time.timeScale = 1;
    }

    public void EnableUpgradePopup()
    {
        Time.timeScale = 0;

        //enable the upgrade menu
        _upgradeMenuCanvasGroup.alpha = 1;
        _upgradeMenuCanvasGroup.interactable = true;
        _upgradeMenuCanvasGroup.blocksRaycasts = true;

        GetRandomUpgrades();
    }

    private void GetRandomUpgrades()
    {
        int[] randomNumbers = new int[3] { -1, -1, -1 }; //initialize to impossible values
        int random;
        for (int i = 0; i < randomNumbers.Length; i++)
        {
            //NOT WORKING BECAUSE OF INFINITE LOOP
            random = Random.Range(0, sliders.Length);
            //if the random number is already in the array, try again
            if (isMaxed[random] || randomNumbers[0] == random || randomNumbers[1] == random || randomNumbers[2] == random)
            {
                i--;
                //find how many upgrades are left
                int upgradesLeft = 0;
                foreach (bool maxed in isMaxed)
                {
                    if (!maxed)
                    {
                        upgradesLeft++;
                    }
                }
                //if there is three or less upgrades left, use these last upgrades
                if (upgradesLeft < 3)
                {
                    randomNumbers = new int[upgradesLeft];

                    for (int j = 0; j < randomNumbers.Length; j++)
                    {
                        random = Random.Range(0, sliders.Length);
                        if (isMaxed[random])
                        {
                            j--;
                            continue;
                        }
                        randomNumbers[j] = random;
                    }
                    break;
                }
                if (upgradesLeft == 0)
                {
                    Debug.Log("No upgrades left");
                    return;
                }
                continue;
            }
            randomNumbers[i] = random;
        }
        foreach (int buttonIndex in randomNumbers)
        {
            buttons[buttonIndex].interactable = true;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _upgradeMenuCanvasGroup.alpha == 1)
        {
            StartCoroutine(DisableUpgradePopup(0f));
        }

        if (Input.GetKeyDown(KeyCode.E) && _gameUI.upgradeCount > 0)
        {
            EnableUpgradePopup();
        }

    }
}
