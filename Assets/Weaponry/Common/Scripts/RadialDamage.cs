using Ascent.Enemies;
using Ascent.Managers;
using Ascent.Managers.Pools;
using Ascent.Weaponry;
using UnityEngine;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class RadialDamage : TempPooledMonoBehavior<RadialDamage>
    {
        public float damageRadius = 3f;
        public float maxExplosionDamage = 5f;
        public float maxForceToApply = 10f;
        public LayerMask enemiesLayer;

        protected bool doRadialDamageOnNextFrame = false;

        public override void OnSpawn()
        {
            base.OnSpawn();
            doRadialDamageOnNextFrame = true;
        }

        protected override void Update()
        {
            base.Update();

            if (doRadialDamageOnNextFrame)
            {
                doRadialDamageOnNextFrame = false;
                DoRadialDamage();
            }
        }

        private void DoRadialDamage()
        {
            var enemies = Physics.OverlapSphere(transform.position, damageRadius, enemiesLayer);

            foreach (var enemy in enemies)
            {
                var enemyController = enemy.GetComponent<EnemyShipControllerV2>();
                if (enemyController == null)
                    Debug.LogWarningFormat("Enemy hit by RadialDamage does not contain a EnemyShipControllerV2 component: {0}", enemy.name);

                var enemyRigidbody = enemy.GetComponentInChildren<Rigidbody>();
                if (enemyRigidbody == null)
                    Debug.LogWarningFormat("Enemy hit by RadialDamage does not contain a Rigidbody component: {0}", enemy.name);

                var diffVector = enemy.transform.position - transform.position;
                var distance = diffVector.magnitude;

                var distanceMultiplier = Mathf.Clamp(1 - (distance / damageRadius), 0f, 1f);
                var damage = distanceMultiplier * maxExplosionDamage;
                var forceToApply = distanceMultiplier * maxForceToApply;

                enemyRigidbody.AddForce(diffVector.normalized * forceToApply);
                enemyController.TakeDamage(damage);

                //Debug.LogFormat(
                //    "RadialDamage dealt {0} damage on {1}. distance = {2}, multiplier = {3}, damageRadius = {4}, maxExplosionDamage = {5}", 
                //    damage, enemy.name, distance, distanceMultiplier, damageRadius, maxExplosionDamage);
            }
        }
    }
}