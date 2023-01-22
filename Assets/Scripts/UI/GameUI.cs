using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public int upgradeCount { get; set; }
    public GameObject upgradeImages;

    [SerializeField]
    private Image _upgradeImage;
    private Slider _healthBar;
    private Slider _xpBar;
    private Slider _enemiesBar;
    private TMPro.TMP_Text _waveText;

    // private IPlayer _player;
    void Awake()
    {
        _healthBar = GameObject.Find("HealthBar").GetComponentInChildren<Slider>();
        _xpBar = GameObject.Find("XPBar").GetComponentInChildren<Slider>();
        _enemiesBar = GameObject.Find("EnemiesBar").GetComponentInChildren<Slider>();
        _waveText = GameObject.Find("Waves").GetComponentInChildren<TMPro.TMP_Text>();

        // _healthBar.maxValue = _player.MaxHealth;
        _healthBar.value = _healthBar.maxValue;
        _enemiesBar.value = _enemiesBar.minValue;

        _xpBar.value = _xpBar.minValue;

        upgradeCount = 0;
    }

    public void UpdateEnnemiesBar(int enemies)
    {
        _enemiesBar.value = enemies;
    }

    public void UpdateHealthBar(float health) //or int ?
    {
        _healthBar.value = health;
    }

    public void UpdateXPBar(float xp) //or int ?
    {
        _xpBar.value = xp;
        if (_xpBar.value >= _xpBar.maxValue)
        {
            upgradeCount++;
            Instantiate(
                _upgradeImage,
                upgradeImages.transform.position + (5 * upgradeCount) * Vector3.right,
                Quaternion.identity,
                upgradeImages.transform
            );

            _xpBar.value = _xpBar.minValue;
            _xpBar.maxValue *= 2; //find balance
        }
    }

    public void UpdateWaveText(int wave, int enemies)
    {
        _waveText.text = "Wave: " + wave;
        _enemiesBar.maxValue = enemies;
    }
}
