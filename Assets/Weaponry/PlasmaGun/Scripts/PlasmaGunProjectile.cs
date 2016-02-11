using Ascent.Managers.Audio;
using Ascent.Managers.Pools;
using UnityEngine;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class PlasmaGunProjectile : PontualDamageProjectile<PlasmaGunProjectile, PlasmaGunExplosion, ExplosionDecal>
    {
        public bool isEnemy = false;
        public override void OnSpawn()
        {
            base.OnSpawn();
            //SoundFxsManager.instance.PlayOneShot(SoundFx.PlasmaGunShot, transform.position);
        }
    }
}