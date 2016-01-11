using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    [CustomEditor(typeof(WaypointsConnection))]
    [CanEditMultipleObjects]
    public class WaypointsConnectionEditor : Editor
    {
        WaypointsConnection[] conns;
        SerializedProperty enabledProp;

        public void OnEnable()
        {
            enabledProp = serializedObject.FindProperty("enabled");

            if (targets != null)
            {
                conns = new WaypointsConnection[targets.Length];

                for (var i = 0; i < targets.Length; i++)
                {
                    conns[i] = (WaypointsConnection)targets[i];
                }
            }
        }

        private void OnSceneGUI()
        {
            if (conns != null)
            {
                foreach (var conn in conns)
                {
                    var str = conn.cost.ToString();

                    if (!string.IsNullOrEmpty(conn.name))
                    {
                        str += " [" + conn.name + "]";
                    }

                    Handles.Label(conn.transform.position, str);
                }
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var conn = ((WaypointsConnection)target);

            if (GUILayout.Button("Select Network"))
            {
                Selection.activeGameObject = conn.parentNetwork.gameObject;
            }

            if (targets.Length == 1)
            {
                conn.name = EditorGUILayout.TextField("Name", conn.name);

                if (conn.name != null)
                {
                    conn.name = conn.name.Trim();
                }
            }
            else
            {
                if (GUILayout.Button("Clear Names"))
                {
                    ClearNames();
                }
            }

            EditorGUILayout.PropertyField(enabledProp, new GUIContent("Enabled"));

            if (GUI.changed)
            {
                EditorUtility.SetDirty(conn.gameObject);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void ClearNames()
        {
            foreach (var obj in targets)
            {
                var conn = (WaypointsConnection)obj;
                conn.name = null;
            }
        }
    }
}
