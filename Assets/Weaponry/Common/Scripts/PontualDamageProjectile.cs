using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Game;
using System.Collections.Generic;
using Ascent.Managers;

namespace Ascent.Weaponry
{
    public abstract class PontualDamageProjectile<PoolType, ExplosionType, DecalType>
        : BasicProjectile<PoolType, DecalType>, IPontualDamageProjectile
        where PoolType : Component
        where ExplosionType : Component
        where DecalType : Component
    {
        public float forceToApplyOnObject { get; set; }
        public float damageDealt { get; set; }
    
        protected override void OnHit(RaycastHit raycastHit)
        {
            var IHittableComponents = raycastHit.collider.gameObject.GetComponents<IHittableObject>();
            foreach (var IHittableComponent in IHittableComponents)
            {
                IHittableComponent.OnHitByWeapon(damageDealt, shotByPlayer);
            }
    
            if (forceToApplyOnObject > 0)
            {
                var hitObjRigidBody = raycastHit.collider.gameObject.GetComponent<Rigidbody>();
                if (hitObjRigidBody != null && !hitObjRigidBody.isKinematic)
                {
                    hitObjRigidBody.AddForceAtPosition(transform.forward * forceToApplyOnObject, raycastHit.point, ForceMode.VelocityChange);
                }
            }
    
            var explosion = PoolManager.instance.Spawn<ExplosionType>(null, lastPosition, Quaternion.identity);
            SetupExplosion(explosion);
        }
    
        protected virtual void SetupExplosion(ExplosionType explosion) { }
    }
}
