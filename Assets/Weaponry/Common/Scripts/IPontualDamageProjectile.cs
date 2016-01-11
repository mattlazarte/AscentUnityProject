using System;
using Ascent.Weaponry;
using UnityEngine;

namespace Ascent.Weaponry
{
    public interface IPontualDamageProjectile : IProjectile
    {
        float forceToApplyOnObject { get; set; }
        float damageDealt { get; set; }
    }
}
