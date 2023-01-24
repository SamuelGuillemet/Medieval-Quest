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


    private AudioManager _audioManager;


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
        // TODO : End of game - Defeat
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


}
