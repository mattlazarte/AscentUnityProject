using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    [Serializable]
    public class WaypointNetworkV2 : MonoBehaviour
    {
        public class WaypointNeighbor
        {
            public WaypointNeighbor(RuntimeWaypoint waypoint, RuntimeWaypointsConnection connection)
            {
                this.waypoint = waypoint;
                this.connection = connection;
            }

            public RuntimeWaypoint waypoint;
            public RuntimeWaypointsConnection connection;
        }

        [SerializeField]
        public List<Waypoint> waypoints;

        [SerializeField]
        public List<WaypointsConnection> connections;

        [SerializeField]
        public bool displayWaypoints = true;

        [SerializeField]
        public bool displayConnections = true;

        [SerializeField]
        public float waypointsRadius = 0.3f;

        [SerializeField]
        public Color waypointsColor = Color.white;

        [SerializeField]
        public Color connectionsColor = Color.gray;

        [SerializeField]
        public Color namedConnectionsColor = Color.yellow;

        [SerializeField]
        public Color disabledConnectionsColor = Color.magenta;

        [NonSerialized]
        public List<RuntimeWaypoint> runtimeWaypoints;

        [NonSerialized]
        public List<RuntimeWaypointsConnection> runtimeConnections;

        [NonSerialized]
        public Dictionary<RuntimeWaypoint, List<WaypointNeighbor>> neighborsIndex;

        [NonSerialized]
        public Dictionary<string, RuntimeWaypointsConnection> namedConnectionsIndex;

        [NonSerialized]
        public bool isReady;

        [NonSerialized]
        public bool connectionToggledLastFrame;

        private void Awake()
        {
            isReady = false;
            InitRuntimeInstances();
            BuildNamedConnectionIndex();
            BuildNeighborsIndex();
            isReady = true;
        }

        private void LateUpdate()
        {
            connectionToggledLastFrame = false;
        }

        private void InitRuntimeInstances()
        {
            runtimeWaypoints = new List<RuntimeWaypoint>();
            runtimeConnections = new List<RuntimeWaypointsConnection>();
            var waypointsMapping = new Dictionary<Waypoint, RuntimeWaypoint>();

            foreach (var wp in waypoints)
            {
                var newRuntimeWaypoint = new RuntimeWaypoint(wp.transform.position);
                waypointsMapping.Add(wp, newRuntimeWaypoint);
                runtimeWaypoints.Add(newRuntimeWaypoint);
            }

            foreach (var conn in connections)
            {
                runtimeConnections.Add(
                    new RuntimeWaypointsConnection(
                        waypointsMapping[conn.wp01],
                        waypointsMapping[conn.wp02],
                        conn.cost,
                        conn.enabled,
                        conn.name
                    )
                );
                Destroy(conn.gameObject);
            }

            foreach (var wp in waypoints)
            {
                Destroy(wp.gameObject);
            }

            connections = null;
            waypoints = null;
        }

        private void BuildNamedConnectionIndex()
        {
            namedConnectionsIndex = new Dictionary<string, RuntimeWaypointsConnection>();

            if (runtimeConnections != null)
            {
                foreach (var conn in runtimeConnections)
                {
                    if (!string.IsNullOrEmpty(conn.name))
                    {
                        if (namedConnectionsIndex.ContainsKey(conn.name))
                        {
                            throw new Exception("The name '" + conn.name + "' is assigned to more than one connection.");
                        }

                        namedConnectionsIndex.Add(conn.name, conn);
                    }
                }
            }
        }

        private void BuildNeighborsIndex()
        {
            neighborsIndex = new Dictionary<RuntimeWaypoint, List<WaypointNeighbor>>();

            if (runtimeConnections != null)
            {
                foreach (var conn in runtimeConnections)
                {
                    if (!neighborsIndex.ContainsKey(conn.Waypoint1))
                    {
                        neighborsIndex.Add(conn.Waypoint1, new List<WaypointNeighbor>() { 
                            new WaypointNeighbor(conn.Waypoint2, conn)
                        });
                    }
                    else
                    {
                        neighborsIndex[conn.Waypoint1].Add(new WaypointNeighbor(conn.Waypoint2, conn));
                    }

                    if (!neighborsIndex.ContainsKey(conn.Waypoint2))
                    {
                        neighborsIndex.Add(conn.Waypoint2, new List<WaypointNeighbor>() { 
                            new WaypointNeighbor(conn.Waypoint1, conn)
                        });
                    }
                    else
                    {
                        neighborsIndex[conn.Waypoint2].Add(new WaypointNeighbor(conn.Waypoint1, conn));
                    }
                }
            }
        }

        public RuntimeWaypoint[] GetWaypointsOrderedByProximity(Vector3 position)
        {
            if (runtimeWaypoints == null || runtimeWaypoints.Count == 0)
            {
                return null;
            }

            if (runtimeWaypoints.Count == 1)
            {
                return new RuntimeWaypoint[] { runtimeWaypoints[0] };
            }

            var arr = runtimeWaypoints.ToArray();
            Array.Sort<RuntimeWaypoint>(arr, (w1, w2) =>
            {
                if (w1 == null || w2 == null)
                    return 0;

                var w1SqrMag = (w1.position - position).sqrMagnitude;
                var w2SqrMag = (w2.position - position).sqrMagnitude;

                if (w1SqrMag == w2SqrMag) return 0;
                else if (w1SqrMag > w2SqrMag) return 1;
                else return -1;
            });

            return arr;
        }

        //public RuntimeWaypoint FindNearestWaypoint(Vector3 position)
        //{
        //    if (runtimeWaypoints == null || runtimeWaypoints.Count == 0)
        //    {
        //        return null;
        //    }

        //    if (runtimeWaypoints.Count == 1)
        //    {
        //        return runtimeWaypoints[0];
        //    }

        //    var nearestWaypoint = runtimeWaypoints[0];
        //    var nearestSqrMagnitude = (nearestWaypoint.position - position).sqrMagnitude;

        //    for (var i = 1; i < runtimeWaypoints.Count; i++)
        //    {
        //        var currWaypoint = runtimeWaypoints[i];
        //        var currSqrMagnitude = (currWaypoint.position - position).sqrMagnitude;

        //        if (currSqrMagnitude < nearestSqrMagnitude)
        //        {
        //            nearestSqrMagnitude = currSqrMagnitude;
        //            nearestWaypoint = currWaypoint;
        //        }
        //    }

        //    return nearestWaypoint;
        //}

        public List<WaypointNeighbor> GetNeighborWaypoints(RuntimeWaypoint waypoint)
        {
            if (neighborsIndex.ContainsKey(waypoint))
            {
                return neighborsIndex[waypoint];
            }

            return new List<WaypointNeighbor>();
        }

        public PathFindingResult FindPath(RuntimeWaypoint origin, RuntimeWaypoint destination)
        {
            var frontier = new PriorityQueue();
            frontier.Enqueue(destination, 0);

            var cameFrom = new Dictionary<RuntimeWaypoint, RuntimeWaypoint>();
            cameFrom.Add(destination, null);

            var costSoFar = new Dictionary<RuntimeWaypoint, float>();
            costSoFar.Add(destination, 0);

            var frontierLoopCount = 0;
            while (frontier.IsNotEmpty)
            {
                if (++frontierLoopCount > 1000)
                {
                    return new PathFindingResult(false, msg: "Loops overflow.");
                }

                var current = frontier.Dequeue();

                if (current.Equals(origin))
                {
                    break;
                }

                foreach (var neighbor in GetNeighborWaypoints(current))
                {
                    if (!neighbor.connection.enabled) continue;

                    var newCost = costSoFar[current] + neighbor.connection.cost;
                    if (!costSoFar.ContainsKey(neighbor.waypoint) || newCost < costSoFar[neighbor.waypoint])
                    {
                        if (costSoFar.ContainsKey(neighbor.waypoint))
                        {
                            costSoFar[neighbor.waypoint] = newCost;
                        }
                        else
                        {
                            costSoFar.Add(neighbor.waypoint, newCost);
                        }

                        var priority =
                            Math.Abs(origin.position.x - neighbor.waypoint.position.x) +
                            Math.Abs(origin.position.y - neighbor.waypoint.position.y) +
                            Math.Abs(origin.position.z - neighbor.waypoint.position.z) +
                            newCost;

                        frontier.Enqueue(neighbor.waypoint, priority);

                        if (cameFrom.ContainsKey(neighbor.waypoint))
                        {
                            cameFrom[neighbor.waypoint] = current;
                        }
                        else
                        {
                            cameFrom.Add(neighbor.waypoint, current);
                        }
                    }
                }
            }

            var currentStep = origin;
            var path = new List<RuntimeWaypoint>();
            path.Add(currentStep);
            var loopCount = 0;
            while (!currentStep.Equals(destination))
            {
                if (++loopCount > runtimeConnections.Count)
                {
                    return new PathFindingResult(false, msg: "More loops than connections. Path finding aborted.");
                }

                if (!cameFrom.ContainsKey(currentStep))
                {
                    return new PathFindingResult(false, msg: "Impossible path.");
                }

                currentStep = cameFrom[currentStep];
                path.Add(currentStep);
            }

            return new PathFindingResult(true, path.ToArray(), cameFrom);
        }

        public void SetNamedConnectionEnabled(string name, bool value)
        {
            if (namedConnectionsIndex.ContainsKey(name))
            {
                namedConnectionsIndex[name].enabled = value;
                connectionToggledLastFrame = true;
            }
        }

        public bool? GetNamedConnectionEnabled(string name)
        {
            if (!namedConnectionsIndex.ContainsKey(name))
                return null;

            return namedConnectionsIndex[name].enabled;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying && isReady)
            {
                if (displayWaypoints && runtimeWaypoints != null && runtimeWaypoints.Count > 0)
                {
                    Gizmos.color = waypointsColor;

                    foreach (var wp in runtimeWaypoints)
                        Gizmos.DrawWireSphere(wp.position, waypointsRadius);
                }

                if (displayConnections && runtimeConnections != null && runtimeConnections.Count > 0)
                {
                    foreach (var conn in runtimeConnections)
                    {
                        Gizmos.color = conn.enabled ? connectionsColor : disabledConnectionsColor;

                        if (!string.IsNullOrEmpty(conn.name))
                        {
                            Gizmos.color = Color.Lerp(Gizmos.color, namedConnectionsColor, .5f);
                        }

                        Gizmos.DrawLine(conn.Waypoint1.position, conn.Waypoint2.position);
                    }
                }
            }
        }
    }
}