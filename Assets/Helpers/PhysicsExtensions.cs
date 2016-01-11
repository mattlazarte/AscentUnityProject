using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ascent
{
    public static class PhysicsExtensions
    {
        public static bool CheckSightBetweenTwoPositions(Vector3 origin, Vector3 target, LayerMask layersToIgnore)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, target - origin, out hit, float.MaxValue, layersToIgnore))
            {
                var sqrMagnitudeToTarget = (target - origin).sqrMagnitude;
                var sqrMagnitudeToHit = (hit.point - origin).sqrMagnitude;

                // If the hit point is closer than the position that we want to see, there is something in between.
                if (sqrMagnitudeToHit < sqrMagnitudeToTarget)
                {
                    return false;
                }

                // If the hit point is at target or beyond, there is nothing in between.
                return true;
            }

            // if nothing was hit, there is nothing in between, nor after.
            return true;
        }

        public static bool IsColliderInSight(Vector3 origin, Collider collider)
        {
            RaycastHit hit;
            var ray = new Ray(origin, collider.transform.position - origin);
            if (Physics.Raycast(ray, out hit, float.MaxValue))
            {
                if (hit.collider == collider)
                {
                    return true;
                }
                else
                {
                    //Debug.LogFormat("IsInSight false. Looking for {0}, found {1}.", collider.name, hit.collider.name); 
                    return false;
                }
            }

            //Debug.LogFormat("IsInSight false. Raycast return nothing."); 
            return false;
        }

        public static bool IsColliderInSight(Vector3 origin, Collider collider, LayerMask layermask)
        {
            RaycastHit hit;
            var ray = new Ray(origin, collider.transform.position - origin);
            if (Physics.Raycast(ray, out hit, float.MaxValue, layermask))
            {
                if (hit.collider == collider)
                {
                    return true;
                }
                else
                {
                    //Debug.LogFormat("IsInSight false. Looking for {0}, found {1}.", collider.name, hit.collider.name); 
                    return false;
                }
            }

            //Debug.LogFormat("IsInSight false. Raycast return nothing."); 
            return false;
        }

        public static RaycastHit[] RaycastCilinderWithEightRays(
            Vector3 origin, Vector3 destination, float radius, int? layerMask = null, 
            bool debugHits = false, bool debugRays = false
            )
        {
            var direction = destination - origin;
            var rotation = Quaternion.LookRotation(direction);

            var origins = new Vector3[8];
            origins[0] = rotation * (Vector3.up * radius) + origin;
            origins[1] = rotation * (Vector3.down * radius) + origin;
            origins[2] = rotation * (Vector3.left * radius) + origin;
            origins[3] = rotation * (Vector3.right * radius) + origin;
            origins[4] = rotation * ((Vector3.up + Vector3.right).normalized * radius) + origin;
            origins[5] = rotation * ((Vector3.up + Vector3.left).normalized * radius) + origin;
            origins[6] = rotation * ((Vector3.down + Vector3.right).normalized * radius) + origin;
            origins[7] = rotation * ((Vector3.down + Vector3.left).normalized * radius) + origin;

            var hits = new List<RaycastHit>();
            var hitsOrigins = new List<Vector3>();
            for (var i = 0; i < 8; i++)
            {
                RaycastHit hit;

                if ((layerMask.HasValue && Physics.Raycast(origins[i], direction, out hit, float.MaxValue, layerMask.Value)) ||
                    Physics.Raycast(origins[i], direction, out hit, float.MaxValue))
                {
                    hitsOrigins.Add(origins[i]);
                    hits.Add(hit);
                }

                if (debugRays)
                {
                    Debug.DrawRay(origins[i], direction, new Color(.5f, .5f, .5f, .5f));
                }
            }

            if (debugHits && hits.Count > 0)
            {
                for (var i = 0; i < hits.Count; i++)
                {
                    Debug.DrawLine(hitsOrigins[i], hits[i].point, new Color(1f, 1f, 1f, 0.5f));
                }
            }

            return hits.ToArray();
        }
    }
}
