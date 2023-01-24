using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 40 points de vie 
///  Action 1 : 
///  - Tir d’une boule de feu qui explose à l’impact d’un mur ou d’un ennemi. 
///  - En explosant elle inflige 5 dégâts et repousse les ennemis du centre de l’explosion. 
///  - 2 secondes de cooldown.
///  Action 2 : 
///  - Création d’un projectile Orbe qui orbite autour du personnage pendant 5 secondes ou jusqu’à avoir percuté 5 ennemis. 
///  - Le projectile inflige 2 dégâts en traversant un ennemi. 
///  - 10 secondes de cooldown.
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
    [SerializeField] private FireBall _fireBallPrefab; // TODO Pooling
    [SerializeField] private Orb _orbPrefab; // TODO Pooling
    [SerializeField] private GameObject _hand;
    private int _attack1Damage = 5;
    private float _attack2Duration = 5f;
    private int _maxEnemyTouched;
    private float _orbRepulsion;

    public int MaxEnemyTouched { get => _maxEnemyTouched; }
    public float OrbRepulsion { get => _orbRepulsion; }

    [SerializeField] public float lengthWall = 1f;
    [SerializeField] private Material IceMaterial;
    [SerializeField] private bool WallMakedamage = false;
    [SerializeField] private float wallDamage = 1f;

    public override void Start()
    {
        base.Start();

        MaxHealth = 40;

        _cooldown1 = 2f;
        _cooldown2 = 10f;
        _cooldown3 = 10f;

        _maxEnemyTouched = 5;
        _orbRepulsion = 0;
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
        FireBall fireBall = Instantiate(_fireBallPrefab, _hand.transform.position, _hand.transform.rotation);
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
        Vector3 offset = new Vector3(0, 2, 2);
        Orb orb = Instantiate(_orbPrefab, transform.position + offset, transform.rotation, transform);
        orb.OrbRepulsion = _orbRepulsion;
        Destroy(orb.gameObject, _attack2Duration); // TODO Pooling
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
        cube.transform.localScale = new Vector3(lengthWall, 6.5f, 0.2f);
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
        // TODO Increase IceWall size
    }

    public override void SpecificUpgrade5()
    {
        // TODO Add damage on IceWall
    }

    #endregion Amelioration

}
