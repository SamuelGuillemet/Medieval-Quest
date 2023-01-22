using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(UpgradeMenu))]
public class UpgradeMenuEditor : Editor
{

    public override void OnInspectorGUI()
    {
        UpgradeMenu up = (UpgradeMenu)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Upgrade"))
        {
            up.EnableUpgradePopup();
        }
    }
}