using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance
    {
        get
        {
            _instance = FindObjectOfType<AudioManager>();
            if (_instance == null)
            {
                // _instance = FindObjectOfType<AudioManager>();
                new GameObject("AudioManager", typeof(AudioManager));
            }
            return _instance;
        }
    }
    private static AudioManager _instance;

    [SerializeField] private AudioSource _audioSourceSFX;
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private List<Audio> _audioClips;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string name)
    {
        AudioClip clip = _audioClips.Find(x => x.Name == name).Clip;
        _audioSourceSFX.PlayOneShot(clip);
    }

    public void Stop()
    {
        _audioSourceSFX.Stop();
    }

    public void PlayMusic(string name)
    {
        AudioClip clip = _audioClips.Find(x => x.Name == name).Clip;
        _audioSourceMusic.PlayOneShot(clip);
    }

}

[System.Serializable]
public struct Audio
{
    public string Name;
    public AudioClip Clip;
}
