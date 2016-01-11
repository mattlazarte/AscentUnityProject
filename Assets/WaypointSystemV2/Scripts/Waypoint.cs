using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    [Serializable]
    [ExecuteInEditMode]
    public class Waypoint : MonoBehaviour
    {
        public WaypointNetworkV2 parentNetwork;

        private void OnDrawGizmos()
        {
            if (parentNetwork.displayWaypoints)
            {
                Gizmos.color = parentNetwork.waypointsColor;
                Gizmos.DrawWireSphere(transform.position, parentNetwork.waypointsRadius);
            }
        }

        private void OnDestroy()
        {
            if (!Application.isPlaying && parentNetwork.waypoints.Contains(this))
            {
                if (parentNetwork.connections != null)
                {
                    for (var i = parentNetwork.connections.Count - 1; i >= 0; i--)
                    {
                        var conn = parentNetwork.connections[i]; 

                        if (conn.wp01 == this || conn.wp02 == this)
                        {
                            if (parentNetwork.connections.Contains(conn))
                            {
                                parentNetwork.connections.Remove(conn);
                            }

                            DestroyImmediate(conn.gameObject);
                        }
                    }
                }

                parentNetwork.waypoints.Remove(this);
            }
        }
    }
}
