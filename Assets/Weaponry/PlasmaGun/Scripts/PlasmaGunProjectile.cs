using Ascent.Managers.Audio;
using Ascent.Managers.Pools;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class PlasmaGunProjectile : PontualDamageProjectile<PlasmaGunProjectile, PlasmaGunExplosion, ExplosionDecal>
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            AudioManager.instance.Play(AudioBank.SFX_FIRE_PLASMA_GUN, this.gameObject);
            //SoundFxsManager.instance.PlayOneShot(SoundFx.PlasmaGunShot, transform.position);
        }
    }
}