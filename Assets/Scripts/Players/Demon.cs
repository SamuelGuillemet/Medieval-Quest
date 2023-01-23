using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : IPlayer
{
    [SerializeField] float damage = 2f;
    [SerializeField] float attackRadius = 2f;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        MaxHealth = 60;

        _cooldown1 = 2f;
        _cooldown2 = 10f;
        _cooldown3 = 10f;
    }

    public override IEnumerator Attack1()
    {
        _canAction1 = false;
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_cooldown1);
        _canAction1 = true;
    }

    public void Attack()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward, attackRadius);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Enemy")
            {
                //Make damage to enemy
            }
        }
    }


    public override IEnumerator Attack2()
    {
        _canAction2 = false;
        _animator.SetTrigger("Invicible");
        yield return new WaitForSeconds(_cooldown2);
        _canAction2 = true;
    }

    public void SwitchInvincible()
    {
        //Make the hero invicible 
    }




    public override IEnumerator Attack3()
    {
        _canAction3 = false;
        _animator.SetTrigger("Summon");
        yield return new WaitForSeconds(_cooldown3);
        _canAction3 = true;
    }

    public void Summon()
    {
        //Liche liche = Instantiate(lichePrefab, transform, false);
        //Destroy(liche, 10);
        Debug.Log("Liche");
    }

    public void SwitchMovement()
    {
        _playerAgent.Agent.isStopped = !_playerAgent.Agent.isStopped;
    }
}

