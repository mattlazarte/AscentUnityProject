using Ascent.Enemies;
using Ascent.Managers.Pools;
using Ascent.Weaponry;
using UnityEngine;

namespace Ascent
{
    public class ExplodingBarrel : MonoBehaviour, IHittableObject
    {
        public float damageRadius = 3f;
        public float maxExplosionDamage = 5f;
        public float maxForceToApply = 10f;
        public LayerMask enemiesLayer;

        public void OnHitByWeapon(float damage, bool shotByPlayer)
        {
            var radialDamage = PoolManager.instance.Spawn<RadialDamage>(null, transform);
            radialDamage.damageRadius = damageRadius;
            radialDamage.maxExplosionDamage = maxExplosionDamage;
            radialDamage.maxForceToApply = maxForceToApply;
            radialDamage.enemiesLayer = enemiesLayer;

            PoolManager.instance.Spawn<EnemyExplosion01>(null, transform);
            Destroy(gameObject);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}