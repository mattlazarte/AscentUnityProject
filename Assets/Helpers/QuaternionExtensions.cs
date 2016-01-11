using UnityEngine;

namespace Ascent
{
    public static class QuaternionExtensions
    {
        public static PitchYawRoll GetPitchYawRoll(this Quaternion q)
        {
            var pitch = Mathf.Atan2(2 * q.x * q.w - 2 * q.y * q.z, 1 - 2 * q.x * q.x - 2 * q.z * q.z) * Mathf.Rad2Deg;
            if (pitch < 0) pitch += 360;

            var yaw = Mathf.Atan2(2 * q.y * q.w - 2 * q.x * q.z, 1 - 2 * q.y * q.y - 2 * q.z * q.z) * Mathf.Rad2Deg;
            if (yaw < 0) yaw += 360;

            var roll = Mathf.Asin(2 * q.x * q.y + 2 * q.z * q.w) * Mathf.Rad2Deg;
            if (roll < 0) roll += 360;

            return new PitchYawRoll(pitch, yaw, roll);
        }
    }

    public class PitchYawRoll
    {
        public PitchYawRoll(float pitch, float yaw, float roll)
        {
            this.Pitch = pitch;
            this.Yaw = yaw;
            this.Roll = roll;
        }

        public float Pitch;
        public float Yaw;
        public float Roll;
    }
}