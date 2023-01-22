using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    [SerializeField] private int _experienceValue = 1;
    [SerializeField] private AudioSource _audioSource;
    public int ExperienceValue { set => _experienceValue = value; get => _experienceValue; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _audioSource.Play();
            StartCoroutine(DestroyOrb());
        }
    }

    IEnumerator DestroyOrb()
    {
        yield return new WaitForSeconds(0.25f);
        GameManager.Instance.OnOrbCollected?.Invoke(this);
    }

    private void Update()
    {
        if (transform.position.y < -10)
        {
            PoolingManager.Instance.ReturnToPool(this.gameObject);
        }
    }
}
