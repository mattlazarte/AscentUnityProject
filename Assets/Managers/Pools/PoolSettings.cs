using System;
using UnityEngine;

namespace Ascent.Managers.Pools
{
    [Serializable]
    public class PoolSettings
    {
        [SerializeField]
        public String TypeName;
    
        [SerializeField]
        public String AssemblyQualifiedTypeName;
    
        [SerializeField]
        public GameObject Prefab;
    
        [SerializeField]
        public int Size;
    }
}
