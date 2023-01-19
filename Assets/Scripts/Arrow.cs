using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public float speed = 40f;
    public float damage;
    public int enemyTouched = 0;
    public int maxEnemyTouched = 1;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= transform.right * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision infoCollision) 
    {
        if (infoCollision.gameObject.name == "Enemy")
        {
            enemyTouched += 1;
            //makeDamageToEnemy
        }
        Debug.Log(infoCollision.gameObject);
        if ((infoCollision.gameObject.tag != "Player" && infoCollision.gameObject.name != "Enemy") || enemyTouched == maxEnemyTouched )
        {
            Destroy(gameObject);
        }

    } 

}
