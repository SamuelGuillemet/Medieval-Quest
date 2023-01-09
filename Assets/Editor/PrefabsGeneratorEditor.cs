using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PrefabsGenerator))]
public class PrefabsGeneratorEditor : Editor
{
    SerializedProperty holes;
    SerializedProperty obstacles;
    SerializedProperty lands;

    SerializedProperty lightPrefab;
    SerializedProperty holePrefab;
    SerializedProperty obstaclePrefab;
    SerializedProperty landPrefab;
    SerializedProperty lightPole;
    SerializedProperty start;
    SerializedProperty wall;
    SerializedProperty angle;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        PrefabsGenerator prefabsGenerator = (PrefabsGenerator)target;

        lightPrefab.isExpanded = EditorGUILayout.Foldout(lightPrefab.isExpanded, "Prefabs", EditorStyles.foldoutHeader);
        if (lightPrefab.isExpanded)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(lightPrefab, new GUIContent("Light"));
            EditorGUILayout.PropertyField(holePrefab, new GUIContent("Hole"));
            EditorGUILayout.PropertyField(obstaclePrefab, new GUIContent("Obstacle"));
            EditorGUILayout.PropertyField(landPrefab, new GUIContent("Land"));
            EditorGUILayout.PropertyField(lightPole, new GUIContent("LightPole"));
            EditorGUILayout.PropertyField(start, new GUIContent("Start"));
            EditorGUILayout.PropertyField(wall, new GUIContent("Wall"));
            EditorGUILayout.PropertyField(angle, new GUIContent("Angle"));
            EditorGUI.indentLevel--;
        }

        GUILayout.Space(10);
        lands.isExpanded = EditorGUILayout.Foldout(lands.isExpanded, "Prefabs count", EditorStyles.foldoutHeader);
        if (lands.isExpanded)
        {
            EditorGUI.indentLevel++;
            GUI.enabled = false;
            EditorGUILayout.PropertyField(holes, new GUIContent("Holes"));
            EditorGUILayout.PropertyField(obstacles, new GUIContent("Obstacles"));
            EditorGUILayout.PropertyField(lands, new GUIContent("Lands"));
            GUI.enabled = true;
            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }

    void OnEnable()
    {
        holes = serializedObject.FindProperty("_holes");
        obstacles = serializedObject.FindProperty("_obstacles");
        lands = serializedObject.FindProperty("_lands");

        lightPrefab = serializedObject.FindProperty("LightPrefab");
        holePrefab = serializedObject.FindProperty("HolePrefab");
        obstaclePrefab = serializedObject.FindProperty("ObstaclePrefab");
        landPrefab = serializedObject.FindProperty("LandPrefab");
        lightPole = serializedObject.FindProperty("LightPole");
        start = serializedObject.FindProperty("Start");
        wall = serializedObject.FindProperty("Wall");
        angle = serializedObject.FindProperty("Angle");

        lightPrefab.isExpanded = true;

    }
}