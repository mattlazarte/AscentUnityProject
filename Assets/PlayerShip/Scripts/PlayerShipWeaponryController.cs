using Ascent.Managers.Game;
using Ascent.Managers.Input;
using Ascent.Weaponry;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Ascent.Managers.Audio;
using Ascent.Enemies;

namespace Ascent.PlayerShip
{
    public class PlayerShipWeaponryController : MonoBehaviour, IHittableObject
    {
        public event Action<IProjectile> OnProjectileLaunch;

        [Header("Ship Properties")]
        public float hullLevel = 100f;
        public float shieldLevel = 100f;
        public float autoRecoveryShieldUntil = 50f;
        public float autoRecoveryFrequency = 0.5f;
        public float autoRecoveryValue = 0.5f;
        private float lastShieldAutoRecoveryTime;
        private bool shieldsDown = false;

        [Header("Target Detection")]
        public Transform targetDetectorOrigin;
        public LayerMask enemyLayerMask;
        public LayerMask targetDetectionLayersToIgnore;
        public float targetDetectionRadius = 1f;
        public bool debugTargetDetection = false;
        public Image crosshair;
        public float crosshairChangeColorVelocity = 5f;
        public float cannonAutoAimVelocity = 5f;
        public Color crosshairColor = Color.white;
        public Color crosshairOnTargetColor = Color.red;
        private Color currentCrosshairColor = Color.white;
        private bool enemyOnTarget = false;
        public bool IsEnemyOnTarget { get { return enemyOnTarget; } }

        [Header("Locking System")]
        public Image lockArea;
        public Image LockAreaLimitHelper;
        public Image lockCrosshair;
        public Camera lockingSystemCamera;
        public Color lockCrosshairUnlockedColor = Color.blue;
        public Color lockCrosshairLockedColor = Color.red;
        private bool lockingSystemOn = false;
        private EnemyShipControllerV2 targetBeingLocked = null;
        private float targetBeingLockedStartTime;
        private float timeToLockTarget = 1f;

        [Header("Initial Ammo")]
        public int initialLaserAmmo = 5;
        public int initialUnguidedMissilesAmmo = 10;
        public int initialGuidedMissilesAmmo = 5;

        [Header("Cannon Attachments")]
        public Transform primaryWeaponLeftCannonAttachment;
        public Transform primaryWeaponRightCannonAttachment;
        public Transform secondaryWeaponLeftCannonAttachment;
        public Transform secondaryWeaponRightCannonAttachment;
        private Cannon selectedLeftPrimaryWeapon;
        private Cannon selectedRightPrimaryWeapon;
        private Cannon selectedLeftSecondaryWeapon;
        private Cannon selectedRightSecondaryWeapon;
        private Quaternion currentPrimaryWeaponLeftCannonRotation;
        private Quaternion currentPrimaryWeaponRightCannonRotation;
        private Quaternion currentSecondaryWeaponLeftCannonRotation;
        private Quaternion currentSecondaryWeaponRightCannonRotation;

        [Header("Cannons")]
        public PlasmaGunCannon leftPlasmaGun;
        public PlasmaGunCannon rightPlasmaGun;
        public LaserBeamCannon leftLaser;
        public LaserBeamCannon rightLaser;
        public UnguidedMissilesCannon leftUnguidedMissileCannon;
        public UnguidedMissilesCannon rightUnguidedMissileCannon;
        public GuidedMissilesCannon leftGuidedMissileCannon;
        public GuidedMissilesCannon rightGuidedMissileCannon;
        private PrimaryWeapon selectedPrimaryWeapon;
        private SecondaryWeapon selectedSecondaryWeapon;
        private enum PrimaryWeapon { None, PlasmaGun, Laser }
        private enum SecondaryWeapon { None, UnguidedMissiles, GuidedMissiles }
        private Dictionary<PrimaryWeapon, int> primaryWeaponAmmo;
        private Dictionary<SecondaryWeapon, int> secondaryWeaponAmmo;
        private Dictionary<PrimaryWeapon, bool> primaryWeaponInfiniteAmmo;
        private Dictionary<SecondaryWeapon, bool> secondaryWeaponInfiniteAmmo;
        private float depletedAudioPlayInterval = 0.2f;
        private float lastTimeDepletedAudioWasPlayed;

        [Header("Left Weapon Indicator")] // Secondary
        public Image leftWeaponIndicatorBackground;
        public Text leftWeaponIndicatorAmmoLabel;
        public Image leftWeaponIndicatorIcon;
        public Image leftWeaponIndicatorNoneIcon;
        public Image leftWeaponIndicatorInfiniteIcon;

        [Header("Right Weapon Indicator")] // Primary
        public Image rightWeaponIndicatorBackground;
        public Text rightWeaponIndicatorAmmoLabel;
        public Image rightWeaponIndicatorIcon;
        public Image rightWeaponIndicatorNoneIcon;
        public Image rightWeaponIndicatorInfiniteIcon;

        [Header("Weapon Icons")]
        public Sprite plasmaGunIcon;
        public Sprite laserIcon;
        public Sprite unguidedMissilesIcon;
        public Sprite guidedMissilesIcon;

