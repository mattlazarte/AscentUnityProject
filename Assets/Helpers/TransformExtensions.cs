using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Ascent
{
    public static class TransformExtensions
    {
        public static Transform GetOrCreateChild(this Transform transform, string name)
        {
            var obj = transform.Find(name);

            if (obj == null)
            {
                obj = new GameObject(name).transform;
                obj.parent = transform;
                obj.localPosition = Vector3.zero;
            }

            return obj;
        }
    }
}