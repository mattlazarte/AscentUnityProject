//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;

//namespace Ascent.WaypointsSystem
//{
//    public class PathFindTester : MonoBehaviour
//    {
//        public WaypointNetworkV2 waypoints;
//        public Transform target;
//        public string connNameToControl;
//        public bool enableConn = true;

//        private RuntimeWaypoint nearestWaypoint;
//        private RuntimeWaypoint targetNearestWaypoint;
//        private List<WaypointNetworkV2.WaypointNeighbor> neighbors;
//        private PathFindingResult pathFindingResult;

//        private void Start()
//        {
//            if (waypoints == null)
//            {
//                throw new Exception("waypoints is null.");
//            }

//            StartCoroutine(ProcessCalls());
//        }

//        private void Update()
//        {
//            if (!string.IsNullOrEmpty(connNameToControl))
//            {
//                waypoints.SetNamedConnectionEnabled(connNameToControl, enableConn);
//            }
//        }

//        private IEnumerator ProcessCalls()
//        {
//            while (true)
//            {
//                if (waypoints.isReady)
//                {
//                    nearestWaypoint = null;
//                    neighbors = null;
//                    pathFindingResult = null;
//                    targetNearestWaypoint = null;

//                    nearestWaypoint = waypoints.FindNearestWaypoint(transform.position);
//                    if (nearestWaypoint != null)
//                    {
//                        neighbors = waypoints.GetNeighborWaypoints(nearestWaypoint);

//                        if (target != null)
//                        {
//                            targetNearestWaypoint = waypoints.FindNearestWaypoint(target.position);

//                            if (targetNearestWaypoint != null)
//                            {
//                                pathFindingResult = waypoints.FindPath(nearestWaypoint, targetNearestWaypoint);
//                            }
//                        }
//                    }
//                }

//                if (pathFindingResult == null)
//                {
//                    Debug.Log("pathFindingResult = null");
//                }
//                else
//                {
//                    if (!pathFindingResult.success)
//                    {
//                        Debug.Log("pathFindingResult.success = false. msg = " + pathFindingResult.msg);
//                    }
//                    else
//                    {
//                        Debug.Log("pathFindingResult.success = true. count = " + pathFindingResult.path.Length);
//                    }
//                }

//                yield return new WaitForSeconds(1f);
//            }
//        }

//        private void OnDrawGizmos()
//        {
//            if (Application.isPlaying)
//            {
//                if (neighbors != null)
//                {
//                    Gizmos.color = new Color(0, 1, 0, 0.2f);
//                    foreach (var neighbor in neighbors)
//                    {
//                        Gizmos.DrawWireSphere(neighbor.waypoint.position, 0.4f);
//                    }
//                }

//                if (nearestWaypoint != null)
//                {
//                    if (nearestWaypoint != null)
//                    {
//                        Gizmos.color = Color.blue;
//                        Gizmos.DrawWireSphere(nearestWaypoint.position, 0.4f);
//                    }
//                }

//                if (targetNearestWaypoint != null)
//                {
//                    if (targetNearestWaypoint != null)
//                    {
//                        Gizmos.color = Color.red;
//                        Gizmos.DrawWireSphere(targetNearestWaypoint.position, 0.4f);
//                    }
//                }

//                if (pathFindingResult != null)
//                {
//                    if (pathFindingResult.success)
//                    {
//                        if (pathFindingResult.cameFrom != null)
//                        {
//                            Gizmos.color = new Color(1, 0, 0, 0.2f);
//                            foreach (var pair in pathFindingResult.cameFrom)
//                            {
//                                Gizmos.DrawWireSphere(pair.Key.position, 0.4f);

//                                if (pair.Value != null)
//                                {
//                                    Gizmos.DrawWireSphere(pair.Value.position, 0.4f);
//                                    Gizmos.DrawLine(pair.Key.position, pair.Value.position);
//                                }
//                            }
//                        }

//                        if (pathFindingResult.path != null)
//                        {
//                            Gizmos.color = Color.red;
//                            RuntimeWaypoint lastWaypoint = null;
//                            foreach (var wp in pathFindingResult.path)
//                            {
//                                Gizmos.DrawWireSphere(wp.position, 0.5f);

//                                if (lastWaypoint != null)
//                                {
//                                    Gizmos.DrawLine(lastWaypoint.position, wp.position);
//                                }

//                                lastWaypoint = wp;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//    }
//}
