using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    public class PathFindingResult
    {
        public PathFindingResult(
            bool success,
            RuntimeWaypoint[] path = null,
            Dictionary<RuntimeWaypoint, RuntimeWaypoint> cameFrom = null,
            string msg = null
        )
        {
            this.success = success;
            this.path = path;
            this.cameFrom = cameFrom;
            this.msg = msg;
        }
    
        public bool success;
        public RuntimeWaypoint[] path;
        public Dictionary<RuntimeWaypoint, RuntimeWaypoint> cameFrom;
        public string msg;
    }
}
