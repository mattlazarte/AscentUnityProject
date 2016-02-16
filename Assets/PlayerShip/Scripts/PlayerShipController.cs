//using Ascent.Managers.Audio;
using Ascent.Managers.Game;
using Ascent.Managers.Input;
using Ascent.Weaponry;
using System;
using UnityEngine;
using UnityEngine.UI;
using XInputDotNetPure;

namespace Ascent.PlayerShip
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerShipController : MonoBehaviour, IHittableObject
    {
        private float forwardValue = 2;
        private float sideValue = 2;
        private bool isMovingForward = false;
        private bool isMovingSideways = false;

        public PlayerShipLeveler leveler;
        public float strafeForce = 40f;
        public float moveForward = 40f;
        public float moveBackward = 40f;
        public float barrelRollTorque = 10f;
        public float yawTorque = 15f;
        public float pitchTorque = 20f;
        public Light headlights;
        public bool lockControls = true;

        [Header("Joystick Rumble Settings")]
        public float joystickMaxRumble = 1f;
        public float joystickRumblePeriod = 0.07f;

        [Header("Indicators")]
        public Text autoLevelIndicator;
        public Text headlightsIndicator;
        public Color indicatorEnabledColor;
        public Color indicatorDisabledColor;

        private Rigidbody rb;
        private float joystickRumbleLastActivated = 0;
        private PlayerShipWeaponryController weaponryController;

        // Pause
        protected Vector3 savedVelocityBeforePause;
        protected Vector3 savedAngularVelocityBeforePause;

        public void Start()
        {
            AudioManager.instance.Play(AudioBank.SFX_SHIP_IDLE, this.gameObject);
            if (leveler == null)
                throw new Exception("leveler is null.");

            if (headlights == null)
                throw new Exception("headLights is null.");

            if (autoLevelIndicator == null)
                throw new Exception("autoLevelIndicator is null.");

            if (headlightsIndicator == null)
                throw new Exception("headlightsIndicator is null.");

            weaponryController = GetComponent<PlayerShipWeaponryController>();
            if (weaponryController == null)
                throw new Exception("weaponryController is null.");

            rb = GetComponent<Rigidbody>();

            PauseManager.OnPause += Pause;
            PauseManager.OnUnpause += Unpause;

            // Controls should be unlocked by the game manager.
            lockControls = true;
        }
        protected virtual void OnDestroy()
        {
            PauseManager.OnPause -= Pause;
            PauseManager.OnUnpause -= Unpause;
        }

        public void Pause()
        {
            savedVelocityBeforePause = rb.velocity;
            savedAngularVelocityBeforePause = rb.angularVelocity;
            rb.isKinematic = true;
            rb.AddForce(Vector3.zero, ForceMode.VelocityChange);
            rb.AddTorque(Vector3.zero, ForceMode.VelocityChange);
            leveler.gameObject.SetActive(false);
        }

        public void Unpause(float pauseDeltaTime)
        {
            rb.isKinematic = false;
            rb.AddForce(savedVelocityBeforePause, ForceMode.VelocityChange);
            rb.AddTorque(savedAngularVelocityBeforePause, ForceMode.VelocityChange);
            leveler.gameObject.SetActive(true);
        }

        public void Update()
        {
            if (!PauseManager.instance.paused && !lockControls)
            {
                var input = InputManager.instance.GetPlayerInput();
                UpdateAutoLevel(input);
                UpdateHeadlights(input);
                UpdateJoystickRumbleState();
            }
        }

        private void UpdateAutoLevel(PlayerInput input)
        {
            if (input.ToggleAutoLevel)
            {
                //SoundFxsManager.instance.PlayOneShot(SoundFx.UIBlip01, transform.position);
                leveler.gameObject.SetActive(!leveler.gameObject.activeSelf);
                UIUtils.BlinkUI(autoLevelIndicator);
            }

            if (leveler.gameObject.activeSelf)
            {
                autoLevelIndicator.color = indicatorEnabledColor;
                autoLevelIndicator.text = "AUTO LEVEL ENABLED";
            }
            else
            {
                autoLevelIndicator.color = indicatorDisabledColor;
                autoLevelIndicator.text = "AUTO LEVEL DISABLED";
            }
        }

        private void UpdateHeadlights(PlayerInput input)
        {
            if (input.ToggleHeadlights)
            {
                //SoundFxsManager.instance.PlayOneShot(SoundFx.UIBlip01, transform.position);
                headlights.gameObject.SetActive(!headlights.gameObject.activeSelf);
                UIUtils.BlinkUI(headlightsIndicator);
            }

            if (headlights.gameObject.activeSelf)
            {
                headlightsIndicator.color = indicatorEnabledColor;
                headlightsIndicator.text = "HEADLIGHTS ENABLED";
            }
            else
            {
                headlightsIndicator.color = indicatorDisabledColor;
                headlightsIndicator.text = "HEADLIGHTS DISABLED";
            }
        }

        public void FixedUpdate()
        {
            if (!PauseManager.instance.paused && !lockControls)
            {
                var input = InputManager.instance.GetPlayerInput();
                UpdateMovement(input);
            }
        }

        private void UpdateMovement(PlayerInput input)
        {
            rb.AddRelativeForce(0, 0, input.MoveForward * moveForward);
            rb.AddRelativeForce(input.StrafeRight * strafeForce, 0, 0);
            rb.AddRelativeForce(0, input.StrafeUp * strafeForce, 0);
            rb.AddRelativeTorque(0, 0, input.RollLeft * barrelRollTorque);
            rb.AddRelativeTorque(0, input.YawDelta * yawTorque, 0);
            rb.AddRelativeTorque(input.PitchDelta * pitchTorque, 0, 0);

            if(input.MoveForward != 0)
            {
                if (!isMovingForward)
                     AudioManager.instance.Play(AudioBank.SFX_SHIP_FORBACK, this.gameObject);
					

                isMovingForward = true;
                forwardValue += input.MoveForward;
                forwardValue = Mathf.Clamp(forwardValue, 1, 3);
                AkSoundEngine.SetRTPCValue("Pitch", forwardValue);
            }
		
            if (input.StrafeRight != 0)
            {
                if (!isMovingSideways)
                    AudioManager.instance.Play(AudioBank.SFX_SHIP_SIDE, this.gameObject);
					//AudioManager.instance.Play(AudioBank.SFX_SHIP_BURST, this.gameObject);
					//AudioManager.instance.Play(AudioBank.SFX_SHIP_SWAY, this.gameObject);

                isMovingSideways = true;
                sideValue += input.StrafeRight;
                sideValue = Mathf.Clamp(sideValue, 1, 3);
                AkSoundEngine.SetRTPCValue("Roll", sideValue);
            }

            if (input.StrafeRight == 0 && input.MoveForward == 0 && (isMovingForward || isMovingSideways))
            {
                isMovingSideways = false;
                isMovingForward = false;
                forwardValue = 50;
                sideValue = 50;
                AudioManager.instance.Play(AudioBank.SFX_SHIP_STOP, this.gameObject);
            }

        }

        private void OnApplicationQuit()
        {
            // Make sure to disable rumble on game quit.
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }

        public void ForceStopVibration()
        {
            GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
        }

        private void UpdateJoystickRumbleState()
        {
            if (Time.time - joystickRumbleLastActivated > joystickRumblePeriod)
            {
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
            }
        }

        public void OnHitByWeapon(float damage, bool shotByPlayer)
        {
            if (InputManager.instance.settings.enableJoystickVibration)
            {
                GamePad.SetVibration(PlayerIndex.One, joystickMaxRumble, joystickMaxRumble);
                joystickRumbleLastActivated = Time.time;
            }
        }
    }

}