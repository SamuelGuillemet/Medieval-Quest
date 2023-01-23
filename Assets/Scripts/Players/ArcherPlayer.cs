using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherArcherPlayer: IPlayer
{   
    [SerializeField] private ArrowPlayer arrowPrefab;
    [SerializeField] public float damage = 10;
    [SerializeField] public int maxEnemyTouched;

    [SerializeField] public GameObject trapPrefab;

    [SerializeField] public float action3Duration = 3f;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        MaxHealth = 30;
        _cooldown1 = 1f;

        _cooldown2 = 3f;

        _cooldown3 = 10f;
    }

    public override IEnumerator Attack1()
    {
        _canAction1 = false;
        _animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(_cooldown1);
        _canAction1 = true;
    }

    public void ThrowArrow()
    {
        ArrowPlayer arrow = Instantiate(arrowPrefab, transform.position + transform.forward + transform.forward + new Vector3(0,2,0), transform.rotation);
        arrow.transform.right = -transform.forward;
        arrow.damage = damage;
        arrow.maxEnemyTouched = maxEnemyTouched;
    }

    public override IEnumerator Attack2()
    {
        _canAction2 = false;   
        _animator.SetTrigger("PlaceTrap");
        yield return new WaitForSeconds(_cooldown2);
        _canAction2 = true;

    }

    public void PlaceTrap()
    {
        GameObject trap = Instantiate(trapPrefab, transform.position , new Quaternion(0,0,0,0));
    }

    public void SwitchMovement()
    {
        _playerAgent.Agent.isStopped = !_playerAgent.Agent.isStopped;
    }

    public override IEnumerator Attack3()
    {
        _canAction3 = false;
        Action3();
        yield return new WaitForSeconds(action3Duration);
        UnAction3();
        yield return new WaitForSeconds(_cooldown3 - action3Duration);
        _canAction3 = true;
    }

    private void Action3()
    {
        _playerAgent.Agent.speed *= 2f;
        _cooldown1 /= 2f;
    }
    private void UnAction3()
    {
        _playerAgent.Agent.speed /= 2f;
        _cooldown1 *= 2f;
    }
}
