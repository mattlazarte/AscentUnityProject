using System;
using Ascent.Weaponry;
using UnityEngine;

namespace Ascent.Weaponry
{
    public interface IProjectile
    {
        void SetTagsToIgnore(string[] tags);
        bool shotByPlayer { get; set; }
    }
}
