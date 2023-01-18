using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private Slider _healthBar;
    private Slider _xpBar;
    private Slider _enemiesBar;
    private TMPro.TMP_Text _waveText;

    void Awake()
    {
        _healthBar = GameObject.Find("HealthBar").GetComponentInChildren<Slider>();
        _xpBar = GameObject.Find("XPBar").GetComponentInChildren<Slider>();
        _enemiesBar = GameObject.Find("EnemiesBar").GetComponentInChildren<Slider>();
        _waveText = GameObject.Find("Waves").GetComponentInChildren<TMPro.TMP_Text>();

        _healthBar.maxValue = PlayerPrefs.GetFloat("Health");
        _healthBar.value = _healthBar.maxValue;
        _enemiesBar.value = _enemiesBar.minValue;

        _xpBar.value = _xpBar.minValue;
    }

    public void UpdateEnnemiesBar(int enemies)
    {
        _enemiesBar.value = enemies;
    }

    public void UpdateHealthBar(float health)
    {
        _healthBar.value = health;
    }

    public void UpdateXPBar(float xp)
    {
        _xpBar.value = xp;
    }

    public void UpdateWaveText(int wave, int enemies)
    {
        _waveText.text = "Wave: " + wave;
        _enemiesBar.maxValue = enemies;
    }
}
