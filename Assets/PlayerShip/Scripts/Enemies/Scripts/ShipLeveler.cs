using UnityEngine;
using System.Collections;
using Ascent.Managers.Game;
using System;

namespace Ascent.Enemies
{
    public class ShipLeveler : MonoBehaviour
    {
        public AutoPilotedShip ship;
    
        private void Start()
        {
            if (ship == null)
                throw new Exception("ship is null.");
        }
    
        private void FixedUpdate()
        {
            transform.position = ship.transform.position;
        }
    }
}
