using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascent.Managers.Pools
{
    public interface IPoolOnInstantiateListener
    {
        void OnInstantiate();
    }
}
