using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public int upgradeCount { get; set; }
    public GameObject upgradeImages;
    private GameManager _gameManager;

    [SerializeField]
    private Image _upgradeImage;

    [SerializeField]
    private Image _upgradeAlert;

    [SerializeField]
    private Slider _healthBar;

    [SerializeField]
    private Slider _xpBar;

    [SerializeField]
    private Slider _enemiesBar;

    [SerializeField]
    private TMPro.TMP_Text _waveText;

    // private IPlayer _player;
    void Awake()
    {
        // _healthBar.maxValue = _player.MaxHealth;
        _healthBar.value = _healthBar.maxValue;
        _enemiesBar.value = _enemiesBar.minValue;

        _xpBar.value = _xpBar.minValue;

        upgradeCount = 0;
        _gameManager = GameManager.Instance;

    }

    public void UpdateEnnemiesBar(int enemies)
    {
        _enemiesBar.value = enemies;
    }

    public void UpdateHealthBar(float health) //or int ?
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
        StartCoroutine(UpgradeAlert());
    }

    public void UpdateWaveText(int wave, int enemies)
    {
        _waveText.text = "Wave: " + wave;
        _enemiesBar.maxValue = enemies;
    }

    IEnumerator UpgradeAlert()
    {
        Vector3 initpos = transform.position;
        Vector3 endpos = upgradeImages.transform.position + (5 * upgradeCount) * Vector3.right;
        Image img = Instantiate(_upgradeImage, initpos, Quaternion.identity, upgradeImages.transform);

        Vector3 popupinitpos = new Vector3(initpos.x, upgradeImages.transform.position.y - 50f, initpos.z);
        Vector3 popupendpos = new Vector3(initpos.x, upgradeImages.transform.position.y, initpos.z);
        Image popup = Instantiate(_upgradeAlert, initpos, Quaternion.identity, transform);

        float timer = 0f;
        while (timer < 1f)
        {
            float scale = Mathf.Lerp(20f, 1f, timer);
            img.transform.localScale = new Vector3(scale, scale, 1f);
            img.transform.position = Vector3.Lerp(initpos, endpos, timer);
            popup.transform.position = Vector3.Lerp(popupinitpos, popupendpos, timer);
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
        }
        yield return null;

        yield return new WaitForSeconds(1f);
        float alpha = 1f;
        while (alpha > 0f)
        {
            popup.color = new Color(1f, 1f, 1f, alpha);
            yield return new WaitForEndOfFrame();
            alpha -= 2f * Time.deltaTime;
        }
        Destroy(popup.gameObject);
    }
}
