using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : IPlayer
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private float attackRadius = 2f;

    [SerializeField] public float magnitude = 0.001f;
    [SerializeField] public float chocWaveRadius = 3f;
    [SerializeField] public float chocWaveDamage = 3f;
    [SerializeField] public Camera cam;




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
        _audioManager.PlaySound("AttackDemon");
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
        _animator.SetTrigger("ChocWave");
        _audioManager.PlaySound("DemonChocWave");
        yield return new WaitForSeconds(_cooldown2);
        _canAction2 = true;
    }

    public void ChocWave()
    {
        StartCoroutine(Shake());
        Collider[] cols = Physics.OverlapSphere(transform.position, chocWaveRadius);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Enemy")
            {
                //Make damage to enemy with WallDamage
            }
        }
    }


    IEnumerator Shake()
    {
        Vector3 originalPos = cam.transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < 1)
        {
            float x = Random.Range(-0.5f, 0.5f) * magnitude;
            float y = Random.Range(-0.5f, 0.5f) * magnitude;
            cam.transform.localPosition = new Vector3(originalPos.x + x,originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime * 5;
            yield return null;
        }
        cam.transform.localPosition = originalPos;
    }



    public override IEnumerator Attack3()
    {
        _canAction3 = false;
        _animator.SetTrigger("Summon");
        _audioManager.PlaySound("DemonSummon");
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

    public override void DamageSound()
    {
        _audioManager.PlaySound("DamageDemon");
    }
}

