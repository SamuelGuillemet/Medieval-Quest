using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 40 points de vie 
///  Action 1 : 
///  - Tir d’une boule de feu qui explose à l’impact d’un mur ou d’un ennemi. 
///  - En explosant elle inflige 5 dégâts et repousse les ennemis du centre de l’explosion. 
///  - 1.5 secondes de cooldown.
///  Action 2 : 
///  - Création d’un projectile Orbe qui orbite autour du personnage pendant 5 secondes ou jusqu’à avoir percuté 5 ennemis. 
///  - Le projectile inflige 2 dégâts en traversant un ennemi. 
///  - 7 secondes de cooldown.
///  Action 3 : 
///  - Création d’un mur de glace qui disparaît après 5 secondes. 
///  - Le mur n’est traversable ni par le personnage, ni par les ennemis, ni par les projectiles. 
///  - Ne s’active pas si la souris est au-dessus d’une case de fosse. 
///  - 10 secondes de cooldown.
/// Amélioration spécifique :
/// - Gain de dégât sur l’attaque 1,
/// - Ajout puis gain de répulsion sur le projectile de l’attaque 2,
/// - Augmentation du nombre d’ennemis touché avant la disparition d’une orbe,
/// - Augmentation de la taille du mur.
/// - Ajout puis gain de dégâts de zone à l’apparition du mur.
/// </summary>
public class Mage : IPlayer
{
    [SerializeField] private FireBall _fireBallPrefab;
    [SerializeField] private GameObject _orbPrefab;
    [SerializeField] private GameObject _wallPrefab;
    [SerializeField] private GameObject _hand;
    private int _attack1Damage;
    private float _attack2Duration;
    private int _maxEnemyTouched;
    private float _orbRepulsion;

    public int MaxEnemyTouched { get => _maxEnemyTouched; }
    public float OrbRepulsion { get => _orbRepulsion; }

    private float _lengthWall;
    private int _wallDamage;

    public override void OnEnable()
    {
        base.OnEnable();

        MaxHealth = 40;

        _cooldown1 = 1.5f;
        _cooldown2 = 7f;
        _cooldown3 = 10f;

        _attack1Damage = 5;

        _maxEnemyTouched = 5;
        _attack2Duration = 5f;
        _orbRepulsion = 0;

        _lengthWall = 2f;
        _wallDamage = 0;
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
        FireBall fireBall = _poolingManager.SpawnObjectFromPool(_fireBallPrefab, _hand.transform.position, _hand.transform.rotation);
        fireBall.Damage = _attack1Damage;
        fireBall.SetMovement();
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
        GameObject orb = _poolingManager.SpawnObjectFromPool(_orbPrefab, transform.position, transform.rotation);
        _poolingManager.ReturnToPool(orb, _attack2Duration);
    }


    public void SwitchMovement()
    {
        _playerAgent.Agent.isStopped = !_playerAgent.Agent.isStopped;
    }

    public override IEnumerator Attack3()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit, 100f, LayerMask.GetMask("Floor")))
        {
            yield break;
        }
        _canAction3 = false;
        _animator.SetTrigger("IceWall");
        _audioManager.PlaySound("IceWall");
        yield return new WaitForSeconds(_cooldown3);
        _canAction3 = true;
    }

    public void IceWall()
    {
        Vector3 spawnPosition = transform.position + 3 * transform.forward;
        spawnPosition.y = 3.5f;
        GameObject wall = _poolingManager.SpawnObjectFromPool(_wallPrefab, spawnPosition, transform.rotation);
        wall.transform.localScale = new Vector3(_lengthWall, 3f, 0.2f);

        if (_wallDamage > 0)
        {
            Collider[] cols = Physics.OverlapSphere(wall.transform.position, _lengthWall);
            foreach (Collider col in cols)
            {
                if (col.gameObject.tag == "Enemy")
                {
                    IEnemy enemy = col.gameObject.GetComponent<IEnemy>();
                    _gameManager.OnEnemyDamageTaken?.Invoke(_wallDamage, enemy, 1);
                }
            }
        }
        _poolingManager.ReturnToPool(wall, 5f);
    }

    public override void DamageSound()
    {
        _audioManager.PlaySound("DamageMage");
    }


    #region Amelioration

    public override void SpecificUpgrade1()
    {
        _attack1Damage += 1;
    }

    public override void SpecificUpgrade2()
    {
        _orbRepulsion += 0.5f;
    }

    public override void SpecificUpgrade3()
    {
        _maxEnemyTouched += 1;
    }

    public override void SpecificUpgrade4()
    {
        _lengthWall += 0.5f;
    }

    public override void SpecificUpgrade5()
    {
        _wallDamage += 1;
    }

    #endregion Amelioration

}
