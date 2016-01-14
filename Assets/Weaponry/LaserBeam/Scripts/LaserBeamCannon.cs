using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;
using Ascent.Managers.Game;
using Ascent.Managers;
using Ascent.Managers.Audio;

namespace Ascent.Weaponry
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserBeamCannon : Cannon
    {
        private LineRenderer laserLine;
        public GameObject hitFlare;
        public GameObject muzzle;

        public LayerMask layersToPlaceDecals;
        public float decalsPlacementFrequency = .1f;
        public float forceToApplyOnObject = 10f;

        public LayerMask ignoreLayer;
        public float damageDealt = .1f;
        public float damageFrequency = 0.5f;
        public float ammoConsumptionIntervalInSeconds = .5f;

        private float beamMaxLength = 100f;
        private float beamScale = 2.6f;
        private float uvOffset = 0;
        private float uvAnimSpeed = -7f;
        private float lastDecalPlacementTime;
        private float lastDamageTime;
        private string audioId = Guid.NewGuid().ToString();
        private float lastAmmoConsumptionTime;
        private bool shootCommandReceived = false;
        
        private void Start()
        {
            if (hitFlare == null)
                throw new Exception("hitFlare is null.");

            if (muzzle == null)
                throw new Exception("muzzle is null.");

            laserLine = GetComponent<LineRenderer>();
            laserLine.SetWidth(.1f, .1f);
            laserLine.enabled = false;
            hitFlare.SetActive(false);
            muzzle.SetActive(false);
        }

        public override void OnSelect(PlayerInput input)
        {
            base.OnSelect(input);

            // I'm calling Start here because it is not being
            // automatically called before OnSelect is called
            // for some reason.
            Start();

            laserLine.enabled = false;
            hitFlare.SetActive(false);
            muzzle.SetActive(false);
        }

        public override void OnDeselect()
        {
            base.OnDeselect();

            if (IsShooting())
            {
                StopLaser();
            }
        }

        public void StartLaser()
        {
            laserLine.enabled = true;
            hitFlare.SetActive(true);
            muzzle.SetActive(true);

            //SoundFxsManager.instance.PlayOneShot(SoundFx.LaserBeamOpen, transform.position);
            //SoundFxsManager.instance.LoopPlay(audioId, SoundFx.LaserBeamLoop, transform);
        }

        public void StopLaser()
        {
            laserLine.enabled = false;
            hitFlare.SetActive(false);
            muzzle.SetActive(false);

            //SoundFxsManager.instance.StopLooped(audioId);
            //SoundFxsManager.instance.PlayOneShot(SoundFx.LaserBeamClose, transform.position);
        }

        public override bool Shoot()
        {
            shootCommandReceived = true;

            if (Time.time - lastAmmoConsumptionTime > ammoConsumptionIntervalInSeconds)
            {
                lastAmmoConsumptionTime = Time.time;
                return true;
            }

            return false;
        }

        public bool IsShooting()
        {
            return laserLine.enabled;
        }

        private void Update()
        {
            if (IsShooting() && !shootCommandReceived)
            {
                StopLaser();
            }
            else if (!IsShooting() && shootCommandReceived)
            {
                StartLaser();
            }

            shootCommandReceived = false;

            if (IsShooting())
            {
                AnimateTextureUV();
                laserLine.SetPosition(0, transform.position);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, float.MaxValue, ~ignoreLayer))
                {
                    laserLine.SetPosition(1, hit.point);
                    var beamLength = Vector3.Distance(transform.position, hit.point);
                    SetTextureScale(beamLength * (beamScale / 10f));

                    hitFlare.transform.position = hit.point;
                    hitFlare.transform.rotation = transform.rotation;
                    hitFlare.transform.Translate(Vector3.forward * -0.1f, Space.Self);

                    if (layersToPlaceDecals.ContainsLayer(hit.collider.gameObject.layer))
                    {
                        if (Time.time - lastDecalPlacementTime >= decalsPlacementFrequency)
                        {
                            lastDecalPlacementTime = Time.time;
                            var decalRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                            var decal = PoolManager.instance.Spawn<ExplosionDecal>(null, hit.point, decalRotation);
                            decal.transform.Translate(0, 0, .05f);
                            decal.transform.localScale = Vector3.one * .3f;
                        }
                    }

                    if (forceToApplyOnObject > 0)
                    {
                        var hitObjRigidBody = hit.collider.gameObject.GetComponent<Rigidbody>();
                        if (hitObjRigidBody != null && !hitObjRigidBody.isKinematic)
                        {
                            hitObjRigidBody.AddForceAtPosition(transform.forward * forceToApplyOnObject, hit.point, ForceMode.VelocityChange);
                        }
                    }

                    if (Time.time - lastDamageTime >= damageFrequency)
                    {
                        var ILaserHittableComponents = hit.collider.gameObject.GetComponents<IHittableObject>();
                        foreach (var ILaserHittableComponent in ILaserHittableComponents)
                        {
                            ILaserHittableComponent.OnHitByWeapon(damageDealt, true);
                            lastDamageTime = Time.time;
                        }
                    }
                }
                else
                {
                    var endPosition = transform.position + transform.rotation * new Vector3(0, 0, beamMaxLength);
                    laserLine.SetPosition(1, endPosition);
                    SetTextureScale(beamMaxLength * (beamScale / 10f));

                    hitFlare.transform.position = endPosition;
                }
            }
        }

        private void SetTextureScale(float scale)
        {
            laserLine.material.SetTextureScale("_MainTex", new Vector2(scale, 1f));
        }

        private void AnimateTextureUV()
        {
            uvOffset += uvAnimSpeed * Time.deltaTime;
            laserLine.material.SetTextureOffset("_MainTex", new Vector2(uvOffset, 0f));
        }
    }
}
