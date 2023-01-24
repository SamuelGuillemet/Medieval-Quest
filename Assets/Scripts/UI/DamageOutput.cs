using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageOutput : MonoBehaviour
{
    public static DamageOutput Create(GameObject prefab, Vector3 position, int damage)
    {
        Transform damageOutputTransform = Instantiate(
            prefab.transform,
            position,
            Quaternion.Euler(90, 0, 0)
        );

        DamageOutput damageOutput = damageOutputTransform.GetComponent<DamageOutput>();
        damageOutput.Setup(damage);

        return damageOutput;
    }

    private static int _sortingOrder;

    private const float DISAPPEAR_TIMER_MAX = 1f;

    private TextMeshPro _textMesh;
    private float _disappearTimer;
    private Color _textColor;
    private Vector3 moveVector;

    private void Awake()
    {
        _textMesh = transform.GetComponent<TextMeshPro>();
        moveVector = new Vector3(.1f, 0, 0.5f) * 30f;
    }

    public void Setup(int damage)
    {
        _textMesh.text = damage.ToString();
        _disappearTimer = DISAPPEAR_TIMER_MAX;
        _sortingOrder++;
        _textMesh.sortingOrder = _sortingOrder;
    }

    private void Update()
    {
        transform.position += moveVector * Time.deltaTime;
        moveVector -= moveVector * 8f * Time.deltaTime;

        if (_disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            // First half of the timer
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            // Second half of the timer
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }
        _disappearTimer -= Time.deltaTime;

        if (_disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            _textColor = _textMesh.color;
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _textMesh.color = _textColor;

            if (_textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
