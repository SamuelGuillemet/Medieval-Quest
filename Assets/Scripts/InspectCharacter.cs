using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectCharacter : MonoBehaviour
{
    [SerializeField]
    private GameObject character;
    private bool _isInspected;

    private void OnEnable()
    {
        _isInspected = false;
        character.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnMouseDrag()
    {
        _isInspected = true;
        character.transform.Rotate(0, Input.GetAxis("Mouse X") * -10f, 0);
    }

    private void OnMouseUp()
    {
        _isInspected = false;
    }

    private void Update()
    {
        if (!_isInspected)
        {
            character.transform.Rotate(0, -0.2f, 0);
        }
    }
}
