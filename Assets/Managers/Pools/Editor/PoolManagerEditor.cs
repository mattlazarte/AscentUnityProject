using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ascent.Managers.Pools
{
    [CustomEditor(typeof(PoolManager))]
    public class PoolManagerEditor : Editor
    {
        private PoolManager poolMgr;

        public void OnEnable()
        {
            poolMgr = (PoolManager)target;
            poolMgr.RefreshIncludedTypes();
        }

        public override bool RequiresConstantRepaint()
        {
            return Application.isPlaying;
        }

        public override void OnInspectorGUI()
        {
            if (poolMgr.poolSettings.Count == 0)
            {
                EditorGUILayout.LabelField("No script with pool attribute found.");
            }
            else
            {
                foreach (var poolSetup in poolMgr.poolSettings)
                {
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField("Pool: " + poolSetup.TypeName, EditorStyles.boldLabel);

                    if (Application.isPlaying)
                    {
                        var poolType = Type.GetType(poolSetup.AssemblyQualifiedTypeName);
                        if (poolMgr.pools.ContainsKey(poolType))
                        {
                            var pool = poolMgr.pools[poolType];
                            EditorGUILayout.LabelField("Initial size: " + poolSetup.Size);
                            EditorGUILayout.LabelField("Current size: " + pool.Queue.Count);
                            EditorGUILayout.LabelField("Late instantiations: " + pool.LateInstantiations);
                        }
                        else
                        {
                            EditorGUILayout.LabelField("Pool not created.");
                        }
                    }
                    else
                    {
                        poolSetup.Size = EditorGUILayout.IntField("Size", poolSetup.Size);
                        poolSetup.Prefab = (GameObject)EditorGUILayout.ObjectField("Prefab", poolSetup.Prefab, typeof(GameObject), false);
                    }
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}