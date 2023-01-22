using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Use UpgradeMenu instead", true)]
public class UpgradeStand : MonoBehaviour
{
    private Camera _cam;
    private bool _isInUpgradeZone = false;

    private void Start()
    {
        _cam = Camera.main;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInUpgradeZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isInUpgradeZone = false;
        }
    }

    private void Update()
    {
        if (_isInUpgradeZone)
        {
            // Handle the click on a upgrade stand
            ClickOnUpgradeStand();
        }
    }

    private void ClickOnUpgradeStand()
    {
        // Open the upgrade menu
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "UpgradeStand")
                {
                    GameObject upgradeStand = hit.collider.gameObject;

                    // Play the particle effect
                    ParticleSystem particleSystem =
                        upgradeStand.GetComponentInChildren<ParticleSystem>();

                    if (particleSystem.isPlaying)
                        particleSystem.Stop();
                    particleSystem.Play();
                }
            }
        }
    }
}
