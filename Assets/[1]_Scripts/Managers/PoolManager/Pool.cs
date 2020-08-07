using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace SA.Pool
{
    public class Pool
    {

    #region Var

        Transform parentPool;
        Dictionary<int, Stack<GameObject>> cachedObjects = new Dictionary<int, Stack<GameObject>>();
        
    #endregion


    #region Events

        public event Action OnCompletedPopulateEvent;

    #endregion


    #region Pool Init

        //позволяет создасть некоторое количетво (amount) объектов, 
        //с разбиение по количеству на несклько кадров
        public Pool PopulateWith(GameObject prefab, int amount, int amountPerTick, int tickSize = 1)
        {
            var key = prefab.GetInstanceID();
            
            //если такой ключ есть, выходим из метода
            if (cachedObjects.ContainsKey(key))
            {
                return this;
            }

            cachedObjects.Add(key, new Stack<GameObject>(amount));

            Observable.IntervalFrame(tickSize, FrameCountType.EndOfFrame).Where(val => amount > 0).Subscribe(loop =>
            {
                Observable.Range(0, amountPerTick).Where(check => amount > 0).Subscribe(pop =>
                {
                    Populate(prefab, Vector3.zero, Quaternion.identity, parentPool);                      

                    amount--;

                    //сообщаем подписчикам, что создание завершено
                    if (amount <= 0) OnCompletedPopulateEvent?.Invoke();
                });
            });

            return this;
        }


        public void SetParent(Transform parent)
        {
            this.parentPool = parent;
        }

    #endregion


    #region Create / delete

        public GameObject Spawn(GameObject prefab, 
                                    Vector3 position = default(Vector3), 
                                        Quaternion rotation = default(Quaternion), 
                                            Transform parent = null)
        {
            var key = prefab.GetInstanceID(); 
            var isExistStack = cachedObjects.ContainsKey(key);

            //если стек есть, 
            if (isExistStack)
            {
                var stack = cachedObjects[key];

                //и он не пустой
                if (stack.Count > 0)
                {
                    var go = stack.Pop();                    

                    go.SetActive(true);

                    var tr = go.transform; 
                                   
                    tr.SetParent((parent) ? parent : parentPool);
                    tr.position = position;
                    tr.rotation = rotation;                      

                    //если на объекте есть компонент IPoolable, вызываем метод инициализации
                    var poolable = tr.GetComponent<IPoolable>();
                    if (poolable != null) poolable.OnSpawn();

                    return go;
                }
            }           

            //создаём новый объект и запоминаем его ID
            Populate(prefab, position, rotation, parent);                       

            var newGO = cachedObjects[key].Pop();
            newGO.SetActive(true);

            return newGO;
        }


        public void Despawn(GameObject go)
        {
            var poolable = go.GetComponent<IPoolable>();

            if (poolable != null)
            {
                poolable.OnDespawn();
                var key = poolable.PoolID;
                cachedObjects[key].Push(go);
            }

            if (parentPool != null) go.transform.SetParent(parentPool);

            go.SetActive(false);
        }


        void Populate(GameObject prefab, 
                                Vector3 position = default(Vector3), 
                                    Quaternion rotation = default(Quaternion), 
                                        Transform parent = null)
        {
            //получаем ключ по префабу
            var key = prefab.GetInstanceID();

            var go = UnityEngine.Object.Instantiate(prefab, position, rotation, parent);

            go.GetComponent<IPoolable>().PoolID = key; 

            //создаём стек под объекты, если нет такого
            if (!cachedObjects.ContainsKey(key)) cachedObjects.Add(key, new Stack<GameObject>());
            
            cachedObjects[key].Push(go);

            var tr = go.transform;

            if (parent == null)            
                tr.position = position;                            
            else            
                tr.localPosition = position;               

            go.SetActive(false); 
        }


    #endregion


    #region Clear

        public void Dispose()
        {
            parentPool = null;
            cachedObjects?.Clear();
        }

    #endregion

    }
}