using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{

    Animator _animator;

    public int health = 30;

    
    private bool canShoot = true;
    public Arrow arrowPrefab;
    public float damage = 10;
    public float shootCoolDown = 1f;
    public int maxEnemyTouched;

    private bool canPlaceTrap = true;
    public GameObject trapPrefab;
    public float trapCoolDown = 3f;

    private bool canAction3 = true;
    public float action3Duration = 3f;
    public float action3CoolDown = 10f;

    private HeroController player;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        player = GetComponentInParent<HeroController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canShoot)
        {
            StartCoroutine(ThrowArrowCoroutine());
        }
        if (Input.GetMouseButton(1) && canPlaceTrap)
        {
            StartCoroutine(PlaceTrapCoroutine());
        }
        if (Input.GetKey(KeyCode.Space) && canAction3)
        {
            StartCoroutine(Action3Coroutine());
        }
    }

    IEnumerator ThrowArrowCoroutine()
    {
        canShoot = false;
        _animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(shootCoolDown);
        canShoot = true;
    }

    public void ThrowArrow()
    {
        Arrow arrow = Instantiate(arrowPrefab, transform.position + transform.forward + transform.forward + new Vector3(0,2,0), transform.rotation);
        arrow.transform.right = -transform.forward;
        arrow.damage = damage;
        arrow.maxEnemyTouched = maxEnemyTouched;
    }

    IEnumerator PlaceTrapCoroutine()
    {
        canPlaceTrap = false;   
        _animator.SetTrigger("PlaceTrap");
        yield return new WaitForSeconds(trapCoolDown);
        canPlaceTrap = true;

    }

    public void PlaceTrap()
    {
        GameObject trap = Instantiate(trapPrefab, transform.position , new Quaternion(0,0,0,0));
    }

    public void MovePlayer()
    {
        player.enabled = !player.enabled;
    }

    IEnumerator Action3Coroutine()
    {
        canAction3 = false;
        Action3();
        yield return new WaitForSeconds(action3Duration);
        UnAction3();
        yield return new WaitForSeconds(action3CoolDown - action3Duration);
        canAction3 = true;
    }

    public void Action3()
    {
        //Double la vitesse du perso
        shootCoolDown = shootCoolDown / 2;
    }
    public void UnAction3()
    {
        //divise la vitesse du perso par deux
        shootCoolDown = shootCoolDown * 2;
    }
}
