using UnityEngine;
using System.Collections;
using Ascent.Managers.Game;
using System;

namespace Ascent.Enemies
{
    [RequireComponent(typeof(Rigidbody))]
    public class AutoPilotedShip : MonoBehaviour
    {
        [Header("Auto Pilot Settings")]
        public ShipLeveler leveler;
        public float rotationVelocity = 1f;
        public float thrusterForce = .5f;
		public bool debugAutoPilot = true;
		public Color debugAutoPilotColor = Color.white;

		protected float pilotDistanceToStop = .2f;
        protected Vector3? pilotTargetPosition;
        protected Rigidbody myRigidbody;
        protected bool autoLookToPilotTargetPosition = true;

        protected virtual void Awake()
        {
            if (leveler == null)
                throw new Exception("leveler is null.");

            myRigidbody = GetComponent<Rigidbody>();

            pilotTargetPosition = null;
        }

        protected virtual void Update()
        {
            if (pilotTargetPosition.HasValue)
			{
				var directionVector = pilotTargetPosition.Value - transform.position;

                if (autoLookToPilotTargetPosition)
                    LookToDirection(directionVector);

                if (directionVector.magnitude > pilotDistanceToStop)
                {
                    var forceTowardDestination = directionVector.normalized * thrusterForce;
                    myRigidbody.AddForce(forceTowardDestination);
                }
            }
        }

        protected void LookToPosition(Vector3 position)
        {
            var directionVector = position - transform.position;
            LookToDirection(directionVector);
        }

        protected void LookToDirection(Vector3 directionVector)
        {
            var desiredRotation = Quaternion.LookRotation(directionVector);
            var lerpedRotation = Quaternion.Lerp(leveler.transform.rotation, desiredRotation, Time.deltaTime * rotationVelocity);
            leveler.transform.rotation = lerpedRotation;
        }

		protected virtual void OnDrawGizmos()
		{
			if (debugAutoPilot && Application.isPlaying && pilotTargetPosition.HasValue)
			{
				Gizmos.color = debugAutoPilotColor;
				Gizmos.DrawLine(transform.position, pilotTargetPosition.Value);
			}
		}
    }
}
