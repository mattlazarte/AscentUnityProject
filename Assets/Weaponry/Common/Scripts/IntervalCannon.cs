using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;

namespace Ascent.Weaponry
{
    public abstract class IntervalCannon : Cannon
    {
        public float fireInterval = 0.3f;
        private float lastFireTime;

        public override void OnSelect(PlayerInput input)
        {
            base.OnSelect(input);
        }

        public override bool Shoot()
        {
            if (Time.time - lastFireTime >= fireInterval)
            {
                lastFireTime = Time.time;
                IntervalShoot();
                return true;
            }

            return false;
        }
    
        protected abstract void IntervalShoot();
    }
}
