using UnityEngine;
using System.Collections;
using Ascent.Managers.Pools;
using System;
using Ascent.Managers.Input;

namespace Ascent.Weaponry
{
    public abstract class Cannon : MonoBehaviour
    {
        public virtual void OnSelect(PlayerInput input)
        {
            gameObject.SetActive(true);
        }
    
        public virtual void OnDeselect()
        {
            gameObject.SetActive(false);
        }

        public virtual bool Shoot() { return false; }
    }
}
