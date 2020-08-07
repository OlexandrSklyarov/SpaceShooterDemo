using UnityEngine;
using SA.Pool;
using System;

public class BuildManager
{
    #region Var

    private readonly static object syncRoot = new object();
    private static BuildManager instance;

    #endregion


    #region Init

    private BuildManager()
    {
        Debug.Log("BuildManager => Construct()");
    }


    public static BuildManager GetInstance()
    {
        lock (syncRoot)
        {
            if (instance == null)
            {
                instance = new BuildManager();
            }
        }

        return instance;
    }


    #endregion


    #region Populate Entityes

    public void PopulateEntitys(PoolType type, 
                                GameObject prefab, 
                                int amount, 
                                int amountPerTick, 
                                int tickSize, 
                                Action callback = null)
    {
        PoolManager.GetInstance()
            .Addpool(type)
            .PopulateWith(prefab, amount, amountPerTick, tickSize)
            .OnCompletedPopulateEvent += () =>
            {
                callback?.Invoke();
            };
    }

    #endregion


    #region Creation entities

    public GameObject Spawn(PoolType id,
                            GameObject prefab,
                            Vector3 pos,
                            Quaternion rot,
                            Transform parent)
    {
        GameObject go = null;

        //если префаб можно создать через POOL
        if (prefab.GetComponent<IPoolable>() is IPoolable) //pool
        {
            go = PoolSpawn(id, prefab, pos, rot, parent);
        }
        else //стандартное создание объекта
        {
            go = UnityEngine.Object.Instantiate(prefab, pos, rot, parent);
        }

        return go;
    }


    //создаём юнита нужного типа
    GameObject PoolSpawn(PoolType id, GameObject prefab, Vector3 pos, Quaternion rot, Transform parent)
    {
        return PoolManager.GetInstance().Spawn(id, prefab, pos, rot, parent);
    }


    #endregion


    #region Destroy entities

    public void Despawn(PoolType id, GameObject go)
    {
        if (go.GetComponent<IPoolable>() is IPoolable) // возврат в pool 
        {
            PoolManager.GetInstance().Despawn(id, go);
        }
        else //стандартное удаление объекта
        {
            UnityEngine.Object.Destroy(go);
        }
    }

    #endregion



    #region Clear

    public void Clear()
    {
        PoolManager.GetInstance().Dispose();
    }

    #endregion
}