        [Header("Shield and Hull Gauges")]
        public Sprite[] levelGaugeSprites;
        public Image shieldGaugeBackgroundImage;
        public Image shieldGaugeImage;
        public Image hullGaugeBackgroundImage;
        public Image hullGaugeImage;

        [Header("Effects - Hit Overlay")]
        public Image hitMaskOverlay;
        public float hitMaskOverlayDurationPerHit = 0.3f;
        private Color hitMaskOverlayOriginalColor;

        [Header("Effects - Levels Alarm")]
        public float shieldAlarmAtLevel = 20f;
        public float hullAlarmAtLevel = 30f;
        public Light alarmLight;
        public float alarmLightLowIntensity = 1f;
        public float alarmLightHighIntensity = 3f;
        private bool shieldAlarmEnabled = false;
        public bool hullAlarmEnabled = false;
        private Color shieldGaugeBackgroundImageOriginalColor;
        private Color hullGaugeBackgroundImageOriginalColor;
        private Sequence shieldAlarmSequence;
        private Sequence hullAlarmSequence;

        public void Start()
        {
            AkSoundEngine.SetRTPCValue("Health", 100);
			AkSoundEngine.SetRTPCValue("Shield", 0);
            AudioManager.instance.Play(AudioBank.SFX_SHIP_HEALTH, this.gameObject);

            BindingsCheck();

            leftPlasmaGun.gameObject.SetActive(false);
            rightPlasmaGun.gameObject.SetActive(false);
            leftLaser.gameObject.SetActive(false);
            rightLaser.gameObject.SetActive(false);
            leftUnguidedMissileCannon.gameObject.SetActive(false);
            rightUnguidedMissileCannon.gameObject.SetActive(false);
            leftGuidedMissileCannon.gameObject.SetActive(false);
            rightGuidedMissileCannon.gameObject.SetActive(false);

            leftPlasmaGun.OnProjectileLaunch += OnProjectileLaunch;
            rightPlasmaGun.OnProjectileLaunch += OnProjectileLaunch;
            leftUnguidedMissileCannon.OnProjectileLaunch += OnProjectileLaunch;
            rightUnguidedMissileCannon.OnProjectileLaunch += OnProjectileLaunch;

            SelectPrimaryWeapon(null, PrimaryWeapon.PlasmaGun, true);
            SelectSecondaryWeapon(null, SecondaryWeapon.UnguidedMissiles, true);

            primaryWeaponInfiniteAmmo = new Dictionary<PrimaryWeapon, bool>();
            primaryWeaponInfiniteAmmo[PrimaryWeapon.None] = true;
            primaryWeaponInfiniteAmmo[PrimaryWeapon.PlasmaGun] = true;
            primaryWeaponInfiniteAmmo[PrimaryWeapon.Laser] = false;

            primaryWeaponAmmo = new Dictionary<PrimaryWeapon, int>();
            primaryWeaponAmmo[PrimaryWeapon.Laser] = initialLaserAmmo;

            secondaryWeaponInfiniteAmmo = new Dictionary<SecondaryWeapon, bool>();
            secondaryWeaponInfiniteAmmo[SecondaryWeapon.None] = true;
            secondaryWeaponInfiniteAmmo[SecondaryWeapon.UnguidedMissiles] = false;
            secondaryWeaponInfiniteAmmo[SecondaryWeapon.GuidedMissiles] = false;

            secondaryWeaponAmmo = new Dictionary<SecondaryWeapon, int>();
            secondaryWeaponAmmo[SecondaryWeapon.UnguidedMissiles] = initialUnguidedMissilesAmmo;
            secondaryWeaponAmmo[SecondaryWeapon.GuidedMissiles] = initialGuidedMissilesAmmo;

            currentPrimaryWeaponLeftCannonRotation = transform.rotation;
            currentPrimaryWeaponRightCannonRotation = transform.rotation;
            currentSecondaryWeaponLeftCannonRotation = transform.rotation;
            currentSecondaryWeaponRightCannonRotation = transform.rotation;

            InitCrosshairs();
            InitLevelsAlarms();

            hitMaskOverlayOriginalColor = hitMaskOverlay.color.Copy();
            hitMaskOverlay.color = hitMaskOverlay.color.Copy(0);
            hitMaskOverlay.enabled = true;
        }
        private void BindingsCheck()
        {
            if (alarmLight == null)
                throw new Exception("alarmLight is null.");

            if (LockAreaLimitHelper == null)
                throw new Exception("LockAreaLimitHelper is null.");

            if (lockArea == null)
                throw new Exception("lockArea is null.");

            if (crosshair == null)
                throw new Exception("crosshair is null.");

            if (leftPlasmaGun == null)
                throw new Exception("leftPlasmaGun is null.");

            if (rightPlasmaGun == null)
                throw new Exception("rightPlasmaGun is null.");

            if (leftLaser == null)
                throw new Exception("leftLaser is null.");

            if (rightLaser == null)
                throw new Exception("rightLaser is null.");

            if (leftUnguidedMissileCannon == null)
                throw new Exception("leftUnguidedMissileCannon is null.");

            if (rightUnguidedMissileCannon == null)
                throw new Exception("rightUnguidedMissileCannon is null.");

            if (leftGuidedMissileCannon == null)
                throw new Exception("leftGuidedMissileCannon is null.");

            if (rightGuidedMissileCannon == null)
                throw new Exception("rightGuidedMissileCannon is null.");

            if (targetDetectorOrigin == null)
                throw new Exception("targetDetectorOrigin is null.");

            if (primaryWeaponLeftCannonAttachment == null)
                throw new Exception("primaryWeaponLeftCannon is null.");

            if (primaryWeaponRightCannonAttachment == null)
                throw new Exception("primaryWeaponRightCannon is null.");

            if (secondaryWeaponLeftCannonAttachment == null)
                throw new Exception("secondaryWeaponLeftCannon is null.");

            if (secondaryWeaponRightCannonAttachment == null)
                throw new Exception("secondaryWeaponRightCannon is null.");

            if (plasmaGunIcon == null)
                throw new Exception("plasmaGunIcon is null.");

            if (laserIcon == null)
                throw new Exception("laserIcon is null.");

            if (unguidedMissilesIcon == null)
                throw new Exception("unguidedMissilesIcon is null.");

            if (guidedMissilesIcon == null)
                throw new Exception("guidedMissilesIcon is null.");

            if (leftWeaponIndicatorBackground == null)
                throw new Exception("leftWeaponIndicatorBackground is null.");

            if (leftWeaponIndicatorAmmoLabel == null)
                throw new Exception("leftWeaponIndicatorAmmoLabel is null.");

            if (leftWeaponIndicatorIcon == null)
                throw new Exception("leftWeaponIndicatorIcon is null.");

            if (leftWeaponIndicatorNoneIcon == null)
                throw new Exception("leftWeaponIndicatorNoneIcon is null.");

            if (leftWeaponIndicatorInfiniteIcon == null)
                throw new Exception("leftWeaponIndicatorInfiniteIcon is null.");

            if (rightWeaponIndicatorBackground == null)
                throw new Exception("rightWeaponIndicatorBackground is null.");

            if (rightWeaponIndicatorAmmoLabel == null)
                throw new Exception("rightWeaponIndicatorAmmoLabel is null.");

            if (rightWeaponIndicatorIcon == null)
                throw new Exception("rightWeaponIndicatorIcon is null.");

            if (rightWeaponIndicatorNoneIcon == null)
                throw new Exception("rightWeaponIndicatorNoneIcon is null.");

            if (rightWeaponIndicatorInfiniteIcon == null)
                throw new Exception("rightWeaponIndicatorInfiniteIcon is null.");

            if (levelGaugeSprites == null)
                throw new Exception("levelGaugeSprites is null.");

            if (levelGaugeSprites.Length != 18)
                throw new Exception("levelGaugeSprites must have a length of 18.");

            for (var i = 0; i < levelGaugeSprites.Length; i++)
                if (levelGaugeSprites[i] == null)
                    throw new Exception("levelGaugeSprites[" + i.ToString() + "] is null.");

            if (shieldGaugeImage == null)
                throw new Exception("shieldGaugeImage is null.");

            if (hullGaugeImage == null)
                throw new Exception("hullGaugeImage is null.");

            if (shieldGaugeBackgroundImage == null)
                throw new Exception("shieldGaugeBackgroundImage is null.");

            if (hullGaugeBackgroundImage == null)
                throw new Exception("hullGaugeBackgroundImage is null.");

            if (hitMaskOverlay == null)
                throw new Exception("hitMaskOverlay is null.");
        }
        private void InitCrosshairs()
        {
            currentCrosshairColor = crosshairColor;
        }
        private void InitLevelsAlarms()
        {
            var shieldFrequency = 0.3f;
            shieldGaugeBackgroundImageOriginalColor = shieldGaugeBackgroundImage.color.Copy();
            shieldAlarmSequence = DOTween.Sequence();
            shieldAlarmSequence.Append(shieldGaugeBackgroundImage.DOColor(new Color(1f, 0, 0, 1f), shieldFrequency));
            shieldAlarmSequence.Append(shieldGaugeBackgroundImage.DOColor(shieldGaugeBackgroundImageOriginalColor, shieldFrequency));
            shieldAlarmSequence.SetLoops(-1);
            shieldAlarmSequence.Pause();

            var hullFrequency = 0.1f;
            hullGaugeBackgroundImageOriginalColor = hullGaugeBackgroundImage.color.Copy();
            hullAlarmSequence = DOTween.Sequence();
            hullAlarmSequence.Append(hullGaugeBackgroundImage.DOColor(new Color(1f, 0, 0, 1f), hullFrequency));
            hullAlarmSequence.Append(hullGaugeBackgroundImage.DOColor(hullGaugeBackgroundImageOriginalColor, hullFrequency));
            hullAlarmSequence.Insert(0, alarmLight.DOIntensity(alarmLightHighIntensity, hullFrequency));
            hullAlarmSequence.Insert(hullFrequency, alarmLight.DOIntensity(alarmLightLowIntensity, hullFrequency));
            hullAlarmSequence.SetLoops(-1);
            hullAlarmSequence.Pause();
        }

