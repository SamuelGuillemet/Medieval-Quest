using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(IPlayer), true)]
public class IPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        IPlayer iPlayer = (IPlayer)target;

        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Health: " + iPlayer.Health);

        EditorGUILayout.FloatField("Current Cooldown 1", iPlayer.GetCoolDowns(1));
        EditorGUILayout.FloatField("Current Cooldown 2", iPlayer.GetCoolDowns(2));
        EditorGUILayout.FloatField("Current Cooldown 3", iPlayer.GetCoolDowns(3));

        if (EditorApplication.isPlaying)
            Repaint();
    }
}
