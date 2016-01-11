using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class ExplosionDecal : FadeOutDecal<ExplosionDecal>, IPoolOnDespawnListener
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(0, 359)), Space.Self);
        }

        public void OnDespawn()
        {
            transform.localScale = Vector3.one;
        }
    }
}