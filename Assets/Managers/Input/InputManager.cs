using System;
using UnityEngine;
using Ascent.Managers.Input.Sources;
using XInputDotNetPure;
using Newtonsoft.Json;
using System.IO;

namespace Ascent.Managers.Input
{
    public class InputSettings
    {
        public bool enableJoystickVibration;
        public bool invertVerticalAxis = true;
        public float mouseSensibility = 1f;
        public float joystickSensibility = 1f;
        public AxisInputMap MoveForwardMap { get; set; }
        public AxisInputMap StrafeRightMap { get; set; }
        public AxisInputMap StrafeUpMap { get; set; }
        public AxisInputMap RollLeftMap { get; set; }
        public AxisInputMap YawDeltaMap { get; set; }
        public AxisInputMap PitchDeltaMap { get; set; }
        public ButtonInputMap SelectPlasmaGunMap { get; set; }
        public ButtonInputMap SelectLaserMap { get; set; }
        public ButtonInputMap SelectUnguidedMissilesMap { get; set; }
        public ButtonInputMap SelectGuidedMissilesMap { get; set; }
        public ButtonInputMap CiclePrimaryWeaponMap { get; set; }
        public ButtonInputMap CicleSecondaryWeaponMap { get; set; }
        public ButtonInputMap ShootPrimaryWeaponMap { get; set; }
        public ButtonInputMap ShootSecondaryWeaponMap { get; set; }
        public ButtonInputMap ToggleAutoLevelMap { get; set; }
        public ButtonInputMap ToggleHeadlightsMap { get; set; }
        public ButtonInputMap PauseMap { get; set; }
    }

    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        public InputSettings settings;
        private PlayerInput cachedPlayerInput;

        private void Awake()
        {
            instance = this;
            Load();
        }

        private void Update()
        {
            if (settings == null) return;

            // IMPORTANT:
            // This script should be set to be the first to update
            // in script execution order to clean the cache before
            // evryone else try to get an updated version of the
            // input.

            // Clear cache.
            cachedPlayerInput = null;

            UpdateSources();
            UpdateVibration();
        }
        private void UpdateSources()
        {
            settings.MoveForwardMap.PrimarySource.Update();
            settings.StrafeRightMap.PrimarySource.Update();
            settings.RollLeftMap.PrimarySource.Update();
            settings.StrafeUpMap.PrimarySource.Update();
            settings.YawDeltaMap.PrimarySource.Update();
            settings.PitchDeltaMap.PrimarySource.Update();
            settings.ShootPrimaryWeaponMap.PrimarySource.Update();
            settings.ShootSecondaryWeaponMap.PrimarySource.Update();
            settings.ToggleAutoLevelMap.PrimarySource.Update();

            if (settings.MoveForwardMap.SecondarySource != null)
                settings.MoveForwardMap.SecondarySource.Update();

            if (settings.StrafeRightMap.SecondarySource != null)
                settings.StrafeRightMap.SecondarySource.Update();

            if (settings.RollLeftMap.SecondarySource != null)
                settings.RollLeftMap.SecondarySource.Update();

            if (settings.StrafeUpMap.SecondarySource != null)
                settings.StrafeUpMap.SecondarySource.Update();

            if (settings.YawDeltaMap.SecondarySource != null)
                settings.YawDeltaMap.SecondarySource.Update();

            if (settings.PitchDeltaMap.SecondarySource != null)
                settings.PitchDeltaMap.SecondarySource.Update();

            if (settings.ShootPrimaryWeaponMap.SecondarySource != null)
                settings.ShootPrimaryWeaponMap.SecondarySource.Update();

            if (settings.ShootSecondaryWeaponMap.SecondarySource != null)
                settings.ShootSecondaryWeaponMap.SecondarySource.Update();

            if (settings.ToggleAutoLevelMap.SecondarySource != null)
                settings.ToggleAutoLevelMap.SecondarySource.Update();
        }

