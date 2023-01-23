using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public int upgradeCount { get; set; }
    public GameObject upgradeImages;

    [SerializeField] private Image _upgradeImage;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _xpBar;
    [SerializeField] private Slider _enemiesBar;
    [SerializeField] private TMPro.TMP_Text _waveText;

    void Start()
    {
        _healthBar.maxValue = GameManager.Instance.Player.MaxHealth;
        _healthBar.value = _healthBar.maxValue;
        _enemiesBar.value = _enemiesBar.minValue;

        _xpBar.value = _xpBar.minValue;

        upgradeCount = 0;
    }

    public void UpdateEnnemiesBar(int enemies)
    {
        _enemiesBar.value = enemies;
    }

    public void UpdateHealthBar(int health)
    {
        _healthBar.value = health;
    }

    public void UpdateExperienceBar(int xp, int maxXP)
    {
        _xpBar.value = xp;
        _xpBar.maxValue = maxXP;
    }

    public void CreateUpgradeImage()
    {
        upgradeCount++;
        Instantiate(_upgradeImage, upgradeImages.transform.position + (5 * upgradeCount) * Vector3.right, Quaternion.identity, upgradeImages.transform);
    }

    public void UpdateWaveText(int wave, int enemies)
    {
        _waveText.text = "Wave: " + wave;
        _enemiesBar.maxValue = enemies;
    }
}
