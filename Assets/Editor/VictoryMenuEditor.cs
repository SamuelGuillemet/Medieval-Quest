using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VictoryMenu))]
public class VictoryMenuEditor : Editor
{

    public override void OnInspectorGUI()
    {
        VictoryMenu vic = (VictoryMenu)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Victory"))
        {
            vic.VictoryMenuButton();
        }
    }
}