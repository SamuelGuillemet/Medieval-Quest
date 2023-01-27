using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoad : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<Camera>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
