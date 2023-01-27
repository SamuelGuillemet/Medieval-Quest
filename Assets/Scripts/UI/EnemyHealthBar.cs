using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Transform target;
    private Vector3 _offset = new Vector3(0, 3, 1);
    private Quaternion _rotation = Quaternion.Euler(90, 0, 0);

    private void LateUpdate()
    {
        transform.position = target.position + _offset;
        transform.rotation = _rotation;
    }
}
