using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;

namespace Ascent.Weaponry
{
    public abstract class RadialDamageProjectileCannon<ProjectileType>
        : ProjectileCannon<ProjectileType>
        where ProjectileType : Component, IProjectile
    {
        public float damageRadius = 3f;
        public float maxExplosionDamage = 5f;
        public float maxForceToApply = 10f;
        public LayerMask enemiesLayer;
    
        protected override void InternalSetupShoot(ProjectileType projectile)
        {
            ((IRadialDamageProjectile)projectile).damageRadius = damageRadius;
            ((IRadialDamageProjectile)projectile).maxExplosionDamage = maxExplosionDamage;
            ((IRadialDamageProjectile)projectile).maxForceToApply = maxForceToApply;
            ((IRadialDamageProjectile)projectile).enemiesLayer = enemiesLayer;
        }
    }
}
