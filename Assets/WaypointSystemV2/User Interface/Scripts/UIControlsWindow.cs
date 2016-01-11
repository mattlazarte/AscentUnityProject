using Ascent.Managers.Input;
using Ascent.Managers.Input.Sources;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ascent.UI
{
    public class UIControlsWindow : UIWindow
    {
        protected enum Maps
        {
            MoveForwardMapPrimary,
            MoveForwardMapSecondary,
            StrafeRightMapPrimary,
            StrafeRightMapSecondary,
            StrafeUpMapPrimary,
            StrafeUpMapSecondary,
            RollLeftMapPrimary,
            RollLeftMapSecondary,
            YawDeltaMapPrimary,
            YawDeltaMapSecondary,
            PitchDeltaMapPrimary,
            PitchDeltaMapSecondary,
            SelectPlasmaGunMapPrimary,
            SelectPlasmaGunMapSecondary,
            SelectLaserMapPrimary,
            SelectLaserMapSecondary,
            SelectUnguidedMissilesMapPrimary,
            SelectUnguidedMissilesMapSecondary,
            SelectGuidedMissilesMapPrimary,
            SelectGuidedMissilesMapSecondary,
            CiclePrimaryWeaponMapPrimary,
            CiclePrimaryWeaponMapSecondary,
            CicleSecondaryWeaponMapPrimary,
            CicleSecondaryWeaponMapSecondary,
            ShootPrimaryWeaponMapPrimary,
            ShootPrimaryWeaponMapSecondary,
            ShootSecondaryWeaponMapPrimary,
            ShootSecondaryWeaponMapSecondary,
            ToggleAutoLevelMapPrimary,
            ToggleAutoLevelMapSecondary,
            ToggleHeadlightsMapPrimary,
            ToggleHeadlightsMapSecondary,
            PauseMapPrimary,
            PauseMapSecondary
        }

        public static UIControlsWindow instance;

        public UIButton buttonResetToDefault;
        public UIButton buttonBack;
        public Scrollbar scrollbar;

        public UIButton buttonMoveForwardMapPrimary;
        public UIButton buttonMoveForwardMapSecondary;
        public UIButton buttonStrafeRightMapPrimary;
        public UIButton buttonStrafeRightMapSecondary;
        public UIButton buttonStrafeUpMapPrimary;
        public UIButton buttonStrafeUpMapSecondary;
        public UIButton buttonRollLeftMapPrimary;
        public UIButton buttonRollLeftMapSecondary;
        public UIButton buttonYawDeltaMapPrimary;
        public UIButton buttonYawDeltaMapSecondary;
        public UIButton buttonPitchDeltaMapPrimary;
        public UIButton buttonPitchDeltaMapSecondary;
        public UIButton buttonSelectPlasmaGunMapPrimary;
        public UIButton buttonSelectPlasmaGunMapSecondary;
        public UIButton buttonSelectLaserMapPrimary;
        public UIButton buttonSelectLaserMapSecondary;
        public UIButton buttonSelectUnguidedMissilesMapPrimary;
        public UIButton buttonSelectUnguidedMissilesMapSecondary;
        public UIButton buttonSelectGuidedMissilesMapPrimary;
        public UIButton buttonSelectGuidedMissilesMapSecondary;
        public UIButton buttonCiclePrimaryWeaponMapPrimary;
        public UIButton buttonCiclePrimaryWeaponMapSecondary;
        public UIButton buttonCicleSecondaryWeaponMapPrimary;
        public UIButton buttonCicleSecondaryWeaponMapSecondary;
        public UIButton buttonShootPrimaryWeaponMapPrimary;
        public UIButton buttonShootPrimaryWeaponMapSecondary;
        public UIButton buttonShootSecondaryWeaponMapPrimary;
        public UIButton buttonShootSecondaryWeaponMapSecondary;
        public UIButton buttonToggleAutoLevelMapPrimary;
        public UIButton buttonToggleAutoLevelMapSecondary;
        public UIButton buttonToggleHeadlightsMapPrimary;
        public UIButton buttonToggleHeadlightsMapSecondary;
        public UIButton buttonPauseMapPrimary;
        public UIButton buttonPauseMapSecondary;

        protected Color buttonLabelNormalColor = Color.white;
        protected Color buttonLabelNotAssignedColor = Color.gray;
        protected Color buttonLabelConflictColor = Color.red;
        protected bool gatherColorFromButton = true;

        public override void AwakeFromManager()
        {
            base.AwakeFromManager();
            CheckBindings();
            InitUI();

            instance = this;
        }
        protected void CheckBindings()
        {
            if (buttonResetToDefault == null) throw new Exception("buttonResetToDefault is null.");
            if (buttonBack == null) throw new Exception("buttonBack is null.");
            if (scrollbar == null) throw new Exception("scrollbar is null.");

            if (buttonMoveForwardMapPrimary == null) throw new Exception("buttonMoveForwardMapPrimary is null.");
            if (buttonMoveForwardMapSecondary == null) throw new Exception("buttonMoveForwardMapSecondary is null.");
            if (buttonStrafeRightMapPrimary == null) throw new Exception("buttonStrafeRightMapPrimary is null.");
            if (buttonStrafeRightMapSecondary == null) throw new Exception("buttonStrafeRightMapSecondary is null.");
            if (buttonStrafeUpMapPrimary == null) throw new Exception("buttonStrafeUpMapPrimary is null.");
            if (buttonStrafeUpMapSecondary == null) throw new Exception("buttonStrafeUpMapSecondary is null.");
            if (buttonRollLeftMapPrimary == null) throw new Exception("buttonRollLeftMapPrimary is null.");
            if (buttonRollLeftMapSecondary == null) throw new Exception("buttonRollLeftMapSecondary is null.");
            if (buttonYawDeltaMapPrimary == null) throw new Exception("buttonYawDeltaMapPrimary is null.");
            if (buttonYawDeltaMapSecondary == null) throw new Exception("buttonYawDeltaMapSecondary is null.");
            if (buttonPitchDeltaMapPrimary == null) throw new Exception("buttonPitchDeltaMapPrimary is null.");
            if (buttonPitchDeltaMapSecondary == null) throw new Exception("buttonPitchDeltaMapSecondary is null.");
            if (buttonSelectPlasmaGunMapPrimary == null) throw new Exception("buttonSelectPlasmaGunMapPrimary is null.");
            if (buttonSelectPlasmaGunMapSecondary == null) throw new Exception("buttonSelectPlasmaGunMapSecondary is null.");
            if (buttonSelectLaserMapPrimary == null) throw new Exception("buttonSelectLaserMapPrimary is null.");
            if (buttonSelectLaserMapSecondary == null) throw new Exception("buttonSelectLaserMapSecondary is null.");
            if (buttonSelectUnguidedMissilesMapPrimary == null) throw new Exception("buttonSelectUnguidedMissilesMapPrimary is null.");
            if (buttonSelectUnguidedMissilesMapSecondary == null) throw new Exception("buttonSelectUnguidedMissilesMapSecondary is null.");
            if (buttonSelectGuidedMissilesMapPrimary == null) throw new Exception("buttonSelectGuidedMissilesMapPrimary is null.");
            if (buttonSelectGuidedMissilesMapSecondary == null) throw new Exception("buttonSelectGuidedMissilesMapSecondary is null.");
            if (buttonCiclePrimaryWeaponMapPrimary == null) throw new Exception("buttonCiclePrimaryWeaponMapPrimary is null.");
            if (buttonCiclePrimaryWeaponMapSecondary == null) throw new Exception("buttonCiclePrimaryWeaponMapSecondary is null.");
            if (buttonCicleSecondaryWeaponMapPrimary == null) throw new Exception("buttonCicleSecondaryWeaponMapPrimary is null.");
            if (buttonCicleSecondaryWeaponMapSecondary == null) throw new Exception("buttonCicleSecondaryWeaponMapSecondary is null.");
            if (buttonShootPrimaryWeaponMapPrimary == null) throw new Exception("buttonShootPrimaryWeaponMapPrimary is null.");
            if (buttonShootPrimaryWeaponMapSecondary == null) throw new Exception("buttonShootPrimaryWeaponMapSecondary is null.");
            if (buttonShootSecondaryWeaponMapPrimary == null) throw new Exception("buttonShootSecondaryWeaponMapPrimary is null.");
            if (buttonShootSecondaryWeaponMapSecondary == null) throw new Exception("buttonShootSecondaryWeaponMapSecondary is null.");
            if (buttonToggleAutoLevelMapPrimary == null) throw new Exception("buttonToggleAutoLevelMapPrimary is null.");
            if (buttonToggleAutoLevelMapSecondary == null) throw new Exception("buttonToggleAutoLevelMapSecondary is null.");
            if (buttonToggleHeadlightsMapPrimary == null) throw new Exception("buttonToggleHeadlightsMapPrimary is null.");
            if (buttonToggleHeadlightsMapSecondary == null) throw new Exception("buttonToggleHeadlightsMapSecondary is null.");
            if (buttonPauseMapPrimary == null) throw new Exception("buttonPauseMapPrimary is null.");
            if (buttonPauseMapSecondary == null) throw new Exception("buttonPauseMapSecondary is null.");
        }
        protected void UpdateMappingButtons()
        {
            if (gatherColorFromButton)
            {
                gatherColorFromButton = false;
                buttonLabelNormalColor = buttonMoveForwardMapPrimary.labelColor.Copy();
                buttonLabelNotAssignedColor = buttonLabelNormalColor.CopyWithHalfAlpha();
            }

            var im = InputManager.instance;

            buttonMoveForwardMapPrimary.label = im.settings.MoveForwardMap.GetPrimarySourceDescription();
            buttonMoveForwardMapPrimary.labelColor = buttonLabelNormalColor;
            buttonMoveForwardMapSecondary.label = im.settings.MoveForwardMap.GetSecondarySourceDescription();
            buttonMoveForwardMapSecondary.labelColor = im.settings.MoveForwardMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonStrafeRightMapPrimary.label = im.settings.StrafeRightMap.GetPrimarySourceDescription();
            buttonStrafeRightMapPrimary.labelColor = buttonLabelNormalColor;
            buttonStrafeRightMapSecondary.label = im.settings.StrafeRightMap.GetSecondarySourceDescription();
            buttonStrafeRightMapSecondary.labelColor = im.settings.StrafeRightMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonStrafeUpMapPrimary.label = im.settings.StrafeUpMap.GetPrimarySourceDescription();
            buttonStrafeUpMapPrimary.labelColor = buttonLabelNormalColor;
            buttonStrafeUpMapSecondary.label = im.settings.StrafeUpMap.GetSecondarySourceDescription();
            buttonStrafeUpMapSecondary.labelColor = im.settings.StrafeUpMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonRollLeftMapPrimary.label = im.settings.RollLeftMap.GetPrimarySourceDescription();
            buttonRollLeftMapPrimary.labelColor = buttonLabelNormalColor;
            buttonRollLeftMapSecondary.label = im.settings.RollLeftMap.GetSecondarySourceDescription();
            buttonRollLeftMapSecondary.labelColor = im.settings.RollLeftMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonYawDeltaMapPrimary.label = im.settings.YawDeltaMap.GetPrimarySourceDescription();
            buttonYawDeltaMapPrimary.labelColor = buttonLabelNormalColor;
            buttonYawDeltaMapSecondary.label = im.settings.YawDeltaMap.GetSecondarySourceDescription();
            buttonYawDeltaMapSecondary.labelColor = im.settings.YawDeltaMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonPitchDeltaMapPrimary.label = im.settings.PitchDeltaMap.GetPrimarySourceDescription();
            buttonPitchDeltaMapPrimary.labelColor = buttonLabelNormalColor;
            buttonPitchDeltaMapSecondary.label = im.settings.PitchDeltaMap.GetSecondarySourceDescription();
            buttonPitchDeltaMapSecondary.labelColor = im.settings.PitchDeltaMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonSelectPlasmaGunMapPrimary.label = im.settings.SelectPlasmaGunMap.GetPrimarySourceDescription();
            buttonSelectPlasmaGunMapPrimary.labelColor = buttonLabelNormalColor;
            buttonSelectPlasmaGunMapSecondary.label = im.settings.SelectPlasmaGunMap.GetSecondarySourceDescription();
            buttonSelectPlasmaGunMapSecondary.labelColor = im.settings.SelectPlasmaGunMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonSelectLaserMapPrimary.label = im.settings.SelectLaserMap.GetPrimarySourceDescription();
            buttonSelectLaserMapPrimary.labelColor = buttonLabelNormalColor;
            buttonSelectLaserMapSecondary.label = im.settings.SelectLaserMap.GetSecondarySourceDescription();
            buttonSelectLaserMapSecondary.labelColor = im.settings.SelectLaserMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonSelectUnguidedMissilesMapPrimary.label = im.settings.SelectUnguidedMissilesMap.GetPrimarySourceDescription();
            buttonSelectUnguidedMissilesMapPrimary.labelColor = buttonLabelNormalColor;
            buttonSelectUnguidedMissilesMapSecondary.label = im.settings.SelectUnguidedMissilesMap.GetSecondarySourceDescription();
            buttonSelectUnguidedMissilesMapSecondary.labelColor = im.settings.SelectUnguidedMissilesMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonSelectGuidedMissilesMapPrimary.label = im.settings.SelectGuidedMissilesMap.GetPrimarySourceDescription();
            buttonSelectGuidedMissilesMapPrimary.labelColor = buttonLabelNormalColor;
            buttonSelectGuidedMissilesMapSecondary.label = im.settings.SelectGuidedMissilesMap.GetSecondarySourceDescription();
            buttonSelectGuidedMissilesMapSecondary.labelColor = im.settings.SelectGuidedMissilesMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonCiclePrimaryWeaponMapPrimary.label = im.settings.CiclePrimaryWeaponMap.GetPrimarySourceDescription();
            buttonCiclePrimaryWeaponMapPrimary.labelColor = buttonLabelNormalColor;
            buttonCiclePrimaryWeaponMapSecondary.label = im.settings.CiclePrimaryWeaponMap.GetSecondarySourceDescription();
            buttonCiclePrimaryWeaponMapSecondary.labelColor = im.settings.CiclePrimaryWeaponMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonCicleSecondaryWeaponMapPrimary.label = im.settings.CicleSecondaryWeaponMap.GetPrimarySourceDescription();
            buttonCicleSecondaryWeaponMapPrimary.labelColor = buttonLabelNormalColor;
            buttonCicleSecondaryWeaponMapSecondary.label = im.settings.CicleSecondaryWeaponMap.GetSecondarySourceDescription();
            buttonCicleSecondaryWeaponMapSecondary.labelColor = im.settings.CicleSecondaryWeaponMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonShootPrimaryWeaponMapPrimary.label = im.settings.ShootPrimaryWeaponMap.GetPrimarySourceDescription();
            buttonShootPrimaryWeaponMapPrimary.labelColor = buttonLabelNormalColor;
            buttonShootPrimaryWeaponMapSecondary.label = im.settings.ShootPrimaryWeaponMap.GetSecondarySourceDescription();
            buttonShootPrimaryWeaponMapSecondary.labelColor = im.settings.ShootPrimaryWeaponMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonShootSecondaryWeaponMapPrimary.label = im.settings.ShootSecondaryWeaponMap.GetPrimarySourceDescription();
            buttonShootSecondaryWeaponMapPrimary.labelColor = buttonLabelNormalColor;
            buttonShootSecondaryWeaponMapSecondary.label = im.settings.ShootSecondaryWeaponMap.GetSecondarySourceDescription();
            buttonShootSecondaryWeaponMapSecondary.labelColor = im.settings.ShootSecondaryWeaponMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonToggleAutoLevelMapPrimary.label = im.settings.ToggleAutoLevelMap.GetPrimarySourceDescription();
            buttonToggleAutoLevelMapPrimary.labelColor = buttonLabelNormalColor;
            buttonToggleAutoLevelMapSecondary.label = im.settings.ToggleAutoLevelMap.GetSecondarySourceDescription();
            buttonToggleAutoLevelMapSecondary.labelColor = im.settings.ToggleAutoLevelMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonToggleHeadlightsMapPrimary.label = im.settings.ToggleHeadlightsMap.GetPrimarySourceDescription();
            buttonToggleHeadlightsMapPrimary.labelColor = buttonLabelNormalColor;
            buttonToggleHeadlightsMapSecondary.label = im.settings.ToggleHeadlightsMap.GetSecondarySourceDescription();
            buttonToggleHeadlightsMapSecondary.labelColor = im.settings.ToggleHeadlightsMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            buttonPauseMapPrimary.label = im.settings.PauseMap.GetPrimarySourceDescription();
            buttonPauseMapPrimary.labelColor = buttonLabelNormalColor;
            buttonPauseMapSecondary.label = im.settings.PauseMap.GetSecondarySourceDescription();
            buttonPauseMapSecondary.labelColor = im.settings.PauseMap.SecondarySource == null ? buttonLabelNotAssignedColor : buttonLabelNormalColor;

            CheckForConflicts();

            if (conflictingMaps.Count == 0)
            {
                buttonBack.Enable();
            }
            else
            {
                buttonBack.Disable();
                if (conflictingMaps.Contains(Maps.MoveForwardMapPrimary)) buttonMoveForwardMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.MoveForwardMapSecondary)) buttonMoveForwardMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.StrafeRightMapPrimary)) buttonStrafeRightMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.StrafeRightMapSecondary)) buttonStrafeRightMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.StrafeUpMapPrimary)) buttonStrafeUpMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.StrafeUpMapSecondary)) buttonStrafeUpMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.RollLeftMapPrimary)) buttonRollLeftMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.RollLeftMapSecondary)) buttonRollLeftMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.YawDeltaMapPrimary)) buttonYawDeltaMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.YawDeltaMapSecondary)) buttonYawDeltaMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.PitchDeltaMapPrimary)) buttonPitchDeltaMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.PitchDeltaMapSecondary)) buttonPitchDeltaMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectPlasmaGunMapPrimary)) buttonSelectPlasmaGunMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectPlasmaGunMapSecondary)) buttonSelectPlasmaGunMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectLaserMapPrimary)) buttonSelectLaserMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectLaserMapSecondary)) buttonSelectLaserMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectUnguidedMissilesMapPrimary)) buttonSelectUnguidedMissilesMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectUnguidedMissilesMapSecondary)) buttonSelectUnguidedMissilesMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectGuidedMissilesMapPrimary)) buttonSelectGuidedMissilesMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.SelectGuidedMissilesMapSecondary)) buttonSelectGuidedMissilesMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.CiclePrimaryWeaponMapPrimary)) buttonCiclePrimaryWeaponMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.CiclePrimaryWeaponMapSecondary)) buttonCiclePrimaryWeaponMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.CicleSecondaryWeaponMapPrimary)) buttonCicleSecondaryWeaponMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.CicleSecondaryWeaponMapSecondary)) buttonCicleSecondaryWeaponMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ShootPrimaryWeaponMapPrimary)) buttonShootPrimaryWeaponMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ShootPrimaryWeaponMapSecondary)) buttonShootPrimaryWeaponMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ShootSecondaryWeaponMapPrimary)) buttonShootSecondaryWeaponMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ShootSecondaryWeaponMapSecondary)) buttonShootSecondaryWeaponMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ToggleAutoLevelMapPrimary)) buttonToggleAutoLevelMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ToggleAutoLevelMapSecondary)) buttonToggleAutoLevelMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ToggleHeadlightsMapPrimary)) buttonToggleHeadlightsMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.ToggleHeadlightsMapSecondary)) buttonToggleHeadlightsMapSecondary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.PauseMapPrimary)) buttonPauseMapPrimary.labelColor = buttonLabelConflictColor;
                if (conflictingMaps.Contains(Maps.PauseMapSecondary)) buttonPauseMapSecondary.labelColor = buttonLabelConflictColor;
            }
        }

        protected void InitUI()
        {
            buttonBack.onClick.AddListener(() => FadeOut(() => UIOptionsWindow.instance.BackFadeIn()));

            buttonResetToDefault.onClick.AddListener(() =>
            {
                UIConfirmWindow.Setup(
                    text: "You are about to reset all the controllers to default values.",
                    onConfirm: () =>
                    {
                        InputManager.instance.InitDefaultSettings();
                        UIConfirmWindow.instance.FadeOut(() => FadeIn());
                    },
                    onCancel: () =>
                    {
                        UIConfirmWindow.instance.FadeOut(() => FadeIn());
                    }
                );

                FadeOut(() => UIConfirmWindow.instance.FadeIn());
            });

            buttonMoveForwardMapPrimary.onClick.AddListener(() => SelectAxisSource("Thrust Forward/Backward", (newAxisSource) => InputManager.instance.settings.MoveForwardMap.PrimarySource = newAxisSource, false));
            buttonMoveForwardMapSecondary.onClick.AddListener(() => SelectAxisSource("Thrust Forward/Backward", (newAxisSource) => InputManager.instance.settings.MoveForwardMap.SecondarySource = newAxisSource));

            buttonStrafeRightMapPrimary.onClick.AddListener(() => SelectAxisSource("Thrust Right/Left", (newAxisSource) => InputManager.instance.settings.StrafeRightMap.PrimarySource = newAxisSource, false));
            buttonStrafeRightMapSecondary.onClick.AddListener(() => SelectAxisSource("Thrust Right/Left", (newAxisSource) => InputManager.instance.settings.StrafeRightMap.SecondarySource = newAxisSource));

            buttonStrafeUpMapPrimary.onClick.AddListener(() => SelectAxisSource("Thrust Up/Down", (newAxisSource) => InputManager.instance.settings.StrafeUpMap.PrimarySource = newAxisSource, false));
            buttonStrafeUpMapSecondary.onClick.AddListener(() => SelectAxisSource("Thrust Up/Down", (newAxisSource) => InputManager.instance.settings.StrafeUpMap.SecondarySource = newAxisSource));

            buttonRollLeftMapPrimary.onClick.AddListener(() => SelectAxisSource("Roll", (newAxisSource) => InputManager.instance.settings.RollLeftMap.PrimarySource = newAxisSource, false));
            buttonRollLeftMapSecondary.onClick.AddListener(() => SelectAxisSource("Roll", (newAxisSource) => InputManager.instance.settings.RollLeftMap.SecondarySource = newAxisSource));

            buttonYawDeltaMapPrimary.onClick.AddListener(() => SelectAxisSource("Yaw", (newAxisSource) => InputManager.instance.settings.YawDeltaMap.PrimarySource = newAxisSource, false));
            buttonYawDeltaMapSecondary.onClick.AddListener(() => SelectAxisSource("Yaw", (newAxisSource) => InputManager.instance.settings.YawDeltaMap.SecondarySource = newAxisSource));

            buttonPitchDeltaMapPrimary.onClick.AddListener(() => SelectAxisSource("Pitch", (newAxisSource) => InputManager.instance.settings.PitchDeltaMap.PrimarySource = newAxisSource, false));
            buttonPitchDeltaMapSecondary.onClick.AddListener(() => SelectAxisSource("Pitch", (newAxisSource) => InputManager.instance.settings.PitchDeltaMap.SecondarySource = newAxisSource));

            buttonSelectPlasmaGunMapPrimary.onClick.AddListener(() => SelectButtonSource("Select Plasma Gun", (newButtonSource) => InputManager.instance.settings.SelectPlasmaGunMap.PrimarySource = newButtonSource, false));
            buttonSelectPlasmaGunMapSecondary.onClick.AddListener(() => SelectButtonSource("Select Plasma Gun", (newButtonSource) => InputManager.instance.settings.SelectPlasmaGunMap.SecondarySource = newButtonSource));

            buttonSelectLaserMapPrimary.onClick.AddListener(() => SelectButtonSource("Select Laser", (newButtonSource) => InputManager.instance.settings.SelectLaserMap.PrimarySource = newButtonSource, false));
            buttonSelectLaserMapSecondary.onClick.AddListener(() => SelectButtonSource("Select Laser", (newButtonSource) => InputManager.instance.settings.SelectLaserMap.SecondarySource = newButtonSource));

            buttonSelectUnguidedMissilesMapPrimary.onClick.AddListener(() => SelectButtonSource("Select Unguided Missiles", (newButtonSource) => InputManager.instance.settings.SelectUnguidedMissilesMap.PrimarySource = newButtonSource, false));
            buttonSelectUnguidedMissilesMapSecondary.onClick.AddListener(() => SelectButtonSource("Select Unguided Missiles", (newButtonSource) => InputManager.instance.settings.SelectUnguidedMissilesMap.SecondarySource = newButtonSource));

            buttonSelectGuidedMissilesMapPrimary.onClick.AddListener(() => SelectButtonSource("Select Guided Missiles", (newButtonSource) => InputManager.instance.settings.SelectGuidedMissilesMap.PrimarySource = newButtonSource, false));
            buttonSelectGuidedMissilesMapSecondary.onClick.AddListener(() => SelectButtonSource("Select Guided Missiles", (newButtonSource) => InputManager.instance.settings.SelectGuidedMissilesMap.SecondarySource = newButtonSource));

            buttonCiclePrimaryWeaponMapPrimary.onClick.AddListener(() => SelectButtonSource("Cicle Primary Weapon", (newButtonSource) => InputManager.instance.settings.CiclePrimaryWeaponMap.PrimarySource = newButtonSource, false));
            buttonCiclePrimaryWeaponMapSecondary.onClick.AddListener(() => SelectButtonSource("Cicle Primary Weapon", (newButtonSource) => InputManager.instance.settings.CiclePrimaryWeaponMap.SecondarySource = newButtonSource));

            buttonCicleSecondaryWeaponMapPrimary.onClick.AddListener(() => SelectButtonSource("Cicle Secondary Weapon", (newButtonSource) => InputManager.instance.settings.CicleSecondaryWeaponMap.PrimarySource = newButtonSource, false));
            buttonCicleSecondaryWeaponMapSecondary.onClick.AddListener(() => SelectButtonSource("Cicle Secondary Weapon", (newButtonSource) => InputManager.instance.settings.CicleSecondaryWeaponMap.SecondarySource = newButtonSource));

            buttonShootPrimaryWeaponMapPrimary.onClick.AddListener(() => SelectButtonSource("Shoot Primary Weapon", (newButtonSource) => InputManager.instance.settings.ShootPrimaryWeaponMap.PrimarySource = newButtonSource, false));
            buttonShootPrimaryWeaponMapSecondary.onClick.AddListener(() => SelectButtonSource("Shoot Primary Weapon", (newButtonSource) => InputManager.instance.settings.ShootPrimaryWeaponMap.SecondarySource = newButtonSource));

            buttonShootSecondaryWeaponMapPrimary.onClick.AddListener(() => SelectButtonSource("Shoot Secondary Weapon", (newButtonSource) => InputManager.instance.settings.ShootSecondaryWeaponMap.PrimarySource = newButtonSource, false));
            buttonShootSecondaryWeaponMapSecondary.onClick.AddListener(() => SelectButtonSource("Shoot Secondary Weapon", (newButtonSource) => InputManager.instance.settings.ShootSecondaryWeaponMap.SecondarySource = newButtonSource));

            buttonToggleAutoLevelMapPrimary.onClick.AddListener(() => SelectButtonSource("Toggle Auto-Level", (newButtonSource) => InputManager.instance.settings.ToggleAutoLevelMap.PrimarySource = newButtonSource, false));
            buttonToggleAutoLevelMapSecondary.onClick.AddListener(() => SelectButtonSource("Toggle Auto-Level", (newButtonSource) => InputManager.instance.settings.ToggleAutoLevelMap.SecondarySource = newButtonSource));

            buttonToggleHeadlightsMapPrimary.onClick.AddListener(() => SelectButtonSource("Toggle Headlights", (newButtonSource) => InputManager.instance.settings.ToggleHeadlightsMap.PrimarySource = newButtonSource, false));
            buttonToggleHeadlightsMapSecondary.onClick.AddListener(() => SelectButtonSource("Toggle Headlights", (newButtonSource) => InputManager.instance.settings.ToggleHeadlightsMap.SecondarySource = newButtonSource));

            buttonPauseMapPrimary.onClick.AddListener(() => SelectButtonSource("Pause", (newButtonSource) => InputManager.instance.settings.PauseMap.PrimarySource = newButtonSource, false));
            buttonPauseMapSecondary.onClick.AddListener(() => SelectButtonSource("Pause", (newButtonSource) => InputManager.instance.settings.PauseMap.SecondarySource = newButtonSource));
        }

        protected void SelectAxisSource(string controlName, Action<AxisSource> onSelect, bool showNotAssignedOption = true)
        {
            UISelectAxisSourceWindow.showNotAssignedOption = showNotAssignedOption;
            UISelectAxisSourceWindow.onCancel = () =>
            {
                UISelectAxisSourceWindow.instance.FadeOut(() => BackFadeIn());
            };

            UISelectAxisSourceWindow.onSelect = (axisSource) =>
            {
                onSelect(axisSource);
                //UpdateMappingButtons();
            };

            UISelectAxisSourceWindow.controlName = controlName;
            FadeOut(() => UISelectAxisSourceWindow.instance.FadeIn());
        }

        protected void SelectButtonSource(string controlName, Action<ButtonSource> onSelect, bool showNotAssignedOption = true)
        {
            UISelectButtonSourceWindow.showNotAssignedOption = showNotAssignedOption;
            UISelectButtonSourceWindow.onCancel = () =>
            {
                UISelectButtonSourceWindow.instance.FadeOut(() => BackFadeIn());
            };

            UISelectButtonSourceWindow.onSelect = (buttonSource) =>
            {
                onSelect(buttonSource);
                //UpdateMappingButtons();
            };

            UISelectButtonSourceWindow.controlName = controlName;
            FadeOut(() => UISelectButtonSourceWindow.instance.FadeIn());
        }

        protected override void OnBeforeFadeIn()
        {
            base.OnBeforeFadeIn();
            scrollbar.value = 1f;
            UpdateMappingButtons();
            buttonBack.MuteSelect();
        }

        protected override void OnBeforeBackFadeIn()
        {
            base.OnBeforeBackFadeIn();
            UpdateMappingButtons();
        }

        protected HashSet<Maps> conflictingMaps;
        protected void CheckForConflicts()
        {
            var mapsPerKeyboardKey = new Dictionary<MappableKeys, HashSet<Maps>>();
            var mapsPerMouseButton = new Dictionary<MouseButton, HashSet<Maps>>();
            var mapsPerJoystickButton = new Dictionary<XboxJoystickButtons, HashSet<Maps>>();
            var mapsPerJoystickDPad = new Dictionary<XboxJoystickDPad, HashSet<Maps>>();
            var mapsPerMouseAxis = new Dictionary<MouseAxis, HashSet<Maps>>();
            var mapsPerJoystickAxis = new Dictionary<XboxJoystickAxis, HashSet<Maps>>();

            MapInputsPerMapInAxisSource(InputManager.instance.settings.MoveForwardMap.PrimarySource, Maps.MoveForwardMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInAxisSource(InputManager.instance.settings.MoveForwardMap.SecondarySource, Maps.MoveForwardMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInAxisSource(InputManager.instance.settings.StrafeRightMap.PrimarySource, Maps.StrafeRightMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInAxisSource(InputManager.instance.settings.StrafeRightMap.SecondarySource, Maps.StrafeRightMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInAxisSource(InputManager.instance.settings.StrafeUpMap.PrimarySource, Maps.StrafeUpMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInAxisSource(InputManager.instance.settings.StrafeUpMap.SecondarySource, Maps.StrafeUpMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInAxisSource(InputManager.instance.settings.RollLeftMap.PrimarySource, Maps.RollLeftMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInAxisSource(InputManager.instance.settings.RollLeftMap.SecondarySource, Maps.RollLeftMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInAxisSource(InputManager.instance.settings.YawDeltaMap.PrimarySource, Maps.YawDeltaMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInAxisSource(InputManager.instance.settings.YawDeltaMap.SecondarySource, Maps.YawDeltaMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInAxisSource(InputManager.instance.settings.PitchDeltaMap.PrimarySource, Maps.PitchDeltaMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInAxisSource(InputManager.instance.settings.PitchDeltaMap.SecondarySource, Maps.PitchDeltaMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectPlasmaGunMap.PrimarySource, Maps.SelectPlasmaGunMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectPlasmaGunMap.SecondarySource, Maps.SelectPlasmaGunMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectLaserMap.PrimarySource, Maps.SelectLaserMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectLaserMap.SecondarySource, Maps.SelectLaserMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectUnguidedMissilesMap.PrimarySource, Maps.SelectUnguidedMissilesMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectUnguidedMissilesMap.SecondarySource, Maps.SelectUnguidedMissilesMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectGuidedMissilesMap.PrimarySource, Maps.SelectGuidedMissilesMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.SelectGuidedMissilesMap.SecondarySource, Maps.SelectGuidedMissilesMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.CiclePrimaryWeaponMap.PrimarySource, Maps.CiclePrimaryWeaponMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.CiclePrimaryWeaponMap.SecondarySource, Maps.CiclePrimaryWeaponMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.CicleSecondaryWeaponMap.PrimarySource, Maps.CicleSecondaryWeaponMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.CicleSecondaryWeaponMap.SecondarySource, Maps.CicleSecondaryWeaponMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.ShootPrimaryWeaponMap.PrimarySource, Maps.ShootPrimaryWeaponMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.ShootPrimaryWeaponMap.SecondarySource, Maps.ShootPrimaryWeaponMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.ShootSecondaryWeaponMap.PrimarySource, Maps.ShootSecondaryWeaponMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.ShootSecondaryWeaponMap.SecondarySource, Maps.ShootSecondaryWeaponMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.ToggleAutoLevelMap.PrimarySource, Maps.ToggleAutoLevelMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.ToggleAutoLevelMap.SecondarySource, Maps.ToggleAutoLevelMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.ToggleHeadlightsMap.PrimarySource, Maps.ToggleHeadlightsMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.ToggleHeadlightsMap.SecondarySource, Maps.ToggleHeadlightsMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            MapInputsPerMapInButtonSource(InputManager.instance.settings.PauseMap.PrimarySource, Maps.PauseMapPrimary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);
            MapInputsPerMapInButtonSource(InputManager.instance.settings.PauseMap.SecondarySource, Maps.PauseMapSecondary, mapsPerKeyboardKey, mapsPerMouseButton, mapsPerJoystickButton, mapsPerJoystickDPad, mapsPerMouseAxis, mapsPerJoystickAxis);

            conflictingMaps = new HashSet<Maps>();

            foreach (var pair in mapsPerKeyboardKey)
                if (pair.Value.Count > 1)
                    foreach (var map in pair.Value)
                        conflictingMaps.Add(map);

            foreach (var pair in mapsPerMouseButton)
                if (pair.Value.Count > 1)
                    foreach (var map in pair.Value)
                        conflictingMaps.Add(map);

            foreach (var pair in mapsPerJoystickButton)
                if (pair.Value.Count > 1)
                    foreach (var map in pair.Value)
                        conflictingMaps.Add(map);

            foreach (var pair in mapsPerJoystickDPad)
                if (pair.Value.Count > 1)
                    foreach (var map in pair.Value)
                        conflictingMaps.Add(map);

            foreach (var pair in mapsPerMouseAxis)
                if (pair.Value.Count > 1)
                    foreach (var map in pair.Value)
                        conflictingMaps.Add(map);

            foreach (var pair in mapsPerJoystickAxis)
                if (pair.Value.Count > 1)
                    foreach (var map in pair.Value)
                        conflictingMaps.Add(map);
        }
        protected void MapInputsPerMapInButtonSource(
            ButtonSource buttonSource, Maps map,
            Dictionary<MappableKeys, HashSet<Maps>> mapsPerKeyboardKey,
            Dictionary<MouseButton, HashSet<Maps>> mapsPerMouseButton,
            Dictionary<XboxJoystickButtons, HashSet<Maps>> mapsPerJoystickButton,
            Dictionary<XboxJoystickDPad, HashSet<Maps>> mapsPerJoystickDPad,
            Dictionary<MouseAxis, HashSet<Maps>> mapsPerMouseAxis,
            Dictionary<XboxJoystickAxis, HashSet<Maps>> mapsPerJoystickAxis
            )
        {
            if (buttonSource == null) return;

            if (buttonSource.GetType() == typeof(KeyboardButtonSource))
                RelateMapWithKeyboardKey(map, mapsPerKeyboardKey, ((KeyboardButtonSource)buttonSource).keyCode);

            else if (buttonSource.GetType() == typeof(MouseButtonSource))
                RelateMapWithMouseButton(map, mapsPerMouseButton, ((MouseButtonSource)buttonSource).button);

            else if (buttonSource.GetType() == typeof(XboxJoystickAxisAsButtonSource))
                RelateMapWithJoystickAxis(map, mapsPerJoystickAxis, ((XboxJoystickAxisAsButtonSource)buttonSource).axis);

            else if (buttonSource.GetType() == typeof(XboxJoystickButtonSource))
                RelateMapWithJoystickButton(map, mapsPerJoystickButton, ((XboxJoystickButtonSource)buttonSource).button);

            else if (buttonSource.GetType() == typeof(XboxJoystickDPadButtonSource))
                RelateMapWithJoystickDPad(map, mapsPerJoystickDPad, ((XboxJoystickDPadButtonSource)buttonSource).button);
        }
        protected void MapInputsPerMapInAxisSource(
            AxisSource axisSource, Maps map,
            Dictionary<MappableKeys, HashSet<Maps>> mapsPerKeyboardKey,
            Dictionary<MouseButton, HashSet<Maps>> mapsPerMouseButton,
            Dictionary<XboxJoystickButtons, HashSet<Maps>> mapsPerJoystickButton,
            Dictionary<XboxJoystickDPad, HashSet<Maps>> mapsPerJoystickDPad,
            Dictionary<MouseAxis, HashSet<Maps>> mapsPerMouseAxis,
            Dictionary<XboxJoystickAxis, HashSet<Maps>> mapsPerJoystickAxis
            )
        {
            if (axisSource == null) return;

            if (axisSource.GetType() == typeof(KeyboardAxisSource))
            {
                RelateMapWithKeyboardKey(map, mapsPerKeyboardKey, ((KeyboardAxisSource)axisSource).positiveKeyCode);
                RelateMapWithKeyboardKey(map, mapsPerKeyboardKey, ((KeyboardAxisSource)axisSource).negativeKeyCode);
            }
            else if (axisSource.GetType() == typeof(MouseAxisSource))
            {
                RelateMapWithMouseAxis(map, mapsPerMouseAxis, ((MouseAxisSource)axisSource).axis);
            }
            else if (axisSource.GetType() == typeof(XboxJoystickAxisSource))
            {
                RelateMapWithJoystickAxis(map, mapsPerJoystickAxis, ((XboxJoystickAxisSource)axisSource).axis);
            }
            else if (axisSource.GetType() == typeof(XboxJoystickButtonAsAxisSource))
            {
                RelateMapWithJoystickButton(map, mapsPerJoystickButton, ((XboxJoystickButtonAsAxisSource)axisSource).positiveButton);
                RelateMapWithJoystickButton(map, mapsPerJoystickButton, ((XboxJoystickButtonAsAxisSource)axisSource).negativeButton);
            }
            else if (axisSource.GetType() == typeof(XboxJoystickDPadButtonAsAxisSource))
            {
                RelateMapWithJoystickDPad(map, mapsPerJoystickDPad, ((XboxJoystickDPadButtonAsAxisSource)axisSource).positiveButton);
                RelateMapWithJoystickDPad(map, mapsPerJoystickDPad, ((XboxJoystickDPadButtonAsAxisSource)axisSource).negativeButton);
            }
        }
        protected void RelateMapWithKeyboardKey(Maps map, Dictionary<MappableKeys, HashSet<Maps>> mapsPerKeyboardKey, MappableKeys key)
        {
            if (!mapsPerKeyboardKey.ContainsKey(key))
                mapsPerKeyboardKey.Add(key, new HashSet<Maps>());

            mapsPerKeyboardKey[key].Add(map);
        }
        protected void RelateMapWithMouseButton(Maps map, Dictionary<MouseButton, HashSet<Maps>> mapsPerMouseButton, MouseButton button)
        {
            if (!mapsPerMouseButton.ContainsKey(button))
                mapsPerMouseButton.Add(button, new HashSet<Maps>());

            mapsPerMouseButton[button].Add(map);
        }
        protected void RelateMapWithJoystickButton(Maps map, Dictionary<XboxJoystickButtons, HashSet<Maps>> mapsPerJoystickButton, XboxJoystickButtons button)
        {
            if (!mapsPerJoystickButton.ContainsKey(button))
                mapsPerJoystickButton.Add(button, new HashSet<Maps>());

            mapsPerJoystickButton[button].Add(map);
        }
        protected void RelateMapWithJoystickDPad(Maps map, Dictionary<XboxJoystickDPad, HashSet<Maps>> mapsPerJoystickDPad, XboxJoystickDPad button)
        {
            if (!mapsPerJoystickDPad.ContainsKey(button))
                mapsPerJoystickDPad.Add(button, new HashSet<Maps>());

            mapsPerJoystickDPad[button].Add(map);
        }
        protected void RelateMapWithMouseAxis(Maps map, Dictionary<MouseAxis, HashSet<Maps>> mapsPerMouseAxis, MouseAxis axis)
        {
            if (!mapsPerMouseAxis.ContainsKey(axis))
                mapsPerMouseAxis.Add(axis, new HashSet<Maps>());

            mapsPerMouseAxis[axis].Add(map);
        }
        protected void RelateMapWithJoystickAxis(Maps map, Dictionary<XboxJoystickAxis, HashSet<Maps>> mapsPerJoystickAxis, XboxJoystickAxis axis)
        {
            if (!mapsPerJoystickAxis.ContainsKey(axis))
                mapsPerJoystickAxis.Add(axis, new HashSet<Maps>());

            mapsPerJoystickAxis[axis].Add(map);
        }
    }
}
