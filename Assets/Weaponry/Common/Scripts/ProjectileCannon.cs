using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;

namespace Ascent.Weaponry
{
    public abstract class ProjectileCannon<ProjectileType> 
        : IntervalCannon, IProjectileCannon
        where ProjectileType : Component, IProjectile
    {
        public event Action<IProjectile> OnProjectileLaunch;

        public bool attachedToPlayer;
        public string[] tagsToIgnore;
    
        protected override void IntervalShoot()
        {
            var projectile = PoolManager.instance.Spawn<ProjectileType>(null, transform);
            projectile.SetTagsToIgnore(tagsToIgnore);
            projectile.shotByPlayer = attachedToPlayer;

            if (projectile.GetComponentInChildren<PlasmaGunProjectile>() != null)
            {
                if (projectile.shotByPlayer)
                    AudioManager.instance.Play(AudioBank.SFX_FIRE_PLASMA_GUN, projectile.gameObject);
                else
                    AudioManager.instance.Play(AudioBank.SFX_ENEMY_FIRE, projectile.gameObject);
            }   

            InternalSetupShoot(projectile);

            if (OnProjectileLaunch != null)
                OnProjectileLaunch(projectile);
        }
        protected abstract void InternalSetupShoot(ProjectileType projectile);
    }
}
