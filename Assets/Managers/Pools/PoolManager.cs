using Ascent.Managers.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ascent.Managers.Pools
{
    [Serializable]
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager instance;

        public class Pool
        {
            public PoolSettings Settings;
            public Transform ParentFolder;
            public Queue Queue;
            public int Counter;
            public int LateInstantiations;

            public Pool(PoolSettings settings, Transform parentFolder)
            {
                this.Settings = settings;
                this.ParentFolder = parentFolder;
                this.Queue = new Queue();
                this.Counter = 0;
                this.LateInstantiations = 0;
            }
        }

        [SerializeField]
        public List<PoolSettings> poolSettings;

        [NonSerialized]
        public Dictionary<Type, Pool> pools;

        protected void Awake()
        {
            instance = this;

            DontDestroyOnLoad(gameObject);

            // Only refresh on start when inside editor.
#           if UNITY_EDITOR
            RefreshIncludedTypes();
#           endif

            InitPools();
        }

        public T Spawn<T>(Transform parent) where T : Component
        {
            return Spawn<T>(parent, parent.position, parent.rotation);
        }

        public T Spawn<T>(Transform parent, Transform spawnTransform) where T : Component
        {
            return Spawn<T>(parent, spawnTransform.position, spawnTransform.rotation);
        }

        public T Spawn<T>(Transform parent, Vector3 position, Quaternion rotation) where T : Component
        {
            var newObjParent = parent;
            if (newObjParent == null && GameManager.instance != null && GameManager.instance.gameRoot != null)
            {
                newObjParent = GameManager.instance.gameRoot.transform;
            }

            var requiredType = typeof(T);
            var pool = GetPoolFromType(requiredType);
            T obj = null;

            if (pool.Queue.Count == 0)
            {
                Debug.LogWarningFormat("Runtime instantiation on pool '{0}'. Consider to grow the initial pool size.", requiredType.Name);
                pool.LateInstantiations++;

                var newObj = (GameObject)Instantiate(pool.Settings.Prefab, position, rotation);

                pool.Counter++;
                newObj.name = requiredType.Name + pool.Counter.ToString("_000000");
                newObj.transform.parent = newObjParent;

                var newObjComponent = newObj.GetComponent(requiredType);
                if (newObjComponent == null)
                    throw new Exception("Runtime instantiation for pool '" + requiredType.Name + "' fail. Could not get component from prefab.");

                obj = (T)newObjComponent;
            }
            else
            {
                obj = (T)pool.Queue.Dequeue();
                obj.transform.parent = newObjParent;
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.gameObject.SetActive(true);
            }

            if (typeof(IPoolOnSpawnListener).IsAssignableFrom(requiredType))
            {
                ((IPoolOnSpawnListener)obj).OnSpawn();
            }

            return obj;
        }

        public void Despawn<T>(T obj) where T : Component
        {
            var requiredType = typeof(T);
            var pool = GetPoolFromType(requiredType);

            obj.gameObject.SetActive(false);
            obj.transform.parent = pool.ParentFolder;
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;

            if (typeof(IPoolOnDespawnListener).IsAssignableFrom(requiredType))
            {
                ((IPoolOnDespawnListener)obj).OnDespawn();
            }

            pool.Queue.Enqueue(obj);
        }

        public void RefreshIncludedTypes()
        {
            if (poolSettings == null)
                poolSettings = new List<PoolSettings>();

            var typesWithPoolAttr = GetPoolAttributedTypes();
            RemoveTheTypesThatHaveHadTheirPoolAttributeRemoved(typesWithPoolAttr);
            AddTypesThatHaveHadPoolAttributeAdded(typesWithPoolAttr);
        }

        private Pool GetPoolFromType(Type requiredType)
        {
            if (!pools.ContainsKey(requiredType))
                throw new Exception("There is no pool for type '" + requiredType.Name + "'.");

            var pool = pools[requiredType];

            return pool;
        }

        private void InitPools()
        {
            pools = new Dictionary<Type, Pool>();

            foreach (var poolSetting in poolSettings)
            {
                if (poolSetting.Size <= 0)
                {
                    Debug.LogErrorFormat("Pool '{0}' init error: Size is less or equals to zero.", poolSetting.TypeName);
                    break;
                }

                if (poolSetting.Prefab == null)
                {
                    Debug.LogErrorFormat("Pool '{0}' init error: Prefab is not set.", poolSetting.TypeName);
                    break;
                }

                var poolType = Type.GetType(poolSetting.AssemblyQualifiedTypeName);
                if (poolType == null)
                {
                    Debug.LogErrorFormat("Pool '{0}' init error: Type not found.", poolSetting.TypeName);
                    break;
                }
                var poolTypeImplementsPoolOnDespawnListener = typeof(IPoolOnInstantiateListener).IsAssignableFrom(poolType);

                var poolFolder = new GameObject(poolSetting.TypeName + "_Pool");
                poolFolder.transform.position = transform.position;
                poolFolder.transform.parent = transform;

                var newPool = new Pool(poolSetting, poolFolder.transform);
                newPool.Counter = poolSetting.Size;
                pools.Add(poolType, newPool);

                for (var i = 1; i <= poolSetting.Size; i++)
                {
                    var newObj = (GameObject)Instantiate(poolSetting.Prefab, transform.position, Quaternion.identity);
                    newObj.name = poolSetting.TypeName + i.ToString("_000000");
                    newObj.transform.parent = poolFolder.transform;

                    var newObjComponent = newObj.GetComponent(poolType);
                    if (newObjComponent == null)
                    {
                        pools.Remove(poolType);
                        Destroy(poolFolder);
                        Debug.LogErrorFormat("Pool '{0}' init error: Prefab is missing a '{0}' component.", poolSetting.TypeName);
                        break;
                    }

                    if (poolTypeImplementsPoolOnDespawnListener)
                    {
                        ((IPoolOnInstantiateListener)newObjComponent).OnInstantiate();
                    }

                    newObj.SetActive(false);
                    newPool.Queue.Enqueue(newObjComponent);
                }
            }
        }

        private List<Type> GetPoolAttributedTypes()
        {
            var list = new List<Type>();
            var assemblyTypes = typeof(PoolManager).Assembly.GetTypes();

            foreach (var type in assemblyTypes)
            {
                var poolAttrs = type.GetCustomAttributes(typeof(IncludeInPoolManagerAttribute), false);
                if (poolAttrs.Length != 0)
                {
                    list.Add(type);
                }
            }

            return list;
        }

        private void AddTypesThatHaveHadPoolAttributeAdded(List<Type> typesWithPoolAttr)
        {
            foreach (var type in typesWithPoolAttr)
            {
                var found = false;

                foreach (var pool in poolSettings)
                {
                    if (pool.AssemblyQualifiedTypeName == type.AssemblyQualifiedName)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    var newPool = new PoolSettings();
                    newPool.AssemblyQualifiedTypeName = type.AssemblyQualifiedName;
                    newPool.TypeName = type.Name;
                    poolSettings.Add(newPool);

                    Debug.LogFormat("Type '{0}' was added to PoolManager.", type.Name);
                }
            }
        }

        private void RemoveTheTypesThatHaveHadTheirPoolAttributeRemoved(List<Type> typesWithPoolAttr)
        {
            for (var i = poolSettings.Count - 1; i >= 0; i--)
            {
                var pool = poolSettings[i];
                var found = false;

                foreach (var type in typesWithPoolAttr)
                {
                    if (pool.AssemblyQualifiedTypeName == type.AssemblyQualifiedName)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    poolSettings.Remove(pool);

                    Debug.LogFormat("Type '{0}' was removed from PoolManager", pool.TypeName);
                }
            }
        }
    }
}