using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public int upgradeCount = 0;
    public GameObject upgradeImages;
    private GameManager _gameManager;

    [SerializeField] private VictoryMenu _victoryMenu;
    [SerializeField] private DefeatMenu _defeatMenu;

    [SerializeField] private Image _upgradeImage;
    [SerializeField] private Image _upgradeAlert;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _xpBar;
    [SerializeField] private Slider _enemiesBar;
    [SerializeField] private TMPro.TMP_Text _waveText;

    private GameObject _archerAbilities;
    private GameObject _mageAbilities;
    private GameObject _demonAbilities;

    Slider[] abilities;

    void Start()
    {
        _gameManager = GameManager.Instance;

        _archerAbilities = transform.Find("HUD/ArcherAbilities").gameObject;
        _mageAbilities = transform.Find("HUD/MageAbilities").gameObject;
        _demonAbilities = transform.Find("HUD/DemonAbilities").gameObject;

        _archerAbilities.SetActive(false);
        _mageAbilities.SetActive(false);
        _demonAbilities.SetActive(false);

        GameObject active = null;
        switch (GameManager.Instance.SelectedPlayer)
        {
            case PlayerType.Archer:
                _archerAbilities.SetActive(true);
                active = _archerAbilities;
                break;
            case PlayerType.Mage:
                _mageAbilities.SetActive(true);
                active = _mageAbilities;
                break;
            case PlayerType.Demon:
                _demonAbilities.SetActive(true);
                active = _demonAbilities;
                break;
        }

        abilities = active.GetComponentsInChildren<Slider>();
        foreach (Slider slider in abilities)
        {
            slider.maxValue = 1;
            slider.value = 0;
        }

        _healthBar.maxValue = GameManager.Instance.Player.MaxHealth;
        _healthBar.value = _healthBar.maxValue;
        _enemiesBar.value = _enemiesBar.minValue;

        _xpBar.value = _xpBar.minValue;

        upgradeCount = 0;
    }

    void FixedUpdate()
    {
        if (abilities == null)
            return;
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i].value = 1 - _gameManager.Player.GetCoolDowns(i + 1);
        }
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
        popup.gameObject.name = "UpgradeAlert: " + upgradeCount.ToString();

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

    public void Victory()
    {
        _victoryMenu.VictoryMenuButton();
    }

    public void Defeat()
    {
        _defeatMenu.DefeatMenuButton();
    }
}
