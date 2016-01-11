using Ascent.Items;
using Ascent.Managers.Pools;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ascent.PlayerShip.Radar
{
    public class RadarController : MonoBehaviour
    {
        public LayerMask enemyLayerMask;
        public LayerMask itemsLayerMask;
        public LayerMask ignoreOnSightTest;
        public float radius = 10f;
        public float scale = 0.01f;

        private List<RadarEnemyShipRepresentation> Enemies;
        private List<RadarItemRepresentation> Items;

        private void Start()
        {
            Enemies = new List<RadarEnemyShipRepresentation>();
            Items = new List<RadarItemRepresentation>();
        }

        private void Update()
        {
            CleanLastFrameItems();
            CleanLastFrameEnemies();

            DetectAndShowItems();
            DetectAndShowEnemies();
        }

        private void CleanLastFrameEnemies()
        {
            foreach (var enemy in Enemies)
                PoolManager.instance.Despawn<RadarEnemyShipRepresentation>(enemy);

            Enemies.Clear();
        }

        private void DetectAndShowEnemies()
        {
            var colliders = Physics.OverlapSphere(transform.position, radius, enemyLayerMask);

            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];

                if (!PhysicsExtensions.IsColliderInSight(transform.position, collider, ~ignoreOnSightTest))
                    continue;

                var diffVector = collider.transform.position - transform.position;
                var enemyPosition = transform.position + (diffVector * scale);

                var enemy = PoolManager.instance.Spawn<RadarEnemyShipRepresentation>(transform, enemyPosition, collider.transform.rotation);
                Enemies.Add(enemy);
            }
        }

        private void CleanLastFrameItems()
        {
            foreach (var item in Items)
                PoolManager.instance.Despawn<RadarItemRepresentation>(item);

            Items.Clear();
        }

        private void DetectAndShowItems()
        {
            var colliders = Physics.OverlapSphere(transform.position, radius, itemsLayerMask);

            for (var i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];

                if (!PhysicsExtensions.IsColliderInSight(transform.position, collider, ~ignoreOnSightTest))
                    continue;

                var diffVector = collider.transform.position - transform.position;
                var itemPosition = transform.position + (diffVector * scale);

                var itemComponent = (Item)collider.GetComponent<Item>();
                if (itemComponent == null)
                    throw new Exception("Item component not found on object.");

                var item = PoolManager.instance.Spawn<RadarItemRepresentation>(transform, itemPosition, collider.transform.rotation);
                item.SetColor(itemComponent.colorOnRadar);
                Items.Add(item);
            }
        }
    }
}