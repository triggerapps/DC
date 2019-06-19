using UnityEngine;
using UnityEditor;
using System.Collections;


[CanEditMultipleObjects]

public class vThirdPersonInputEditor : Editor
{
    GUISkin skin;

    public override void OnInspectorGUI()
    {
        if (!skin) skin = Resources.Load("skin") as GUISkin;
        GUI.skin = skin;

        GUILayout.BeginVertical("INPUT MANAGER", "window");

        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();        
       
        base.OnInspectorGUI();
        
        GUILayout.Space(10);
             

        GUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();
    }
}