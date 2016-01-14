using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Audio;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class UnguidedMissileProjectile : RadialDamageProjectile<UnguidedMissileProjectile, MissileExplosion, ExplosionDecal>
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            //SoundFxsManager.instance.PlayOneShot(SoundFx.MissileShot, transform.position);
        }

        protected override void SetupDecal(ExplosionDecal decal)
        {
            decal.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
    }
}