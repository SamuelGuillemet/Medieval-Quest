using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CrossFade _crossFade;
    [SerializeField] private AudioMixerGroup _musicMixer;
    [SerializeField] private AudioMixerGroup _sfxMixer;

    private void Start()
    {
        AudioManager audioManager = AudioManager.Instance;
        if (!audioManager.IsPlaying()) audioManager.PlayMusic("MainMenu");
        SaveDataBetweenScenes.Instance.PreviousScene = SceneManager.GetActiveScene().name;

        LoadSettings();
    }

    public void PlayGame()
    {
        StartCoroutine(_crossFade.LoadSceneCoroutine("CharacterSelectionMenu"));
    }

    public void SettingsMenu()
    {
        SaveDataBetweenScenes.Instance.PreviousScene = SceneManager.GetActiveScene().name;
        StartCoroutine(_crossFade.LoadSceneCoroutine("SettingsMenu"));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("VolumePreference")) AudioListener.volume = PlayerPrefs.GetFloat("VolumePreference");
        if (PlayerPrefs.HasKey("MusicVolumePreference")) _musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolumePreference")) * 20);
        if (PlayerPrefs.HasKey("SFXVolumePreference")) _sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(PlayerPrefs.GetFloat("SFXVolumePreference")) * 20);
    }
}
