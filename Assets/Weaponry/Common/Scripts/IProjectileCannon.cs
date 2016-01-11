using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;

namespace Ascent.Weaponry
{
    public interface IProjectileCannon
    {
        event Action<IProjectile> OnProjectileLaunch;
    }
}
