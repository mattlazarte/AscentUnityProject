using Ascent.Managers.Game;
using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

namespace Ascent.PlayerShip
{
    [RequireComponent(typeof(Rigidbody))]
    public class PilotCameraController : MonoBehaviour
    {
        public Camera pilotCamera;
        public float cameraTiltLerpVelocity = 5f;
        public float maxCameraTiltAngle = 10f;

        // UpdateCameraTilt will consider the angular velocity to be at 100% at this value. Anything beyond is still consiered to be 100%.
        private float angularFullVelocityAt = 3f;
        private Vector3 currentCameraTilt = Vector3.zero;
        private Rigidbody rb;

        public void Start()
        {
            if (pilotCamera == null)
                throw new Exception("pilotCamera is null.");

            rb = GetComponent<Rigidbody>();
        }

        public void FixedUpdate()
        {
            if (!PauseManager.instance.paused)
            {
                var angularVelocityPercent = new Vector3(
                    (Mathf.Clamp(rb.angularVelocity.x, -angularFullVelocityAt, angularFullVelocityAt) * 100f) / angularFullVelocityAt,
                    (Mathf.Clamp(rb.angularVelocity.y, -angularFullVelocityAt, angularFullVelocityAt) * 100f) / angularFullVelocityAt,
                    (Mathf.Clamp(rb.angularVelocity.z, -angularFullVelocityAt, angularFullVelocityAt) * 100f) / angularFullVelocityAt
                    );

                var targetCameraTilt = new Vector3(
                    ((angularVelocityPercent.x * maxCameraTiltAngle) / 100f),
                    ((angularVelocityPercent.y * maxCameraTiltAngle) / 100f),
                    ((angularVelocityPercent.z * maxCameraTiltAngle) / 100f)
                    );

                targetCameraTilt *= -1;

                currentCameraTilt = Vector3.Lerp(currentCameraTilt, targetCameraTilt, Time.fixedDeltaTime * cameraTiltLerpVelocity);
                pilotCamera.transform.localRotation = Quaternion.identity * Quaternion.Euler(currentCameraTilt);
            }
        }
    }
}