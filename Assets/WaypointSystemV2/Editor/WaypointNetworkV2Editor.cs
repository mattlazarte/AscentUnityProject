using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    [CustomEditor(typeof(WaypointNetworkV2))]
    public class WaypointNetworkV2Editor : Editor
    {
        private WaypointNetworkV2 network;

        public void OnEnable()
        {
            network = (WaypointNetworkV2)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Create waypoint"))
            {
                CreateWaypoint();
            }

            if (network.waypoints != null && network.waypoints.Count > 0)
            {
                if (GUILayout.Button("Delete network"))
                {
                    if (EditorUtility.DisplayDialog(
                        "Network clearing confirmation", 
                        "Are sure you want to delete all the waypoints and connections?", 
                        "Yes", "No"))
                    {
                        DeleteAllConnections();
                        DeleteAllWaypoints();
                    }
                }
            }

            EditorGUILayout.Separator();

            if (Application.isPlaying)
            {
                if (network.runtimeWaypoints == null)
                {
                    EditorGUILayout.LabelField("Waypoints: 0");
                }
                else
                {
                    EditorGUILayout.LabelField("Waypoints: " + network.runtimeWaypoints.Count);
                }

                if (network.runtimeConnections == null)
                {
                    EditorGUILayout.LabelField("Connections: 0");
                }
                else
                {
                    EditorGUILayout.LabelField("Connections: " + network.runtimeConnections.Count);
                }
            }
            else
            {
                if (network.waypoints == null)
                {
                    EditorGUILayout.LabelField("Waypoints: 0");
                }
                else
                {
                    EditorGUILayout.LabelField("Waypoints: " + network.waypoints.Count);
                }

                if (network.connections == null)
                {
                    EditorGUILayout.LabelField("Connections: 0");
                }
                else
                {
                    EditorGUILayout.LabelField("Connections: " + network.connections.Count);
                }
            }

            EditorGUILayout.Separator();
            network.displayWaypoints = EditorGUILayout.Toggle("Display Waypoints", network.displayWaypoints);
            network.waypointsRadius = EditorGUILayout.FloatField("Waypoint Radius", network.waypointsRadius);
            network.waypointsColor = EditorGUILayout.ColorField("Waypoint Color", network.waypointsColor);
            network.displayConnections = EditorGUILayout.Toggle("Display Waypoints Connections", network.displayConnections);
            network.connectionsColor = EditorGUILayout.ColorField("Waypoints Connections Color", network.connectionsColor);
            network.namedConnectionsColor = EditorGUILayout.ColorField("Named Waypoints Connections Color", network.namedConnectionsColor);
            network.disabledConnectionsColor = EditorGUILayout.ColorField("Disabled Waypoints Connections Color", network.disabledConnectionsColor);

            if (GUI.changed)
            {
                EditorUtility.SetDirty(network);
            }
        }

        private void OnSceneGUI()
        {
            if (SceneView.currentDrawingSceneView != null)
            {
                Handles.CubeCap(0, SceneView.currentDrawingSceneView.pivot, Quaternion.identity, 0.1f);
            }
        }

        private void DeleteAllConnections()
        {
            var conns = network.GetComponentsInChildren<WaypointsConnection>();

            foreach (var conn in conns)
            {
                DestroyImmediate(conn.gameObject);
            }

            network.connections = null;
        }

        private void DeleteAllWaypoints()
        {
            var wps = network.GetComponentsInChildren<Waypoint>();

            foreach (var wp in wps)
            {
                DestroyImmediate(wp.gameObject);
            }

            network.waypoints = null;
        }

        private void CreateWaypoint()
        {
            var position = Vector3.zero;

            if (SceneView.currentDrawingSceneView != null)
            {
                position = SceneView.currentDrawingSceneView.pivot;
            }

            var wpGo = new GameObject("Waypoint");
            wpGo.transform.position = position;
            wpGo.transform.parent = GetWaypointsParent();

            var wp = wpGo.AddComponent<Waypoint>();
            wp.parentNetwork = network;

            if (network.waypoints == null)
            {
                network.waypoints = new List<Waypoint>();
            }

            network.waypoints.Add(wp);
        }

        private Transform GetWaypointsParent()
        {
            return network.transform.GetOrCreateChild("Waypoints");
        }
    }
}
