using System;
using Ascent.Weaponry;
using UnityEngine;

namespace Ascent.Weaponry
{
    public interface IRadialDamageProjectile : IProjectile
    {
        float damageRadius { get; set; }
        float maxExplosionDamage { get; set; }
        float maxForceToApply { get; set; }
        LayerMask enemiesLayer { get; set; }
    }
}
