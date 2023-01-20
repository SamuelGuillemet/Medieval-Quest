using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public CanvasGroup settingsCanvasGroup;
    public CanvasGroup popupCanvasGroup;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;
    public Toggle fullscreenToggle;
    public Slider volumeSlider;
    public Button saveButton;
    private float currentVolume;
    private bool _isSaved;
    public CrossFade crossFade;
    Resolution[] resolutions;

    void Awake()
    {
        DisablePopup();

        resolutionDropdown.ClearOptions();
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

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);

        _isSaved = true;
        saveButton.interactable = false;
    }

    public void EnableSaveButton()
    {
        saveButton.interactable = true;
        _isSaved = false;
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        currentVolume = volume;
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
        qualityDropdown.RefreshShownValue();
    }

    public void SaveSettings()
    {
        //Apply settings to the game
        SetVolume(volumeSlider.value);
        SetFullscreen(fullscreenToggle.isOn);
        SetResolution(resolutionDropdown.value);
        SetQuality(qualityDropdown.value);

        //Save settings to PlayerPrefs
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
        PlayerPrefs.Save();
        saveButton.interactable = false;
        _isSaved = true;
    }

    private void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3;

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("FullscreenPreference"))
            fullscreenToggle.isOn = PlayerPrefs.GetInt("FullscreenPreference") == 1 ? true : false;
        else
            Screen.fullScreen = true;

        if (PlayerPrefs.HasKey("VolumePreference"))
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        else
            volumeSlider.value = currentVolume;

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
            StartCoroutine(crossFade.LoadSceneCoroutine(PlayerPrefs.GetString("PreviousScene")));
        }
    }

    private void DisablePopup()
    {
        //enable the normal ui
        settingsCanvasGroup.alpha = 1;
        settingsCanvasGroup.interactable = true;
        settingsCanvasGroup.blocksRaycasts = true;

        //disable the confirmation quit ui
        popupCanvasGroup.alpha = 0;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;
    }

    private void EnablePopup()
    {
        //disable the normal ui
        settingsCanvasGroup.alpha = 0.3f;
        settingsCanvasGroup.interactable = false;
        settingsCanvasGroup.blocksRaycasts = false;

        //enable the confirmation quit ui
        popupCanvasGroup.alpha = 1;
        popupCanvasGroup.interactable = true;
        popupCanvasGroup.blocksRaycasts = true;
    }

    public void ConfirmExit()
    {
        StartCoroutine(crossFade.LoadSceneCoroutine(PlayerPrefs.GetString("PreviousScene")));
    }

    public void SaveAndExit()
    {
        SaveSettings();
        StartCoroutine(crossFade.LoadSceneCoroutine(PlayerPrefs.GetString("PreviousScene")));
    }
}
