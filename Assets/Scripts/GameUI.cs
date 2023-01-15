using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Slider _healthBar;
    private Slider _xpBar;

    // private Slider _ability1;
    // private Slider _ability2;
    // private Slider _ability3;

    private void Start()
    {
        _healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        _xpBar = GameObject.Find("XPBar").GetComponent<Slider>();

        _healthBar.maxValue = PlayerPrefs.GetFloat("Health");
        _healthBar.value = _healthBar.maxValue;

        _xpBar.value = _xpBar.minValue;

        // _ability1.value = _ability1.maxValue;
        // _ability2.value = _ability2.maxValue;
        // _ability3.value = _ability3.maxValue;
    }
}
