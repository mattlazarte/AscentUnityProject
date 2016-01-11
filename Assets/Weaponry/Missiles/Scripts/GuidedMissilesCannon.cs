using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;

namespace Ascent.Weaponry
{
    public class GuidedMissilesCannon : RadialDamageProjectileCannon<GuidedMissileProjectile>
    {
        public Transform target;

        protected virtual void Start()
        {
            this.OnProjectileLaunch += GuidedMissilesCannon_OnProjectileLaunch;
        }

        protected void GuidedMissilesCannon_OnProjectileLaunch(IProjectile obj)
        {
            ((GuidedMissileProjectile)obj).target = target;
        }
    }
}
