using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Ascent.WaypointsSystem
{
    public class RuntimeWaypoint
    {
        public RuntimeWaypoint(Vector3 position)
        {
            this.position = position;
        }

        public Vector3 position;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(RuntimeWaypoint)) return false;

            return ((RuntimeWaypoint)obj).position == position;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}
