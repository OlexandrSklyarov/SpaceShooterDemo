using UnityEngine;
using SA.Pool;

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


    #region Creation entities

    public GameObject Spawn(PoolType id,
                            GameObject prefab,
                            Vector3 pos,
                            Quaternion rot,
                            Transform parent)
    {
        GameObject go = null;

        //если префаб можно создать через POOL
        if (prefab.GetType() == typeof(IPoolable)) //pool
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
        if (go.GetType() == typeof(IPoolable)) // возврат в pool 
        {
            PoolManager.GetInstance().Despawn(id, go);
        }
        else //стандартное удаление объекта
        {
            UnityEngine.Object.Destroy(go);
        }
    }

    #endregion
}
