using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(IEnemy), true)]
public class IEnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IEnemy iEnemy = (IEnemy)target;

        DrawDefaultInspector();

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(iEnemy.transform.position);
        if (screenPoint.x < 0f || screenPoint.x > 1f || screenPoint.y < 0f || screenPoint.y > 1f)
        {
            GUI.color = Color.red;
        }
        else
        {
            GUI.color = Color.green;
        }
        EditorGUILayout.Vector2Field("Screen Point", new Vector2(screenPoint.x, screenPoint.y));
        GUI.color = Color.white;


        EditorGUILayout.Space(10);

        if (GUILayout.Button("DeactivateEnemy"))
        {
            iEnemy.DeactivateEnemy();
        }

        if (GUILayout.Button("ActivateEnemy"))
        {
            iEnemy.ActivateEnemy();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Health: " + iEnemy.Health);
        if (GUILayout.Button("TakeDamage"))
        {
            GameManager.Instance.OnEnemyDamageTaken?.Invoke(3, iEnemy, 1);
        }
    }
}