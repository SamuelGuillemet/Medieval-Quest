using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 50 points de vie 
/// Action 1 : 
///  - Explosion qui fait 10 dégâts aux ennemis dans une sphère devant le personnage. 
///  - 1 seconde de cooldown.
/// Action 2 : 
/// - Bond qui fait une onde de choc et qui fait 3 dégâts à l'impact et repousse les cibles alentour. 
/// - 5 secondes de cooldown.
/// Action 3 : 
/// - Création d'un mignon qui soigne le personnage de 1 point de vie toutes les 2 secondes pendant 6 secondes.
/// - Le mignon a 3 points de vie.
/// - 15 secondes de cooldown.
/// Amélioration spécifique
/// - Gain de dégât sur l’attaque 1,
/// - Gain de dégât sur l’attaque 2,
/// - Ajout d’un knockback sur l’attaque 1,
/// - Réduction du delai de soin du mignon,
/// - Augmentation des points de vie du mignon,
/// </summary>
public class Demon : IPlayer
{
    [SerializeField] private Mignon _mignonPrefab;
    [SerializeField] private GameObject _chocWave;

    private int _damageAttack1;
    private float _attackRadius;
    private float _knockback1;

    private int _mignonMaxHealth;
    private float _mignonDelayBetweenCare;

    private float magnitude = 0.1f;
    private int _chocWaveRadius;
    private int _chocWaveDamage;


    // Start is called before the first frame update
    public override void OnEnable()
    {
        base.OnEnable();

        MaxHealth = 50;

        _cooldown1 = 1f;
        _cooldown2 = 5f;
        _cooldown3 = 15f;

        _knockback1 = 0;
        _damageAttack1 = 10;
        _attackRadius = 2f;

        _mignonMaxHealth = 3;
        _mignonDelayBetweenCare = 2f;

        _chocWaveRadius = 4;
        _chocWaveDamage = 3;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, _attackRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _chocWaveRadius);
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
        Collider[] cols = Physics.OverlapSphere(transform.position + transform.forward, _attackRadius);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Enemy")
            {
                IEnemy enemy = col.gameObject.GetComponent<IEnemy>();
                _gameManager.OnEnemyDamageTaken?.Invoke(_damageAttack1, enemy, _knockback1);
            }
        }
    }


    public override IEnumerator Attack2()
    {
        _canAction2 = false;
        _animator.SetTrigger("ChocWave");
        _audioManager.PlaySound("DemonChocWave");
        _chocWave.SetActive(true);
        yield return new WaitForSeconds(_cooldown2);
        _canAction2 = true;
    }

    public void ChocWave()
    {
        StartCoroutine(Shake());
        _chocWave.transform.localScale = new Vector3(1f, 1f, 1f);
        Collider[] cols = Physics.OverlapSphere(transform.position, _chocWaveRadius);
        foreach (Collider col in cols)
        {
            if (col.gameObject.tag == "Enemy")
            {
                IEnemy enemy = col.gameObject.GetComponent<IEnemy>();
                _gameManager.OnEnemyDamageTaken?.Invoke(_chocWaveDamage, enemy, 2);
            }
        }
    }


    IEnumerator Shake()
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < 2f)
        {
            float x = Random.Range(-0.5f, 0.5f) * magnitude;
            float y = Random.Range(-0.5f, 0.5f) * magnitude;
            Camera.main.transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            elapsed += Time.deltaTime * 5;
            yield return null;
        }
        _chocWave.SetActive(false);
        _chocWave.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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
        Mignon mignon = _poolingManager.SpawnObjectFromPool(_mignonPrefab, transform.position + new Vector3(1, 0, 1), Quaternion.identity);
        mignon.MaxHealth = _mignonMaxHealth;
        mignon.DelayBetweenCare = _mignonDelayBetweenCare;
        mignon.Speed = _playerAgent.Agent.speed;
    }

    public void SwitchMovement()
    {
        _playerAgent.Agent.isStopped = !_playerAgent.Agent.isStopped;
    }

    public override void DamageSound()
    {
        _audioManager.PlaySound("DamageDemon");
    }

    #region Amelioration

    public override void SpecificUpgrade1()
    {
        _damageAttack1 += 1;
    }

    public override void SpecificUpgrade2()
    {
        _chocWaveDamage += 1;
    }

    public override void SpecificUpgrade3()
    {
        _knockback1 += 0.5f;
    }

    public override void SpecificUpgrade4()
    {
        _mignonDelayBetweenCare -= 0.2f;
    }

    public override void SpecificUpgrade5()
    {
        _mignonMaxHealth += 1;
    }

    #endregion
}

