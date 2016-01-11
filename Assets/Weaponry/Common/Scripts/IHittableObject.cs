using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;

namespace Ascent.Weaponry
{
    public interface IHittableObject
    {
        void OnHitByWeapon(float damage, bool shotByPlayer);
    }
}
