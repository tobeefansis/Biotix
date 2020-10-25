using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEditor;
using UnityEditor.Graphs;

using UnityEngine;

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorInspector : Editor
{
    public LevelEditor selcet;
    public GameObject obj;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        GUILayout.Label("Level");
        serializedObject.Update();
        selcet.level = (Level)EditorGUILayout.ObjectField(selcet.level, typeof(Level), false);
        if (GUILayout.Button("Clear"))
        {
            selcet.Clear();
        }
        if (!selcet.level) return;

        if (GUILayout.Button("Save"))
        {
            selcet.Save();
        }
        if (GUILayout.Button("Load"))
        {
            selcet.Load();
        }
       
    }

    private void Awake()
    {
        selcet = (LevelEditor)target;
    }

}
