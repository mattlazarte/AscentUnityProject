using Ascent.Managers;
using Ascent.Managers.Audio;
using Ascent.Managers.Pools;

namespace Ascent.Enemies
{
    [IncludeInPoolManager]
    public class EnemyExplosion01 : TempPooledMonoBehavior<EnemyExplosion01>
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            //SoundFxsManager.instance.PlayOneShot(SoundFx.ShipExplosion, transform.position);
        }
    }
}