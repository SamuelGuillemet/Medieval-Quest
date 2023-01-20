using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : IPlayer
{


    // TODO : Serializes fields


    [SerializeField] private Arrow arrowPrefab;
    public float damage = 10;
    public int maxEnemyTouched;

    public GameObject trapPrefab;

    public float action3Duration = 3f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        MaxHealth = 40;

        _cooldown1 = 1f;
        _cooldown2 = 3f;
        _cooldown3 = 10f;
    }

    public override IEnumerator Attack1()
    {
        _canAction1 = false;

        yield return new WaitForSeconds(_cooldown1);
        _canAction1 = true;
    }


    public override IEnumerator Attack2()
    {
        _canAction2 = false;

        yield return new WaitForSeconds(_cooldown2);
        _canAction2 = true;

    }

    public override IEnumerator Attack3()
    {
        _canAction3 = false;
        yield return new WaitForSeconds(_cooldown3);
        _canAction3 = true;
    }

}
