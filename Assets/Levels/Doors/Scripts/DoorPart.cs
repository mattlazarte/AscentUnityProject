using Ascent.Weaponry;
using System;
using UnityEngine;

namespace Ascent
{
    public class DoorPart : MonoBehaviour, IHittableObject
    {
        private Door door;

        private void Start()
        {
            door = transform.parent.GetComponent<Door>();
            if (door == null)
                throw new Exception("Door component not found on parent.");
        }

        public void OnHitByWeapon(float damage, bool shotByPlayer)
        {
            if (shotByPlayer)
            {
                door.Activate();
            }
        }
    }
}