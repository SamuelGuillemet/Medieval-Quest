using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DefeatMenu))]
public class DefeatMenuEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DefeatMenu def = (DefeatMenu)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Defeat"))
        {
            def.DefeatMenuButton();
        }
    }
}