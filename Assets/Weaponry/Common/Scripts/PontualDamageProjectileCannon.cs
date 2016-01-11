using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;

namespace Ascent.Weaponry
{
    public abstract class PontualDamageProjectileCannon<ProjectileType>
        : ProjectileCannon<ProjectileType>
        where ProjectileType : Component, IProjectile
    {
        public float projectileDamageDealt = 1f;
    
        protected override void InternalSetupShoot(ProjectileType projectile)
        {
            ((IPontualDamageProjectile)projectile).damageDealt = projectileDamageDealt;
        }
    }
}
