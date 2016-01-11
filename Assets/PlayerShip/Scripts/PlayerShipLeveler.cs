using System;
using UnityEngine;

namespace Ascent.PlayerShip
{
    public class PlayerShipLeveler : MonoBehaviour
    {
        public Transform ship;
        public SpringJoint topSpring;
        public SpringJoint bottomSpring;

        private Vector3 offsetToTarget;

        public void Start()
        {
            if (ship == null)
                throw new Exception("ship is null.");

            if (topSpring == null)
                throw new Exception("topSpring is null.");

            if (bottomSpring == null)
                throw new Exception("bottomSpring is null.");

            offsetToTarget = transform.position - ship.position;
        }

        private void FixedUpdate()
        {
            transform.position = ship.position + offsetToTarget;
        }
    }
}