        public void InitDefaultSettings()
        {
            settings = new InputSettings();

            settings.MoveForwardMap =
                new AxisInputMap(
                    false,
                    new KeyboardAxisSource(MappableKeys.W, MappableKeys.S),
                    new XboxJoystickAxisSource(XboxJoystickAxis.LeftStickY)
                );

            settings.StrafeRightMap =
                new AxisInputMap(
                    false,
                    new KeyboardAxisSource(MappableKeys.D, MappableKeys.A),
                    new XboxJoystickAxisSource(XboxJoystickAxis.LeftStickX)
                );

            settings.RollLeftMap =
                new AxisInputMap(
                    false,
                    new KeyboardAxisSource(MappableKeys.Q, MappableKeys.E),
                    new XboxJoystickButtonAsAxisSource(XboxJoystickButtons.X, XboxJoystickButtons.B)
                );

            settings.StrafeUpMap =
                new AxisInputMap(
                    false,
                    new KeyboardAxisSource(MappableKeys.R, MappableKeys.F),
                    new XboxJoystickButtonAsAxisSource(XboxJoystickButtons.RightBumper, XboxJoystickButtons.LeftBumper)
                );

            settings.YawDeltaMap =
                new AxisInputMap(
                    false,
                    new MouseAxisSource(MouseAxis.Horizontal),
                    new XboxJoystickAxisSource(XboxJoystickAxis.RightStickX)
                );

            settings.PitchDeltaMap =
                new AxisInputMap(
                    true,
                    new MouseAxisSource(MouseAxis.Vertical),
                    new XboxJoystickAxisSource(XboxJoystickAxis.RightStickY)
                );

            settings.SelectPlasmaGunMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new KeyboardButtonSource(MappableKeys.Alpha1)
                );

