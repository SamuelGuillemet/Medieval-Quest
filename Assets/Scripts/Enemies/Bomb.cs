using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _renderer;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private AudioSource _audioSource;

    private float _range = 2f;
    public float Range { set => _range = value; }

    private int _damage = 3;
    public int Damage { set => _damage = value; }

    private bool _exploded = false;

    public void SetTarget(Transform target)
    {
        Vector3 direction = target.position - Vector3.up - transform.position;
        direction = direction.normalized * 12f;
        _rigidbody.AddForce(direction, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        _particleSystem.Stop();
        _rigidbody.isKinematic = false;
        _renderer.SetActive(true);
        _exploded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            _particleSystem.Play();
            _rigidbody.isKinematic = true;
            _renderer.SetActive(false);
            _audioSource.Play();
            MakeDamage();
            Invoke(nameof(DisableObject), 0.5f);
        }
    }

    private void DisableObject()
    {
        PoolingManager.Instance.ReturnToPool(this.gameObject);
    }

    private void MakeDamage()
    {
        if (_exploded)
        {
            return;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range);
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.OnPlayerDamageTaken?.Invoke(_damage);
            }
            if (collider.gameObject.CompareTag("Mignon"))
            {
                collider.gameObject.GetComponent<Mignon>().OnDamageTaken(_damage);
            }
        }
        _exploded = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}

