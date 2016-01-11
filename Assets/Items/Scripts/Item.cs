using UnityEngine;
using System.Collections;
using Ascent.PlayerShip;
using System;
using Ascent.Managers.Game;

namespace Ascent.Items
{
    public abstract class Item : MonoBehaviour
    {
        public Color colorOnRadar;
        protected Rigidbody myRigidbody;

        // Pause
        protected Vector3 savedVelocityBeforePause;
        protected Vector3 savedAngularVelocityBeforePause;

        protected virtual void Start()
        {
            myRigidbody = GetComponent<Rigidbody>();
            if (myRigidbody == null)
                throw new Exception("myRigidbody is null.");

            PauseManager.OnPause += Pause;
            PauseManager.OnUnpause += Unpause;
        }
        protected virtual void OnDestroy()
        {
            PauseManager.OnPause -= Pause;
            PauseManager.OnUnpause -= Unpause;
        }

        public void Pause()
        {
            savedVelocityBeforePause = myRigidbody.velocity;
            savedAngularVelocityBeforePause = myRigidbody.angularVelocity;
            myRigidbody.isKinematic = true;
            myRigidbody.AddForce(Vector3.zero, ForceMode.VelocityChange);
            myRigidbody.AddTorque(Vector3.zero, ForceMode.VelocityChange);
        }

        public void Unpause(float pauseDeltaTime)
        {
            myRigidbody.isKinematic = false;
            myRigidbody.AddForce(savedVelocityBeforePause, ForceMode.VelocityChange);
            myRigidbody.AddTorque(savedAngularVelocityBeforePause, ForceMode.VelocityChange);
        }

        public abstract bool OnCatchInternal(PlayerShipWeaponryController playerShipWeaponryController);
        public void OnCatch(PlayerShipWeaponryController playerShipWeaponryController)
        {
            if (OnCatchInternal(playerShipWeaponryController))
            {
                Destroy(gameObject);
            }
        }
    }
}
