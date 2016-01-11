using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using Ascent.Managers;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class PlasmaGunMuzzle : TempPooledMonoBehavior<PlasmaGunMuzzle>
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(45, 135)), Space.Self);
        }
    }
}