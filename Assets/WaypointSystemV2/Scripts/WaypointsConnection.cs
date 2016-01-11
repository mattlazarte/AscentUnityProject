using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    [Serializable]
    [ExecuteInEditMode]
    public class WaypointsConnection : MonoBehaviour
    {
        public WaypointNetworkV2 parentNetwork;
        public Waypoint wp01;
        public Waypoint wp02;
        public float cost;
        public new bool enabled = true;
        public new string name = null;

        private void OnDrawGizmos()
        {
            if (parentNetwork.displayConnections)
            {
                Gizmos.color =
                    enabled
                    ? parentNetwork.connectionsColor
                    : parentNetwork.disabledConnectionsColor;

                if (!string.IsNullOrEmpty(name))
                {
                    Gizmos.color = Color.Lerp(Gizmos.color, parentNetwork.namedConnectionsColor, .5f);
                }

                Gizmos.DrawLine(wp01.transform.position, wp02.transform.position);
            }
        }

        private void Update()
        {
            transform.position = wp02.transform.position + ((wp01.transform.position - wp02.transform.position) / 2);
            cost = Vector3.Distance(wp01.transform.position, wp02.transform.position);
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying && parentNetwork.connections.Contains(this))
            {
                parentNetwork.connections.Remove(this);
            }
        }
    }
}
