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

    /// <summary>
    /// All this stuff is just for debug purpose to see the camera bounds in the scene view
    /// </summary>
    private void OnDrawGizmos()
    {
        float _offsetOnScreen = 0.05f;
        Camera _camera = Camera.main;

        Vector3 a = _camera.ViewportToWorldPoint(new Vector3(_offsetOnScreen, _offsetOnScreen, _camera.transform.position.y));
        Vector3 b = _camera.ViewportToWorldPoint(new Vector3(1f - _offsetOnScreen, _offsetOnScreen, _camera.transform.position.y));
        Vector3 c = _camera.ViewportToWorldPoint(new Vector3(1f - _offsetOnScreen, 1f - 2f * _offsetOnScreen, _camera.transform.position.y));
        Vector3 d = _camera.ViewportToWorldPoint(new Vector3(_offsetOnScreen, 1f - 2f * _offsetOnScreen, _camera.transform.position.y));

        Vector3 aDirection = (a - _camera.transform.position).normalized;
        Vector3 aOnFloor = _camera.transform.position + aDirection * ((2f - _camera.transform.position.y) / aDirection.y);
        Vector3 bDirection = (b - _camera.transform.position).normalized;
        Vector3 bOnFloor = _camera.transform.position + bDirection * ((2f - _camera.transform.position.y) / bDirection.y);
        Vector3 cDirection = (c - _camera.transform.position).normalized;
        Vector3 cOnFloor = _camera.transform.position + cDirection * ((2f - _camera.transform.position.y) / cDirection.y);
        Vector3 dDirection = (d - _camera.transform.position).normalized;
        Vector3 dOnFloor = _camera.transform.position + dDirection * ((2f - _camera.transform.position.y) / dDirection.y);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(aOnFloor, bOnFloor);
        Gizmos.DrawLine(bOnFloor, cOnFloor);
        Gizmos.DrawLine(cOnFloor, dOnFloor);
        Gizmos.DrawLine(dOnFloor, aOnFloor);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(_camera.transform.position, aDirection * 50f);
        Gizmos.DrawRay(_camera.transform.position, bDirection * 50f);
        Gizmos.DrawRay(_camera.transform.position, cDirection * 50f);
        Gizmos.DrawRay(_camera.transform.position, dDirection * 50f);
    }
}

