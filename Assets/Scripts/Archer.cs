using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{

    Animator _animator;

    public int health = 30;

    
    public int action1Cooldown = 1;
    private bool canShoot = true;
    public GameObject arrowPrefab;
    public Vector3 positionOffset;
    public Quaternion rotationOffset;

    public int action2Cooldown = 3;
    private bool canPlaceTrap = true;
    public GameObject trapPrefab;

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
        if (Input.GetMouseButton(1) && canPlaceTrap && !player.isMoving)
        {
            StartCoroutine(PlaceTrapCoroutine());
        }
    }

    IEnumerator ThrowArrowCoroutine()
    {
        canShoot = false;
        _animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(action1Cooldown);
        canShoot = true;
    }

    public void ThrowArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position + positionOffset, transform.rotation /* * rotationOffset */);
        arrow.AddComponent<Arrow>();
    }

    IEnumerator PlaceTrapCoroutine()
    {
        canPlaceTrap = false;
        _animator.SetTrigger("PlaceTrap");
        yield return new WaitForSeconds(action2Cooldown);
        canPlaceTrap = true;
    }

    public void PlaceTrap()
    {
        GameObject trap = Instantiate(trapPrefab, transform.position, new Quaternion(0,0,0,0));
    }
}
