using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Audio;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class GuidedMissileProjectile : RadialDamageProjectile<GuidedMissileProjectile, MissileExplosion, ExplosionDecal>
    {
        public Transform target;
        public float alignSpeed = 1f;

        public override void OnSpawn()
        {
            base.OnSpawn();
            //SoundFxsManager.instance.PlayOneShot(SoundFx.MissileShot, transform.position);
        }
    
        protected override void SetupDecal(ExplosionDecal decal)
        {
            decal.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        protected override void MoveProjectile()
        {
            base.MoveProjectile();

            if (target != null)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), Time.deltaTime * alignSpeed);
            }
        }
    }
}
