using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoorDemo : MonoBehaviour {


    public Animator TrapDoorAnim; 

    // Use this for initialization
    void Awake()
    {
        TrapDoorAnim = GetComponent<Animator>();
    }


    private void OnTriggerEnter(Collider infoCollision)
    {
        if (infoCollision.gameObject.tag == "Enemy")
        {
            //Immobilise le'enemy oendant trois secondes 
            Destroy(gameObject);
        }
    }

}