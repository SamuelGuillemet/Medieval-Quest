using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPlayer : MonoBehaviour
{
    protected Animator _animator;
    protected PlayerAgent _playerAgent;
    protected GameManager _gameManager;

    protected int _health;
    protected int _maxHealth;
    public int MaxHealth { set => _maxHealth = _health = value; get => _maxHealth; }
    public int Health { get => _health; }

    // Attack 1
    protected float _cooldown1;
    private float _currentCooldown1 = 0;
    protected bool _canAction1 = true;
    protected Coroutine _attack1Coroutine;

    // Attack 2
    protected float _cooldown2;
    private float _currentCooldown2 = 0;
    protected bool _canAction2 = true;
    protected Coroutine _attack2Coroutine;

    // Attack 3
    protected float _cooldown3;
    private float _currentCooldown3 = 0;
    protected bool _canAction3 = true;
    protected Coroutine _attack3Coroutine;


    protected AudioManager _audioManager;


    // Start is called before the first frame update
    public virtual void Start()
    {
        _gameManager = GameManager.Instance;
        _animator = GetComponent<Animator>();
        _playerAgent = GetComponentInParent<PlayerAgent>();

        _audioManager = AudioManager.Instance;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (Input.GetMouseButton(0) && _canAction1)
        {
            _attack1Coroutine = StartCoroutine(Attack1());
        }
        if (Input.GetMouseButton(1) && _canAction2)
        {
            _attack2Coroutine = StartCoroutine(Attack2());
        }
        if (Input.GetKey(KeyCode.Space) && _canAction3)
        {
            _attack3Coroutine = StartCoroutine(Attack3());
        }
    }

    private void FixedUpdate()
    {
        if (_currentCooldown1 > 0 && !_canAction1)
        {
            _currentCooldown1 -= Time.deltaTime;
        }
        if (_currentCooldown2 > 0 && !_canAction2)
        {
            _currentCooldown2 -= Time.deltaTime;
        }
        if (_currentCooldown3 > 0 && !_canAction3)
        {
            _currentCooldown3 -= Time.deltaTime;
        }

        if (_currentCooldown1 <= 0 && !_canAction1)
        {
            _currentCooldown1 = _cooldown1;
        }
        if (_currentCooldown2 <= 0 && !_canAction2)
        {
            _currentCooldown2 = _cooldown2;
        }
        if (_currentCooldown3 <= 0 && !_canAction3)
        {
            _currentCooldown3 = _cooldown3;
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
        if (_health <= 0)
        {
            _gameManager.OnPlayerDeath?.Invoke();
        }
    }

    public void Heal(int amount)
    {
        _health += amount;
        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public void Upgrade(int key)
    {
        Debug.Log("Upgrade " + key);
        switch (key)
        {
            case 1:
                GainSpeed();
                break;
            case 2:
                ReduceCooldownAttack1();
                break;
            case 3:
                ReduceCooldownAttack2();
                break;
            case 4:
                ReduceCooldownAttack3();
                break;
            case 5:
                GainHealth();
                break;
            case 6:
                SpecificUpgrade1();
                break;
            case 7:
                SpecificUpgrade2();
                break;
            case 8:
                SpecificUpgrade3();
                break;
            case 9:
                SpecificUpgrade4();
                break;
            case 10:
                SpecificUpgrade5();
                break;
        }
    }

    private void ReduceCooldownAttack1()
    {
        _cooldown1 -= 0.1f;
    }

    private void ReduceCooldownAttack2()
    {
        _cooldown2 -= 0.2f;
    }

    private void ReduceCooldownAttack3()
    {
        _cooldown3 -= 0.5f;
    }

    private void GainSpeed()
    {
        _playerAgent.Agent.speed += 0.25f;
    }

    private void GainHealth()
    {
        _gameManager.OnPlayerHealed?.Invoke(_maxHealth - _health);
    }

    public virtual void SpecificUpgrade1() { }

    public virtual void SpecificUpgrade2() { }

    public virtual void SpecificUpgrade3() { }

    public virtual void SpecificUpgrade4() { }

    public virtual void SpecificUpgrade5() { }

    public virtual void DamageSound() { }

    public float GetCoolDowns(int key)
    {
        switch (key)
        {
            case 1:
                return _currentCooldown1 / _cooldown1;
            case 2:
                return _currentCooldown2 / _cooldown2;
            case 3:
                return _currentCooldown3 / _cooldown3;
        }
        return 0;
    }
}
