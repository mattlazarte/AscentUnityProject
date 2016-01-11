using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    [CustomEditor(typeof(Waypoint))]
    [CanEditMultipleObjects]
    public class WaypointEditor : Editor
    {
        private Waypoint wp;
        private Waypoint[] wps;
        private WaypointsConnection currentConn;
        private WaypointNetworkV2 parentNetwork;
        private bool multipleSelection;

        public void OnEnable()
        {
            multipleSelection = !(targets == null || targets.Length == 1);
            
            wp = (Waypoint)target;
            parentNetwork = wp.parentNetwork;

            wps = new Waypoint[targets.Length];
            for (var i = 0; i < targets.Length; i++)
            {
                wps[i] = (Waypoint)targets[i];
            }

            if (multipleSelection && targets.Length == 2)
            {
                currentConn = GetConnectionBetweenSelectedWaypoints();
            }
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Select Network"))
            {
                Selection.activeGameObject = parentNetwork.gameObject;
            }

            if (multipleSelection && targets.Length == 2)
            {
                if (currentConn != null)
                {
                    if (GUILayout.Button("Disconnect Waypoints"))
                    {
                        DisconnectWaypoints();
                    }
                }
                else
                {
                    if (GUILayout.Button("Connect Waypoints"))
                    {
                        ConnectWaypoints();
                    }
                }
            }
        }

        private WaypointsConnection GetConnectionBetweenSelectedWaypoints()
        {
            if (parentNetwork.connections == null) return null;

            foreach (var conn in parentNetwork.connections)
            {
                if ((conn.wp01 == wps[0] && conn.wp02 == wps[1]) ||
                    (conn.wp01 == wps[1] && conn.wp02 == wps[0]))
                {
                    return conn;
                }
            }
            return null;
        }

        private void DisconnectWaypoints()
        {
            DestroyImmediate(currentConn.gameObject);

            if (parentNetwork.connections.Contains(currentConn))
            {
                parentNetwork.connections.Remove(currentConn);
            }

            currentConn = null;
        }

        private void ConnectWaypoints()
        {
            var connGo = new GameObject("Connection");
            connGo.transform.parent = GetConnectionsParent();

            var conn = connGo.AddComponent<WaypointsConnection>();
            conn.parentNetwork = parentNetwork;
            conn.wp01 = wps[0];
            conn.wp02 = wps[1];

            if (parentNetwork.connections == null)
            {
                parentNetwork.connections = new List<WaypointsConnection>();
            }

            parentNetwork.connections.Add(conn);
            currentConn = conn;
        }

        private Transform GetConnectionsParent()
        {
            return parentNetwork.transform.GetOrCreateChild("Connections");
        }
    }
}
