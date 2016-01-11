using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    public class RuntimeWaypointsConnection
    {
        public RuntimeWaypointsConnection(
            RuntimeWaypoint waypoint1, 
            RuntimeWaypoint waypoint2, 
            float cost,
            bool enabled,
            string name)
        {
            this.Waypoint1 = waypoint1;
            this.Waypoint2 = waypoint2;
            this.cost = cost;
            this.enabled = enabled;
            this.name = name;
        }

        public RuntimeWaypoint Waypoint1;
        public RuntimeWaypoint Waypoint2;
        public float cost;
        public bool enabled;
        public string name;
    }
}
