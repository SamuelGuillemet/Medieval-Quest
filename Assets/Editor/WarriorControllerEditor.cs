using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WarriorAnimsFREE;

[CustomEditor(typeof(WarriorController))]
public class WarriorControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WarriorController warriorController = (WarriorController)target;

        DrawDefaultInspector();

        GUILayout.Space(10);

        if (GUILayout.Button("Attack"))
        {
            warriorController.Attack1();
        }
    }
}
