using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get { if (_instance == null) _instance = FindObjectOfType<AudioManager>(); return _instance; } }
    private static AudioManager _instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private List<Audio> _audioClips;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(string name)
    {
        AudioClip clip = _audioClips.Find(x => x.Name == name).Clip;
        _audioSource.PlayOneShot(clip);
    }

    public void StopSound()
    {
        _audioSource.Stop();
    }

}

[System.Serializable]
public struct Audio
{
    public string Name;
    public AudioClip Clip;
}
