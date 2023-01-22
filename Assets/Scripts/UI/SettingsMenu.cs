using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    private CanvasGroup _settingsCanvasGroup;
    private CanvasGroup _popupCanvasGroup;
    private TMPro.TMP_Dropdown _resolutionDropdown;
    private TMPro.TMP_Dropdown _qualityDropdown;
    private Toggle _fullscreenToggle;
    private Slider _volumeSlider;
    private Button _saveButton;
    private float _currentVolume;
    private bool _isSaved;
    private CrossFade _crossFade;
    Resolution[] resolutions;

    void Awake()
    {
        _settingsCanvasGroup = GameObject.Find("Buttons").GetComponent<CanvasGroup>();
        _popupCanvasGroup = GameObject.Find("PopupCanvas").GetComponent<CanvasGroup>();
        _resolutionDropdown = GameObject
            .Find("ResolutionDropdown")
            .GetComponent<TMPro.TMP_Dropdown>();
        _qualityDropdown = GameObject.Find("QualityDropdown").GetComponent<TMPro.TMP_Dropdown>();
        _fullscreenToggle = GameObject.Find("FullscreenToggle").GetComponent<Toggle>();
        _volumeSlider = GameObject.Find("VolumeSlider").GetComponent<Slider>();
        _saveButton = GameObject.Find("SaveButton").GetComponent<Button>();

        _crossFade = GameObject.Find("CrossFade").GetComponent<CrossFade>();
        DisablePopup();

        _resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (
                resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height
            )
                currentResolutionIndex = i;
        }

        _resolutionDropdown.AddOptions(options);
        _resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);

        _isSaved = true;
        _saveButton.interactable = false;
    }

    public void EnableSaveButton()
    {
        _saveButton.interactable = true;
        _isSaved = false;
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        _currentVolume = volume;
    }

    private void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        _qualityDropdown.RefreshShownValue();
    }

    public void SaveSettings()
    {
        //Apply settings to the game
        SetVolume(_volumeSlider.value);
        SetFullscreen(_fullscreenToggle.isOn);
        SetResolution(_resolutionDropdown.value);
        SetQuality(_qualityDropdown.value);

        //Save settings to PlayerPrefs
        PlayerPrefs.SetInt("QualitySettingPreference", _qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", _fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("VolumePreference", _currentVolume);
        PlayerPrefs.Save();
        _saveButton.interactable = false;
        _isSaved = true;
    }

    private void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            _qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            _qualityDropdown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            _resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            _resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            _fullscreenToggle.isOn = PlayerPrefs.GetInt("FullscreenPreference") == 1 ? true : false;
        else
            Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("VolumePreference"))
            _volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        else
            _volumeSlider.value = _currentVolume;

        if (!PlayerPrefs.HasKey("PreviousScene"))
            PlayerPrefs.SetString("PreviousScene", "MainMenu");
    }

    public void ExitMenu()
    {
        if (!_isSaved)
        {
            EnablePopup();
        }
        else
        {
            StartCoroutine(_crossFade.LoadSceneCoroutine(PlayerPrefs.GetString("PreviousScene")));
        }
    }

    private void DisablePopup()
    {
        //enable the normal ui
        _settingsCanvasGroup.alpha = 1;
        _settingsCanvasGroup.interactable = true;
        _settingsCanvasGroup.blocksRaycasts = true;

        //disable the confirmation quit ui
        _popupCanvasGroup.alpha = 0;
        _popupCanvasGroup.interactable = false;
        _popupCanvasGroup.blocksRaycasts = false;
    }

    private void EnablePopup()
    {
        //disable the normal ui
        _settingsCanvasGroup.alpha = 0.3f;
        _settingsCanvasGroup.interactable = false;
        _settingsCanvasGroup.blocksRaycasts = false;

        //enable the confirmation quit ui
        _popupCanvasGroup.alpha = 1;
        _popupCanvasGroup.interactable = true;
        _popupCanvasGroup.blocksRaycasts = true;
    }

    public void ConfirmExit()
    {
        StartCoroutine(_crossFade.LoadSceneCoroutine(PlayerPrefs.GetString("PreviousScene")));
    }

    public void SaveAndExit()
    {
        SaveSettings();
        StartCoroutine(_crossFade.LoadSceneCoroutine(PlayerPrefs.GetString("PreviousScene")));
    }
}
