using UnityEngine;
using SA.Pool;
using SA.SpaceShooter.Ship;

namespace SA.SpaceShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoActionTimer, IPoolable, IEnemy
    {
        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        [SerializeField] GameObject destroyVFX;

        Rigidbody rb;      

        #endregion


        #region Init

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Push(Vector3 force, float lifeTime)
        {
            ActionTimer(lifeTime, ReturnToPool);
            rb.AddForce(force, ForceMode.VelocityChange);
        }

        #endregion


        #region Collision

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerShip>() is PlayerShip)
            {
                CreateVFX();
                ReturnToPool();
            }
        }


        void CreateVFX()
        {
            BuildManager.GetInstance().Spawn(   PoolType.VFX,
                                                destroyVFX,
                                                transform.position,
                                                Quaternion.identity,
                                                null);
        }

        #endregion


        #region Pool

        void ReturnToPool()
        {
            BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
        }

        public void OnDespawn() { }

        public void OnSpawn() 
        {
            OnDispose();
        }

        #endregion
    }
}