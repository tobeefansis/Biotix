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
        if (!selcet.level) return;

        if (GUILayout.Button("Save"))
        {
            selcet.level.enemys.Clear();
            selcet.level.nodes.Clear();

            foreach (Transform item in selcet.root.transform)
            {
                var node = item.GetComponent<Node>();
                if (node)
                {
                    selcet.level.nodes.Add(node.GetJson());
                }
            }
            foreach (Transform item in selcet.enemys.transform)
            {
                var enemy = item.GetComponent<Enemy>();
                if (enemy)
                {
                    selcet.level.enemys.Add(enemy.GetJson());
                }
            }
        }
        if (GUILayout.Button("Loasdfdsfsdd"))
        {
            while (selcet.root.childCount > 0)
            {
                DestroyImmediate(selcet.root.GetChild(0).gameObject);
            }

        }
        if (GUILayout.Button("Load"))
        {
            while (selcet.root.childCount > 0)
            {
                DestroyImmediate(selcet.root.GetChild(0).gameObject);
            }
            while (selcet.enemys.childCount > 0)
            {
                DestroyImmediate(selcet.enemys.GetChild(0).gameObject);
            }
            
            foreach (var item in selcet.level.nodes)
            {
                var node = Instantiate(selcet.NodePrefub, selcet.root);
                node.SetJson(item);
            }
            foreach (var item in selcet.level.enemys)
            {
                var enemy = Instantiate(selcet.NodeGroupPrefub, selcet.enemys);
                enemy.SetJson(item);
            }
        }

    }

    public string GetValue()
    {
        return JsonUtility.ToJson(this);
    }

    public void SetValue(string value)
    {
        JsonUtility.FromJsonOverwrite(value, this);
    }

    private void Awake()
    {
        selcet = (LevelEditor)target;
    }

}
