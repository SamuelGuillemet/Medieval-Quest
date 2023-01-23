using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPlayer : MonoBehaviour
{
    protected Animator _animator;
    protected PlayerAgent _playerAgent;

    protected int _health;
    protected int _maxHealth;
    public int MaxHealth { set => _health = _health = value; }

    // Attack 1
    protected float _cooldown1;
    protected bool _canAction1 = true;
    protected Coroutine _attack1Coroutine;

    // Attack 2
    protected float _cooldown2;
    protected bool _canAction2 = true;
    protected Coroutine _attack2Coroutine;

    // Attack 3
    protected float _cooldown3;
    protected bool _canAction3 = true;
    protected Coroutine _attack3Coroutine;



    public AudioManager _audioManager;


    // Start is called before the first frame update
    public virtual void Start()
    {
        _animator = GetComponent<Animator>();
        _playerAgent = GetComponentInParent<PlayerAgent>();

        _audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetMouseButton(0) && _canAction1)
        {
            _attack1Coroutine = StartCoroutine(Attack1());
        }
        if (Input.GetMouseButton(1) && _canAction2)
        {
            _attack2Coroutine =  StartCoroutine(Attack2());
        }
        if (Input.GetKey(KeyCode.Space) && _canAction3)
        {
            _attack3Coroutine = StartCoroutine(Attack3());
        }
    }

    public virtual IEnumerator Attack1()
    {
        yield return null;
    }

    public virtual IEnumerator Attack2()
    {
        yield return null;
    }

    public virtual IEnumerator Attack3()
    {
        yield return null;
    }


    
    public void TakeDamage(int amount)
    {
        _health -= amount;
        DamageSound();
        // TODO : Add event when  health = 0
    }

    public virtual void DamageSound()
    {
        
    }


}