        public void Update()
        {
            if (!PauseManager.instance.paused)
            {
                var input = InputManager.instance.GetPlayerInput();

                UpdateCannonAim();
                UpdateLockingSystem();
                UpdateWeaponChange(input);
                UpdateShoot(input);
                UpdateShieldAutoRecovery();
                UpdateGauge(hullGaugeImage, hullLevel);
                UpdateGauge(shieldGaugeImage, shieldLevel);
                UpdateAmmoDisplay();
                UpdateShields();
                UpdateAlarms();
            }
        }
        private void UpdateShields()
        {
            if (!shieldsDown && shieldLevel == 0)
            {
                //Debug.Log("Shields down!");
                shieldsDown = true;
                //SoundFxsManager.instance.PlayOneShot2D(SoundFx.ShieldsDown);
				AudioManager.instance.Play(AudioBank.SFX_SHIELD_LOW, this.gameObject);

                //shieldGaugeBackgroundImage.DOFade(0, 0.5f);
                //shieldGaugeImage.DOFade(0, 0.5f);
            }
            else if (shieldsDown && shieldLevel > 0)
            {
                //Debug.Log("Shields up!");
                shieldsDown = false;
                AudioManager.instance.Play(AudioBank.SFX_SHIELD_UP, this.gameObject);
                //SoundFxsManager.instance.PlayOneShot2D(SoundFx.ShieldsUp);


                //shieldGaugeBackgroundImage.DOFade(1, 0.5f);
                //shieldGaugeImage.DOFade(1, 0.5f);
            }
        }
        private void UpdateAlarms()
        {
            if (!shieldsDown && !shieldAlarmEnabled && shieldLevel <= shieldAlarmAtLevel)
            {
                EnableShieldAlarm();
            }
            else if (shieldAlarmEnabled && shieldLevel > shieldAlarmAtLevel)
            {
                DisableShieldAlarm();
            }
            else if (shieldAlarmEnabled && shieldsDown)
            {
                DisableShieldAlarm();
            }

            if (!hullAlarmEnabled && hullLevel <= hullAlarmAtLevel)
            {
                EnableHullAlarm();
            }
            else if (hullAlarmEnabled && hullLevel > hullAlarmAtLevel)
            {
                DisableHullAlarm();
            }
        }
        private void EnableShieldAlarm()
        {
            Debug.Log("EnableShieldAlarm");

            shieldAlarmEnabled = true;
            shieldAlarmSequence.Play();
            //SoundFxsManager.instance.LoopPlay2D("ShieldAlarm", SoundFx.ShieldAlarm);
            
            AudioManager.instance.Play(AudioBank.SFX_SHIP_SHIELD, this.gameObject);
        }
        private void DisableShieldAlarm()
        {
            Debug.Log("DisableShieldAlarm");

            shieldAlarmEnabled = false;
            shieldAlarmSequence.Pause().Rewind();
            //SoundFxsManager.instance.StopLooped("ShieldAlarm");
           
            AudioManager.instance.Play(AudioBank.SFX_SHIELD_DOWN, this.gameObject);
        }
        private void EnableHullAlarm()
        {
            Debug.Log("EnableHullAlarm");

            AudioManager.instance.Play(AudioBank.MUS_DANGER, this.gameObject);
            AudioManager.instance.Play(AudioBank.SFX_HEALTH_DOWN, this.gameObject);

            alarmLight.intensity = alarmLightLowIntensity;
            alarmLight.gameObject.SetActive(true);

            hullAlarmEnabled = true;
            hullAlarmSequence.Play();

            //SoundFxsManager.instance.LoopPlay2D("HullAlarm", SoundFx.HullAlarm);
        }
        private void DisableHullAlarm()
        {
            Debug.Log("DisableHullAlarm");

			alarmLight.gameObject.SetActive(false);

            hullAlarmEnabled = false;
            hullAlarmSequence.Pause().Rewind();

            AudioManager.instance.Play(AudioBank.MUS_NONE, this.gameObject);
            AudioManager.instance.Play(AudioBank.SFX_HEALTH_UP, this.gameObject);
            //SoundFxsManager.instance.StopLooped("HullAlarm");
        }
        public void DisableAlarms()
        {
            //SoundFxsManager.instance.StopLooped("ShieldAlarm");
            //SoundFxsManager.instance.StopLooped("HullAlarm");
			AudioManager.instance.Play(AudioBank.SFX_HEALTH_UP, this.gameObject);
			AudioManager.instance.Play(AudioBank.SFX_SHIELD_DOWN,this.gameObject);
        }
        private void UpdateAmmoDisplay()
        {
            if (!secondaryWeaponInfiniteAmmo[selectedSecondaryWeapon])
                leftWeaponIndicatorAmmoLabel.text = secondaryWeaponAmmo[selectedSecondaryWeapon].ToString();

            if (!primaryWeaponInfiniteAmmo[selectedPrimaryWeapon])
                rightWeaponIndicatorAmmoLabel.text = primaryWeaponAmmo[selectedPrimaryWeapon].ToString();
        }
        private void UpdateLockingSystem()
        {
            if (lockingSystemOn)
            {
                var screenCenter = ((RectTransform)lockCrosshair.canvas.transform).sizeDelta / 2f;
                var LockAreaLimitHelperPos = RectTransformUtility.WorldToScreenPoint(lockingSystemCamera, ((RectTransform)LockAreaLimitHelper.transform).position);
                var lockAreaRadius = Vector2.Distance(screenCenter, LockAreaLimitHelperPos);

                if (targetBeingLocked != null)
                {
                    if (Vector2.Distance(RectTransformUtility.WorldToScreenPoint(lockingSystemCamera, targetBeingLocked.transform.position), screenCenter) > lockAreaRadius)
                    {
                        targetBeingLocked = null;

                        leftGuidedMissileCannon.target = null;
                        rightGuidedMissileCannon.target = null;
                        lockCrosshair.enabled = false;
                    }
                    else if (Time.time - targetBeingLockedStartTime > timeToLockTarget)
                    {
                        lockCrosshair.color = lockCrosshairLockedColor;
                        leftGuidedMissileCannon.target = targetBeingLocked.transform;
                        rightGuidedMissileCannon.target = targetBeingLocked.transform;
                    }
                }

                if (targetBeingLocked == null)
                {
                    var allEnemies = GameObject.FindObjectsOfType<EnemyShipControllerV2>();
                    float selectedEnemySqrMagnitude = float.MaxValue;

                    foreach (var enemy in allEnemies)
                    {
                        var collider = enemy.GetComponent<Collider>();
                        if (collider == null) continue;

                        if (PhysicsExtensions.IsColliderInSight(targetDetectorOrigin.position, collider, ~targetDetectionLayersToIgnore))
                        {
                            var enemyScreenPoint = RectTransformUtility.WorldToScreenPoint(lockingSystemCamera, enemy.transform.position);
                            if (Vector2.Distance(enemyScreenPoint, screenCenter) > lockAreaRadius) continue;

                            var currSqrMagnitude = (targetDetectorOrigin.position - collider.transform.position).sqrMagnitude;
                            if (currSqrMagnitude < selectedEnemySqrMagnitude)
                            {
                                selectedEnemySqrMagnitude = currSqrMagnitude;
                                targetBeingLocked = enemy;

                                targetBeingLockedStartTime = Time.time;
                                lockCrosshair.enabled = true;
                                lockCrosshair.color = lockCrosshairUnlockedColor;

                                ((RectTransform)lockCrosshair.transform).localScale = new Vector3(.15f, .15f, .15f);
                                ((RectTransform)lockCrosshair.transform).DOScale(new Vector3(.04f, .04f, .04f), timeToLockTarget);

                                lockCrosshair.color = lockCrosshairUnlockedColor.Copy(0);
                                lockCrosshair.DOFade(1f, timeToLockTarget);
                            }
                        }
                    }
                }

                if (targetBeingLocked != null)
                {
                    Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(lockingSystemCamera, targetBeingLocked.transform.position);
                    ((RectTransform)lockCrosshair.transform).anchoredPosition = screenPoint - ((RectTransform)lockCrosshair.canvas.transform).sizeDelta / 2f;
                }
                else
                {
                    lockCrosshair.enabled = false;
                }
            }
        }
        private void TurnLockingSystemOn()
        {
            lockArea.enabled = true;
            lockingSystemOn = true;
        }
        private void TurnLockingSystemOff()
        {
            targetBeingLocked = null;
            lockCrosshair.enabled = false;
            lockArea.enabled = false;
            lockingSystemOn = false;
        }
        private void UpdateShieldAutoRecovery()
        {
            if (shieldLevel < autoRecoveryShieldUntil && !shieldsDown && Time.time - lastShieldAutoRecoveryTime > autoRecoveryFrequency)
            {
                lastShieldAutoRecoveryTime = Time.time;
                shieldLevel += autoRecoveryValue;
            }
        }
        private void UpdateGauge(Image gaugeImage, float value)
        {
            gaugeImage.sprite = levelGaugeSprites[Mathf.CeilToInt((17f * value) / 100f)];
        }
        private void UpdateShoot(PlayerInput input)
        {
            var primaryWeaponFired = false;
            var secondaryWeaponFired = false;

            if (primaryWeaponInfiniteAmmo[selectedPrimaryWeapon] || primaryWeaponAmmo[selectedPrimaryWeapon] > 0)
            {
                if (selectedLeftPrimaryWeapon != null)
                {
                    if (input.ShootPrimaryWeapon)
                        if (selectedLeftPrimaryWeapon.Shoot())
                            primaryWeaponFired = true;
                }

                if (selectedRightPrimaryWeapon != null)
                {
                    if (input.ShootPrimaryWeapon)
                        if (selectedRightPrimaryWeapon.Shoot())
                            primaryWeaponFired = true;
                }
            }
            else if (input.ShootPrimaryWeapon)
            {
                PlayDepletedSoundFx();
            }

            if (secondaryWeaponInfiniteAmmo[selectedSecondaryWeapon] || secondaryWeaponAmmo[selectedSecondaryWeapon] > 0)
            {
                if (selectedLeftSecondaryWeapon != null)
                {
                    if (input.ShootSecondaryWeapon)
                        if (selectedLeftSecondaryWeapon.Shoot())
                            secondaryWeaponFired = true;
                }

                if (selectedRightSecondaryWeapon != null)
                {
                    if (input.ShootSecondaryWeapon)
                        if (selectedRightSecondaryWeapon.Shoot())
                            secondaryWeaponFired = true;
                }
            }
            else if (input.ShootSecondaryWeapon)
            {
                PlayDepletedSoundFx();
            }

            if (primaryWeaponFired && !primaryWeaponInfiniteAmmo[selectedPrimaryWeapon])
            {
                primaryWeaponAmmo[selectedPrimaryWeapon]--;
            }

            if (secondaryWeaponFired && !secondaryWeaponInfiniteAmmo[selectedSecondaryWeapon])
            {
                secondaryWeaponAmmo[selectedSecondaryWeapon]--;
            }
        }
        private void UpdateCannonAim()
        {
            Vector3? targetPosition = null;

            RaycastHit straightHit;
            if (Physics.Raycast(targetDetectorOrigin.position, targetDetectorOrigin.forward, out straightHit, ~targetDetectionLayersToIgnore))
            {
                if (enemyLayerMask.ContainsLayer(straightHit.collider.gameObject.layer))
                {
                    targetPosition = straightHit.transform.position;
                }
            }

            if (!targetPosition.HasValue)
            {
                var hits =
                    PhysicsExtensions.RaycastCilinderWithEightRays(
                        targetDetectorOrigin.position,
                        targetDetectorOrigin.position + targetDetectorOrigin.forward * 10f,
                        targetDetectionRadius,
                        ~targetDetectionLayersToIgnore,
                        debugTargetDetection,
                        debugTargetDetection
                    );

                foreach (var hit in hits)
                {
                    if (enemyLayerMask.ContainsLayer(hit.collider.gameObject.layer))
                    {
                        if (debugTargetDetection)
                        {
                            Debug.DrawLine(targetDetectorOrigin.position, hit.point, Color.red);
                        }

                        targetPosition = hit.transform.position;
                        break;
                    }
                }
            }

            if (targetPosition.HasValue)
            {
                AimCannonsToPosition(targetPosition.Value);
                enemyOnTarget = true;

                if (debugTargetDetection)
                {
                    Debug.DrawLine(targetDetectorOrigin.position, targetPosition.Value, Color.red);
                }
            }
            else
            {
                AimCannonsForward();
                enemyOnTarget = false;
            }

            crosshair.color = currentCrosshairColor;
            primaryWeaponLeftCannonAttachment.transform.rotation = currentPrimaryWeaponLeftCannonRotation;
            primaryWeaponRightCannonAttachment.transform.rotation = currentPrimaryWeaponRightCannonRotation;
            secondaryWeaponLeftCannonAttachment.transform.rotation = currentSecondaryWeaponLeftCannonRotation;
            secondaryWeaponRightCannonAttachment.transform.rotation = currentSecondaryWeaponRightCannonRotation;
        }
        private void AimCannonsToPosition(Vector3 position)
        {
            currentPrimaryWeaponLeftCannonRotation =
                Quaternion.Lerp(
                    currentPrimaryWeaponLeftCannonRotation,
                    Quaternion.LookRotation(position - primaryWeaponLeftCannonAttachment.transform.position),
                    Time.deltaTime * cannonAutoAimVelocity
                );

            currentPrimaryWeaponRightCannonRotation =
                Quaternion.Lerp(
                    currentPrimaryWeaponRightCannonRotation,
                    Quaternion.LookRotation(position - primaryWeaponRightCannonAttachment.transform.position),
                    Time.deltaTime * cannonAutoAimVelocity
                );

            currentSecondaryWeaponLeftCannonRotation =
                Quaternion.Lerp(
                    currentSecondaryWeaponLeftCannonRotation,
                    Quaternion.LookRotation(position - secondaryWeaponLeftCannonAttachment.transform.position),
                    Time.deltaTime * cannonAutoAimVelocity
                );

            currentSecondaryWeaponRightCannonRotation =
                Quaternion.Lerp(
                    currentSecondaryWeaponRightCannonRotation,
                    Quaternion.LookRotation(position - secondaryWeaponRightCannonAttachment.transform.position),
                    Time.deltaTime * cannonAutoAimVelocity
                );

            currentCrosshairColor = Color.Lerp(currentCrosshairColor, crosshairOnTargetColor, Time.deltaTime * crosshairChangeColorVelocity);
        }
        private void AimCannonsForward()
        {
            currentPrimaryWeaponLeftCannonRotation = Quaternion.Lerp(currentPrimaryWeaponLeftCannonRotation, transform.rotation, Time.deltaTime * cannonAutoAimVelocity);
            currentPrimaryWeaponRightCannonRotation = Quaternion.Lerp(currentPrimaryWeaponRightCannonRotation, transform.rotation, Time.deltaTime * cannonAutoAimVelocity);
            currentSecondaryWeaponLeftCannonRotation = Quaternion.Lerp(currentSecondaryWeaponLeftCannonRotation, transform.rotation, Time.deltaTime * cannonAutoAimVelocity);
            currentSecondaryWeaponRightCannonRotation = Quaternion.Lerp(currentSecondaryWeaponRightCannonRotation, transform.rotation, Time.deltaTime * cannonAutoAimVelocity);

            currentCrosshairColor = Color.Lerp(currentCrosshairColor, crosshairColor, Time.deltaTime * crosshairChangeColorVelocity);
        }
        private void UpdateWeaponChange(PlayerInput input)
        {
            if (input.CiclePrimaryWeapon)
            {
                if (selectedPrimaryWeapon == PrimaryWeapon.PlasmaGun)
                {
                    SelectPrimaryWeapon(input, PrimaryWeapon.Laser);
                }
                else if (selectedPrimaryWeapon == PrimaryWeapon.Laser)
                {
                    SelectPrimaryWeapon(input, PrimaryWeapon.PlasmaGun);
                }
            }

            if (input.CicleSecondaryWeapon)
            {
                if (selectedSecondaryWeapon == SecondaryWeapon.UnguidedMissiles)
                {
                    SelectSecondaryWeapon(input, SecondaryWeapon.GuidedMissiles);
                }
                else if (selectedSecondaryWeapon == SecondaryWeapon.GuidedMissiles)
                {
                    SelectSecondaryWeapon(input, SecondaryWeapon.UnguidedMissiles);
                }
            }

            if (input.SelectPlasmaGun)
            {
                SelectPrimaryWeapon(input, PrimaryWeapon.PlasmaGun);
            }

            if (input.SelectLaser)
            {
                SelectPrimaryWeapon(input, PrimaryWeapon.Laser);
            }

            if (input.SelectUnguidedMissiles)
            {
                SelectSecondaryWeapon(input, SecondaryWeapon.UnguidedMissiles);
            }

            if (input.SelectGuidedMissiles)
            {
                SelectSecondaryWeapon(input, SecondaryWeapon.GuidedMissiles);
            }
        }
        private void SelectPrimaryWeapon(PlayerInput input, PrimaryWeapon weapon, bool silent = false)
        {
            if (weapon == selectedPrimaryWeapon)
            {
                //Debug.LogFormat("Primary weapon '{0}' already selected.", weapon);
                return;
            }

            if (selectedLeftPrimaryWeapon != null)
                selectedLeftPrimaryWeapon.OnDeselect();

            if (selectedRightPrimaryWeapon != null)
                selectedRightPrimaryWeapon.OnDeselect();

            switch (weapon)
            {
                case PrimaryWeapon.None:
                    selectedLeftPrimaryWeapon = null;
                    selectedRightPrimaryWeapon = null;
                    rightWeaponIndicatorIcon.enabled = false;
                    rightWeaponIndicatorNoneIcon.enabled = true;
                    rightWeaponIndicatorInfiniteIcon.enabled = false;
                    rightWeaponIndicatorAmmoLabel.enabled = false;
                    break;

                case PrimaryWeapon.PlasmaGun:
                    selectedLeftPrimaryWeapon = leftPlasmaGun;
                    selectedRightPrimaryWeapon = rightPlasmaGun;
                    rightWeaponIndicatorIcon.sprite = plasmaGunIcon;
                    rightWeaponIndicatorIcon.enabled = true;
                    rightWeaponIndicatorNoneIcon.enabled = false;
                    rightWeaponIndicatorInfiniteIcon.enabled = true;
                    rightWeaponIndicatorAmmoLabel.enabled = false;
                    break;

                case PrimaryWeapon.Laser:
                    selectedLeftPrimaryWeapon = leftLaser;
                    selectedRightPrimaryWeapon = rightLaser;
                    rightWeaponIndicatorIcon.sprite = laserIcon;
                    rightWeaponIndicatorIcon.enabled = true;
                    rightWeaponIndicatorNoneIcon.enabled = false;
                    rightWeaponIndicatorInfiniteIcon.enabled = false;
                    rightWeaponIndicatorAmmoLabel.enabled = true;
                    break;

                default:
                    throw new Exception("Unexpected PrimaryWeapon.");
            }

            if (selectedLeftPrimaryWeapon != null)
                selectedLeftPrimaryWeapon.OnSelect(input);

            if (selectedRightPrimaryWeapon != null)
                selectedRightPrimaryWeapon.OnSelect(input);

            selectedPrimaryWeapon = weapon;
        }
        private void SelectSecondaryWeapon(PlayerInput input, SecondaryWeapon weapon, bool silent = false)
        {
            if (weapon == selectedSecondaryWeapon)
            {
                //Debug.LogFormat("Secondary weapon '{0}' already selected.", weapon);
                return;
            }

            if (selectedLeftSecondaryWeapon != null)
                selectedLeftSecondaryWeapon.OnDeselect();

            if (selectedRightSecondaryWeapon != null)
                selectedRightSecondaryWeapon.OnDeselect();

            switch (weapon)
            {
                case SecondaryWeapon.None:
                    selectedLeftSecondaryWeapon = null;
                    selectedRightSecondaryWeapon = null;
                    leftWeaponIndicatorIcon.enabled = false;
                    leftWeaponIndicatorNoneIcon.enabled = true;
                    leftWeaponIndicatorInfiniteIcon.enabled = false;
                    leftWeaponIndicatorAmmoLabel.enabled = false;
                    TurnLockingSystemOff();
                    break;

                case SecondaryWeapon.UnguidedMissiles:
                    selectedLeftSecondaryWeapon = leftUnguidedMissileCannon;
                    selectedRightSecondaryWeapon = rightUnguidedMissileCannon;
                    leftWeaponIndicatorIcon.sprite = unguidedMissilesIcon;
                    leftWeaponIndicatorIcon.enabled = true;
                    leftWeaponIndicatorNoneIcon.enabled = false;
                    leftWeaponIndicatorInfiniteIcon.enabled = false;
                    leftWeaponIndicatorAmmoLabel.enabled = true;
                    TurnLockingSystemOff();
                    break;

                case SecondaryWeapon.GuidedMissiles:
                    selectedLeftSecondaryWeapon = leftGuidedMissileCannon;
                    selectedRightSecondaryWeapon = rightGuidedMissileCannon;
                    leftWeaponIndicatorIcon.sprite = guidedMissilesIcon;
                    leftWeaponIndicatorIcon.enabled = true;
                    leftWeaponIndicatorNoneIcon.enabled = false;
                    leftWeaponIndicatorInfiniteIcon.enabled = false;
                    leftWeaponIndicatorAmmoLabel.enabled = true;
                    TurnLockingSystemOn();
                    break;

                default:
                    throw new Exception("Unexpected PrimaryWeapon.");
            }

            if (selectedLeftSecondaryWeapon != null)
                selectedLeftSecondaryWeapon.OnSelect(input);

            if (selectedRightSecondaryWeapon != null)
                selectedRightSecondaryWeapon.OnSelect(input);

            selectedSecondaryWeapon = weapon;
        }
        private void PlayDepletedSoundFx()
        {
            if (Time.time - lastTimeDepletedAudioWasPlayed > depletedAudioPlayInterval)
            {
                lastTimeDepletedAudioWasPlayed = Time.time;
                //SoundFxsManager.instance.PlayOneShot2D(SoundFx.WeaponDepletedBuzz);
            }
        }

