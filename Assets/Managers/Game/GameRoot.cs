using Ascent.Managers.Audio;
using Ascent.UI;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using Ascent.PlayerShip;

namespace Ascent.Managers.Game
{
    public class GameRoot : MonoBehaviour
    {
        public PilotCamera pilotCamera;
        public PlayerShipController playerShipController;
        public PlayerShipWeaponryController playerShipWeaponryController;

        private void Awake()
        {
            if (pilotCamera == null)
                throw new Exception("pilotCamera is null.");

            if (playerShipController == null)
                throw new Exception("playerShipController is null.");

            if (playerShipWeaponryController == null)
                throw new Exception("playerShipWeaponryController is null.");
        }
    }
}
