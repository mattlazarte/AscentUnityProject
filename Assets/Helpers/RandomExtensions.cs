using UnityEngine;

namespace Ascent
{
    public static class RandomExtensions
    {
        public static Vector3 vector3
        {
            get
            {
                var rndRotation = Random.rotation;
                return new Vector3(rndRotation.x, rndRotation.y, rndRotation.z);
            }
        }
    }
}
