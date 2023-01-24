using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GameUI))]
public class GameUIEditor : Editor
{

    public override void OnInspectorGUI()
    {
        GameUI gameui = (GameUI)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Popup"))
        {
            gameui.CreateUpgradeImage();
        }
    }
}