using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Game;
using System.Collections.Generic;
using Ascent.Managers;

namespace Ascent.Weaponry
{
    public abstract class BasicProjectile<PoolType, DecalType> 
        : TempPooledMonoBehavior<PoolType>, IProjectile
        where PoolType : Component
        where DecalType : Component
    {
        public float speed = 0.5f;
        public string[] tagsToIgnore;
        public LayerMask layersToIgnore;
        public LayerMask layersToPlaceDecals;

        protected Vector3 lastPosition;

        public void SetTagsToIgnore(string[] tags)
        {
            this.tagsToIgnore = tags;
        }

        public bool shotByPlayer { get; set; }

        protected override void Start()
        {
            base.Start();
            lastPosition = transform.position;
        }

        protected abstract void OnHit(RaycastHit raycastHit);
        protected void FixedUpdate()
        {
            if (!PauseManager.instance.paused)
            {
                RaycastHit raycastHit;
                Vector3 direction = (transform.position - lastPosition).normalized;
                if (Physics.Raycast(lastPosition, direction, out raycastHit, 1f, ~layersToIgnore))
                {
                    var ignored = false;
                    if (tagsToIgnore != null)
                    {
                        foreach (var tagToIgnore in tagsToIgnore)
                        {
                            if (raycastHit.collider.tag == tagToIgnore)
                            {
                                ignored = true;
                            }
                        }
                    }

                    if (!ignored)
                    {
                        if (layersToPlaceDecals.ContainsLayer(raycastHit.collider.gameObject.layer))
                        {
                            var decalRotation = Quaternion.FromToRotation(Vector3.forward, raycastHit.normal);
                            var decal = PoolManager.instance.Spawn<DecalType>(null, raycastHit.point, decalRotation);
                            decal.transform.Translate(0, 0, .05f);
                            SetupDecal(decal);
                        }

                        OnHit(raycastHit);

                        ReturnToPool();
                    }
                }

                lastPosition = transform.position;
                MoveProjectile();
            }
        }
        protected virtual void MoveProjectile()
        {
            transform.Translate(new Vector3(0, 0, speed), Space.Self);
        }

        protected virtual void SetupDecal(DecalType decal) { }
    }
}
