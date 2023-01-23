using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
        
    [SerializeField] private Mage player;

    [SerializeField] private int enemyTouched;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<Mage>();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.RotateAround(player.transform.position, Vector3.up, player.speedOrb * Time.deltaTime);
        if (enemyTouched == player.maxEnemyTouched) 
        {
            Destroy(gameObject);
        }

    }

    void OnCollisionEnter(Collision infoCollision)
    {
      
        if (infoCollision.gameObject.tag == "Enemy")
        {
            // TODO : makeDamageTo 
            enemyTouched += 1;
            if (player.orbRepulsion)
            {
                // TODO : repulse enemy
            }
        }

    }

}
