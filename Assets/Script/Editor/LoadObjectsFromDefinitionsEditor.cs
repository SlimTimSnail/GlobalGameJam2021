using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoadObjectsFromDefinitions))]
public class LoadObjectsFromDefinitionsEditor : Editor
{
    void OnEnable()
    {
    }
    override public void OnInspectorGUI()
    {
        LoadObjectsFromDefinitions objectLoader = (LoadObjectsFromDefinitions)target;
        if (GUILayout.Button("Spawn From Definitions"))
        {
            objectLoader.SpawnObjects();
        }
        DrawDefaultInspector();
    }
}
