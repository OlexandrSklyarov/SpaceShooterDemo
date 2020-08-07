using UnityEngine;
using SA.Pool;
using SA.SpaceShooter.Ship;
using Zenject;
using System;

namespace SA.SpaceShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class Asteroid : MonoBehaviour, Pool.IPoolable, IEnemy, ITarget, IHealth
    {
        #region Properties

        public int PoolID { get; set; }

        public Target TargetType { get; private set; }

        public int HP
        {
            get => currentHP;
            private set
            {
                currentHP = Mathf.Clamp(value, 0, MAX_HP);
                if (currentHP <= 0)
                {
                    SignalAddPoint();
                    CreateVFX();
                    ReturnToPool();
                }
            }
        }
        #endregion


        #region Events

        public event Action<Asteroid> OnDestroyAsteroids;

        #endregion 


        #region Var

        [SerializeField] GameObject destroyVFX;

        Transform myTR;
        SignalBus signalBus;
        Rigidbody rb;

        int incomePoints;
        int currentHP;
        const int MAX_HP = 1;

        float zMapSize;

        #endregion


        #region Init

        void Awake()
        {
            myTR = transform;
            rb = GetComponent<Rigidbody>();
            TargetType = Target.OTHER;
        }

        public void Push(Vector3 force, float zMapSize, int incomePoints, SignalBus signalBus)
        {
            this.incomePoints = incomePoints;
            this.zMapSize = zMapSize;
            this.signalBus = signalBus;
            HP = MAX_HP;

            rb.isKinematic = false;
            rb.AddForce(force, ForceMode.VelocityChange);
            RandomRotate();
        }


        void RandomRotate()
        {
           rb.angularVelocity = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.Range(0.1f, 10f);
        }

        #endregion


        #region Update

        public void Tick()
        {
            if (myTR.position.z < zMapSize)
            {
                ReturnToPool();
            }
        }

        #endregion


        #region Collision

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerShip>() is PlayerShip)
            {
                Damage();
            }
        }


        void SignalAddPoint()
        {
            signalBus.Fire(new SignalGame.AddPoints()
            { 
                PointSum = incomePoints
            });
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
            if (gameObject == null) return;

            OnDestroyAsteroids?.Invoke(this);
            BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
        }


        public void OnSpawn() { }


        public void OnDespawn()
        {
            rb.isKinematic = true;
        }


        public void Damage()
        {
            HP--;
        }

        #endregion    

    }
}