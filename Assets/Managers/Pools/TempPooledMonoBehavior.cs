using Ascent.Managers.Game;
using Ascent.Managers.Pools;
using UnityEngine;

namespace Ascent.Managers
{
    public class TempPooledMonoBehavior<PoolType> 
        : MonoBehaviour, IPoolOnSpawnListener
        where PoolType : Component
    {
        public float lifeTime = 0.1f;

        private float spawnTime;

        protected virtual void Start()
        {
            PauseManager.OnUnpause += Unpause;
        }

        protected virtual void Unpause(float pauseDeltaTime)
        {
            spawnTime += pauseDeltaTime;
        }

        public virtual void OnSpawn()
        {
            spawnTime = Time.time;
        }

        protected virtual void Update()
        {
            if (!PauseManager.instance.paused && Time.time - spawnTime > lifeTime)
            {
                ReturnToPool();
            }
        }

        protected void ReturnToPool()
        {
            PoolManager.instance.Despawn<PoolType>((PoolType)(object)this);
        }
    }
}
