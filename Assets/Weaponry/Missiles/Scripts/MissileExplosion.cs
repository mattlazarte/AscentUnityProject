using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers;
using Ascent.Managers.Audio;

namespace Ascent.Weaponry
{
    [IncludeInPoolManager]
    public class MissileExplosion : TempPooledMonoBehavior<MissileExplosion>
    {
        public override void OnSpawn()
        {
            base.OnSpawn();
            SoundFxsManager.instance.PlayOneShot(SoundFx.MissileExplosion, transform.position);
        }
    }
}
