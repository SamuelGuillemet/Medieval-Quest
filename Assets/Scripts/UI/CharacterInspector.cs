using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInspector : MonoBehaviour
{
    private bool _isInspected;

    private void OnEnable()
    {
        _isInspected = false;
        gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void OnMouseDrag()
    {
        _isInspected = true;
        gameObject.transform.Rotate(0, Input.GetAxis("Mouse X") * -10f, 0);
    }

    private void OnMouseUp()
    {
        _isInspected = false;
    }

    private void Update()
    {
        if (!_isInspected)
        {
            gameObject.transform.Rotate(0, -0.2f, 0);
        }
    }
}
