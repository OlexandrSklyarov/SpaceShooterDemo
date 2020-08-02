using UnityEngine;

namespace SA.Pool
{
    public class SimplePoolGameObject : MonoBehaviour, IPoolable
    {
        public int PoolID { get; set; }

        public void OnDespawn()
        {

        }

        public void OnSpawn()
        {

        }
    }
}
