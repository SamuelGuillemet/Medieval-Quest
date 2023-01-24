using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _settingsCanvasGroup;
    [SerializeField] private CanvasGroup _popupCanvasGroup;
    [SerializeField] private TMPro.TMP_Dropdown _resolutionDropdown;
    [SerializeField] private TMPro.TMP_Dropdown _qualityDropdown;
    [SerializeField] private Toggle _fullscreenToggle;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private AudioMixer _musicMixer;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private AudioMixer _sfxMixer;
    [SerializeField] private Button _saveButton;
    [SerializeField] private CrossFade _crossFade;
    private float _currentVolume;
    private float _currentMusicVolume;
    private float _currentSFXVolume;
    private bool _isSaved;
    Resolution[] resolutions;

    void Awake()
    {
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

    private void SetMusicVolume(float volume)
    {
        _musicMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        _currentMusicVolume = volume;
    }

    private void SetSFXVolume(float volume)
    {
        _sfxMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        _currentSFXVolume = volume;
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
        SetMusicVolume(_musicSlider.value);
        SetSFXVolume(_sfxSlider.value);
        SetFullscreen(_fullscreenToggle.isOn);
        SetResolution(_resolutionDropdown.value);
        SetQuality(_qualityDropdown.value);

        //Save settings to PlayerPrefs
        PlayerPrefs.SetInt("QualitySettingPreference", _qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", _resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", _fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("VolumePreference", _currentVolume);
        PlayerPrefs.SetFloat("MusicVolumePreference", _currentMusicVolume);
        PlayerPrefs.SetFloat("SFXVolumePreference", _currentSFXVolume);
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

        if (PlayerPrefs.HasKey("MusicVolumePreference"))
            _musicSlider.value = PlayerPrefs.GetFloat("MusicVolumePreference");
        else
            _musicSlider.value = _currentMusicVolume;

        if (PlayerPrefs.HasKey("SFXVolumePreference"))
            _sfxSlider.value = PlayerPrefs.GetFloat("SFXVolumePreference");
        else
            _sfxSlider.value = _currentSFXVolume;

        // if (!PlayerPrefs.HasKey("PreviousScene"))
        //     PlayerPrefs.SetString("PreviousScene", "MainMenu");
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
