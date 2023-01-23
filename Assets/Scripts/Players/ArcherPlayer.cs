using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 30 points de vie 
/// Action 1 : 
/// - Tir d’une flèche qui inflige 10 dégâts à la première cible touchée. 
/// - 1 seconde de cooldown. 
/// Action 2 : 
/// - Création d’un piège qui immobilise le premier ennemi qui marche dessus. 
/// - 3 secondes de cooldown.
/// Action 3 : 
/// - Double la vitesse de déplacement du personnage pendant 3 secondes. 
/// - Divise le cooldown de la première action par 2 pendant 3 secondes. 
/// - 10 secondes de cooldown.
/// Amélioration spécifique
/// - Gain de dégât sur l’attaque 1,
/// - Ajout puis gain de dégâts de zone à l’activation d’un piège,
/// - Gain de vitesse sur l’attaque 3,
/// - Augmentation du nombre d’ennemis touché avant la disparition d’une flèche,
/// - Augmentation du nombre d’ennemis touché avant la disparition d’un piège,
/// </summary>
public class ArcherPlayer : IPlayer
{
    [SerializeField] private ArrowPlayer _arrowPrefab;
    [SerializeField] private Trap _trapPrefab;
    private int _damageArrow;
    private int _damageTrap;
    private int _maxEnemyTouchedArrow;
    private int _maxEnemyTouchedTrap;
    private float _attack3Duration;
    private float _gainSpeedAttack3;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        MaxHealth = 30;

        _cooldown1 = 1f;
        _cooldown2 = 3f;
        _cooldown3 = 10f;

        _maxEnemyTouchedArrow = 1;
        _damageArrow = 10;

        _maxEnemyTouchedTrap = 1;
        _damageTrap = 10;

        _attack3Duration = 3f;
        _gainSpeedAttack3 = _playerAgent.Agent.speed;
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
        ArrowPlayer arrow = Instantiate(_arrowPrefab, transform.position + transform.forward + transform.forward + new Vector3(0, 2, 0), transform.rotation);
        arrow.transform.right = -transform.forward;
        arrow.Damage = _damageArrow;
        arrow.MaxEnemyTouched = _maxEnemyTouchedArrow;
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
        Trap trap = Instantiate(_trapPrefab, transform.position + Vector3.up * 0.1f, new Quaternion(0, 0, 0, 0));
        trap.Damage = _damageTrap;
        trap.MaxEnemyTouched = _maxEnemyTouchedTrap;
    }

    public void SwitchMovement()
    {
        _playerAgent.Agent.isStopped = !_playerAgent.Agent.isStopped;
    }

    public override IEnumerator Attack3()
    {
        _canAction3 = false;
        Action3();
        yield return new WaitForSeconds(_attack3Duration);
        UnAction3();
        yield return new WaitForSeconds(_cooldown3 - _attack3Duration);
        _canAction3 = true;
    }

    private void Action3()
    {
        _playerAgent.Agent.speed += _gainSpeedAttack3;
        _cooldown1 /= 2f;
    }
    private void UnAction3()
    {
        _playerAgent.Agent.speed -= _gainSpeedAttack3;
        _cooldown1 *= 2f;
    }

    #region Amelioration
    public override void SpecificUpgrade1()
    {
        _damageArrow += 1;
    }

    public override void SpecificUpgrade2()
    {
        _damageTrap += 1;
        _maxEnemyTouchedArrow += 1;
    }

    public override void SpecificUpgrade3()
    {
        _gainSpeedAttack3 += 0.5f;
    }

    public override void SpecificUpgrade4()
    {
        _maxEnemyTouchedArrow += 1;
    }

    public override void SpecificUpgrade5()
    {
        _maxEnemyTouchedTrap += 1;
    }
    #endregion
}