        public bool IsShieldAtMaxValue
        {
            get
            {
                return shieldLevel == 100f;
            }
        }
        public void IncreaseShield(float value)
        {
            shieldLevel += value;
            AkSoundEngine.SetRTPCValue("Shield", shieldLevel);

            if (shieldLevel > 100)
                shieldLevel = 100;
        }

        public bool IsHullAtMaxValue
        {
            get
            {
                return hullLevel == 100f;
            }
        }
        public void IncreaseHull(float value)
        {
            hullLevel += value;
            AkSoundEngine.SetRTPCValue("Health", hullLevel);

            if (hullLevel > 100)
                hullLevel = 100;
        }

        public void IncreaseLaserAmmo(int amount)
        {
            primaryWeaponAmmo[PrimaryWeapon.Laser] += amount;
        }
        public void IncreaseUnguidedMissilesAmmo(int amount)
        {
            secondaryWeaponAmmo[SecondaryWeapon.UnguidedMissiles] += amount;
        }
        public void IncreaseGuidedMissilesAmmo(int amount)
        {
            secondaryWeaponAmmo[SecondaryWeapon.GuidedMissiles] += amount;
        }

        public void OnHitByWeapon(float damage, bool shotByPlayer)
        {
            hitMaskOverlay.color = hitMaskOverlayOriginalColor;
            hitMaskOverlay.DOFade(0, hitMaskOverlayDurationPerHit);
            AudioManager.instance.Play(AudioBank.SFX_SHIP_IMPACT, this.gameObject);

            if (shieldLevel > 0)
            {
                shieldLevel -= damage;
				AkSoundEngine.SetRTPCValue("Shield", shieldLevel);

                if (shieldLevel < 0)
                    shieldLevel = 0;
            }
            else
            {   
				AudioManager.instance.Play(AudioBank.MUS_DANGER, this.gameObject);
                hullLevel -= damage;
                AkSoundEngine.SetRTPCValue("Health", hullLevel);


                if (hullLevel < 0)
                    hullLevel = 0;
            }

            if (hullLevel == 0)
            {
                AudioManager.instance.Play(AudioBank.SFX_STOP_ALL, this.gameObject);


                GameManager.instance.KillPlayer();
            }
        }
    }
}