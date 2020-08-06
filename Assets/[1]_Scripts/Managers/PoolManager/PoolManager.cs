using System.Collections.Generic;
using UnityEngine;

namespace SA.Pool
{
    public class PoolManager
    {

        #region Properties

        public bool IsPoolEmpty
        {
            get
            {
                if (pools == null) return true;
                return (pools.Count == 0);
            }
        }

        #endregion


        #region Var

        private static object syncRoot = new object();
        private static PoolManager instance;
        private Transform poolsContainer;
        private Dictionary<int, Pool> pools = new Dictionary<int, Pool>();

        #endregion


        #region Init


        private PoolManager()
        {
            Debug.Log("PoolManager Init...");
        }


        public static PoolManager GetInstance()
        {
            lock (syncRoot)
            {
                if (instance == null)
                {
                    instance = new PoolManager();
                }
            }

            return instance;
        }


        #endregion


        #region Pool work


        public Pool Addpool(PoolType id, bool reparent = true)
        {
            Pool pool;

            //пытаемся получить пул по ключу ID, если нет, создаём его
            if (pools.TryGetValue((int)id, out pool) == false)
            {
                pool = new Pool();
                pools.Add((int)id, pool);

                if (reparent)
                {
                    //создаём контейнер для пулов, ели он не создан
                    if (poolsContainer == null) poolsContainer = new GameObject("[POOLS]").transform;

                    //создаём дочерний пул с данным типом в контейнере
                    var poolGO = new GameObject("Pool:" + id);
                    poolGO.transform.SetParent(poolsContainer);

                    //назначаем новому пулу родительский объект
                    pool.SetParent(poolGO.transform);
                }
            }

            return pool;
        }


        #endregion


        #region Create GameObject

        public GameObject Spawn(PoolType id,
                                    GameObject prefab,
                                        Vector3 position = default(Vector3),
                                            Quaternion rotation = default(Quaternion),
                                                Transform parent = null)
        {
            if (!pools.ContainsKey((int)id))
            {
                Addpool(id);
            }

            return pools[(int)id].Spawn(prefab, position, rotation, parent);
        }


        public void Despawn(PoolType id, GameObject go)
        {
            if (!pools.ContainsKey((int)id))
            {
                Addpool(id);
            }

            pools[(int)id].Despawn(go);
        }

        #endregion


        #region Clear

        public void Dispose()
        {
            for (int i = 0; i < pools.Count; i++)
            {
                if (pools.ContainsKey(i))
                    pools[i].Dispose();
            }

            pools.Clear();
        }

        #endregion

    }
}