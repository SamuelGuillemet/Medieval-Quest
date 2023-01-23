using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : IPlayer
{


    [SerializeField] private FireBall fireBallPrefab;
    [SerializeField] private float attack1Damage = 5f;
    [SerializeField] public GameObject hand;

    [SerializeField] private Orb orbPrefab;
    [SerializeField] private float attack2Duration = 5f;
    [SerializeField] public int maxEnemyTouched;
    [SerializeField] public float speedOrb = 300f;
    [SerializeField] public bool orbRepulsion = false;


    


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        MaxHealth = 40;

        _cooldown1 = 2f;
        _cooldown2 = 10f;
        _cooldown3 = 10f;

    }

    public override IEnumerator Attack1()
    {
        _canAction1 = false;
        _animator.SetTrigger("ThrowFireBall");
        yield return new WaitForSeconds(_cooldown1);
        _canAction1 = true;
    }

    public void ThrowFireBall()
    {
        FireBall fireBall = Instantiate(fireBallPrefab, hand.transform, false);
    }


    public override IEnumerator Attack2()
    {
        _canAction2 = false;
        _animator.SetTrigger("ThrowOrb");
        _audioManager.PlaySound("Orb");
        yield return new WaitForSeconds(_cooldown2);
        _canAction2 = true;
    }

    public void ThrowOrb()
    {
        Orb orb = Instantiate(orbPrefab, transform.position + new Vector3(0,2,5), transform.rotation, transform);
        Destroy(orb.gameObject, attack2Duration);
    }


    public void SwitchMovement()
    {
        _playerAgent.Agent.isStopped = !_playerAgent.Agent.isStopped;
    }

    public override IEnumerator Attack3()
    {
        _canAction3 = false;
        yield return new WaitForSeconds(_cooldown3);
        _canAction3 = true;
    }

    

    public override void DamageSound()
    {
        _audioManager.PlaySound("DamageMage");
    }
}
