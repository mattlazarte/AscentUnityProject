using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Game;
using System.Collections.Generic;
using Ascent.Managers;

namespace Ascent.Weaponry
{
    public abstract class RadialDamageProjectile<PoolType, ExplosionType, DecalType>
        : BasicProjectile<PoolType, DecalType>, IRadialDamageProjectile
        where PoolType : Component
        where ExplosionType : Component
        where DecalType : Component
    {
        public float damageRadius { get; set; }
        public float maxExplosionDamage { get; set; }
        public float maxForceToApply { get; set; }
        public LayerMask enemiesLayer { get; set; }

        protected override void OnHit(RaycastHit raycastHit)
        {
            var IHittableComponents = raycastHit.collider.gameObject.GetComponents<IHittableObject>();
            foreach (var IHittableComponent in IHittableComponents)
            {
                IHittableComponent.OnHitByWeapon(0f, shotByPlayer);
            }

            var radialDamage = PoolManager.instance.Spawn<RadialDamage>(null, transform);
            radialDamage.damageRadius = damageRadius;
            radialDamage.maxExplosionDamage = maxExplosionDamage;
            radialDamage.maxForceToApply = maxForceToApply;
            radialDamage.enemiesLayer = enemiesLayer;

            PoolManager.instance.Spawn<ExplosionType>(null, transform.position, Quaternion.identity);
        }
    }
}