            settings.SelectLaserMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new KeyboardButtonSource(MappableKeys.Alpha2)
                );

            settings.SelectUnguidedMissilesMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new KeyboardButtonSource(MappableKeys.Alpha3)
                );

            settings.SelectGuidedMissilesMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new KeyboardButtonSource(MappableKeys.Alpha4)
                );

            settings.CiclePrimaryWeaponMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new XboxJoystickDPadButtonSource(XboxJoystickDPad.Right)
                );

            settings.CicleSecondaryWeaponMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new XboxJoystickDPadButtonSource(XboxJoystickDPad.Left)
                );

            settings.ToggleAutoLevelMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new MouseButtonSource(MouseButton.Middle),
                    new XboxJoystickButtonSource(XboxJoystickButtons.A)
                );

            settings.ToggleHeadlightsMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new KeyboardButtonSource(MappableKeys.H),
                    new XboxJoystickButtonSource(XboxJoystickButtons.Y)
                );

            settings.PauseMap =
                new ButtonInputMap(
                    ButtonState.Down,
                    new KeyboardButtonSource(MappableKeys.Escape),
                    new XboxJoystickButtonSource(XboxJoystickButtons.Start)
                );

            settings.ShootPrimaryWeaponMap =
                new ButtonInputMap(
                    ButtonState.On,
                    new MouseButtonSource(MouseButton.Left),
                    new XboxJoystickAxisAsButtonSource(XboxJoystickAxis.RightTrigger)
                );

            settings.ShootSecondaryWeaponMap =
                new ButtonInputMap(
                    ButtonState.On,
                    new MouseButtonSource(MouseButton.Right),
                    new XboxJoystickAxisAsButtonSource(XboxJoystickAxis.LeftTrigger)
                );
        }
        public PlayerInput GetPlayerInput()
        {
            if (cachedPlayerInput == null)
            {
                cachedPlayerInput = new PlayerInput();

                cachedPlayerInput.MoveForward = settings.MoveForwardMap.GetAxisCurrentState(settings.mouseSensibility, settings.joystickSensibility, settings.invertVerticalAxis);
                cachedPlayerInput.StrafeRight = settings.StrafeRightMap.GetAxisCurrentState(settings.mouseSensibility, settings.joystickSensibility, settings.invertVerticalAxis);
                cachedPlayerInput.RollLeft = settings.RollLeftMap.GetAxisCurrentState(settings.mouseSensibility, settings.joystickSensibility, settings.invertVerticalAxis);
                cachedPlayerInput.StrafeUp = settings.StrafeUpMap.GetAxisCurrentState(settings.mouseSensibility, settings.joystickSensibility, settings.invertVerticalAxis);
                cachedPlayerInput.YawDelta = settings.YawDeltaMap.GetAxisCurrentState(settings.mouseSensibility, settings.joystickSensibility, settings.invertVerticalAxis);
                cachedPlayerInput.PitchDelta = settings.PitchDeltaMap.GetAxisCurrentState(settings.mouseSensibility, settings.joystickSensibility, settings.invertVerticalAxis);

                cachedPlayerInput.SelectPlasmaGun = settings.SelectPlasmaGunMap.GetButtonCurrentState();
                cachedPlayerInput.SelectLaser = settings.SelectLaserMap.GetButtonCurrentState();
                cachedPlayerInput.SelectUnguidedMissiles = settings.SelectUnguidedMissilesMap.GetButtonCurrentState();
                cachedPlayerInput.SelectGuidedMissiles = settings.SelectGuidedMissilesMap.GetButtonCurrentState();

                cachedPlayerInput.CiclePrimaryWeapon = settings.CiclePrimaryWeaponMap.GetButtonCurrentState();
                cachedPlayerInput.CicleSecondaryWeapon = settings.CicleSecondaryWeaponMap.GetButtonCurrentState();

                cachedPlayerInput.ShootPrimaryWeapon = settings.ShootPrimaryWeaponMap.GetButtonCurrentState();
                cachedPlayerInput.ShootSecondaryWeapon = settings.ShootSecondaryWeaponMap.GetButtonCurrentState();

                cachedPlayerInput.ToggleAutoLevel = settings.ToggleAutoLevelMap.GetButtonCurrentState();
                cachedPlayerInput.ToggleHeadlights = settings.ToggleHeadlightsMap.GetButtonCurrentState();

                cachedPlayerInput.Pause = settings.PauseMap.GetButtonCurrentState();
            }

            return cachedPlayerInput;
        }

        private bool isVibrating;
        private float vibrationStartTime;
        private float vibrationForce = 1f;
        private float vibrationDuration = .07f;
        private void UpdateVibration()
        {
            if (isVibrating && Time.time - vibrationStartTime > vibrationDuration)
            {
                isVibrating = false;
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
            }
        }
        public void VibrateJoystick()
        {
            if (settings.enableJoystickVibration)
            {
                isVibrating = true;
                vibrationStartTime = Time.time;
                GamePad.SetVibration(PlayerIndex.One, vibrationForce, vibrationForce);
            }
        }

        private string GetPersistenceFilePath()
        {
            return Path.Combine(Application.persistentDataPath, "Inputs.json");
        }
        public void Load()
        {
            try
            {
                var path = GetPersistenceFilePath();
                var contents = File.ReadAllText(path);
                settings = JsonConvert.DeserializeObject<InputSettings>(contents, new JsonSerializerSettings{ TypeNameHandling = TypeNameHandling.All });
                Debug.LogFormat("InputManager settings loaded from: {0}", path);
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("InputManager loading error: {0}", ex.Message);

                InitDefaultSettings();
                Debug.LogWarning("InputManager using default settings.");
            }
        }
        public void Save()
        {
            try
            {
                var serialized = JsonConvert.SerializeObject(settings, Formatting.Indented, new JsonSerializerSettings{ TypeNameHandling = TypeNameHandling.All });
                var path = GetPersistenceFilePath();
                File.WriteAllText(path, serialized);
                Debug.LogFormat("InputManager settings saved at: {0}", path);
            }
            catch (Exception ex)
            {
                Debug.LogWarningFormat("InputManager saving error: {0}", ex.Message);
            }
        }
    }
}