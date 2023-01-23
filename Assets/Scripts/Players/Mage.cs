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

    [SerializeField] public float lengthWall = 1f;
    [SerializeField] private Material IceMaterial;
    [SerializeField] private bool WallMakedamage = false;
    [SerializeField] private float wallDamage = 1f;


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
        _animator.SetTrigger("IceWall");
        yield return new WaitForSeconds(_cooldown3);
        _canAction3 = true;
    }

    public void IceWall()
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = transform.position + 4 * transform.forward;
        cube.transform.localScale= new Vector3(lengthWall, 6.5f, 0.2f);
        cube.transform.rotation = transform.rotation;
        _audioManager.PlaySound("IceWall");
        cube.GetComponent<Renderer>().material = IceMaterial;
        if (WallMakedamage)
        {
            Collider[] cols = Physics.OverlapSphere(cube.transform.position, lengthWall);
            foreach (Collider col in cols)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    //Make damage to enemy with WallDamage
                }
            }
        }
        Destroy(cube, 5);
        //cube.transform.localScale()
    }

    public override void DamageSound()
    {
        _audioManager.PlaySound("DamageMage");
    }
}
