using UnityEngine;
using SA.Pool;
using SA.SpaceShooter.Ship;
using Zenject;
using System;
using Random = UnityEngine.Random;

namespace SA.SpaceShooter
{
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
                    OnDestroyAsteroid?.Invoke(this);
                    
                    SignalAddPoint();
                    CreateVFX();
                    SignalSFX();
                    ReturnToPool();
                }
            }
        }
        #endregion


        #region Events

        public event Action<Asteroid> OnDestroyAsteroid;

        #endregion 


        #region Var

        [SerializeField] GameObject destroyVFX;

        Transform myTR;
        SignalBus signalBus;

        int incomePoints;
        int currentHP;
        const int MAX_HP = 1;

        float zMapSize;
        private Vector3 _moveDirection;
        private float _speed;
        private float _rotateSpeed;
        private Quaternion _startRotation;
        private Vector3 _randomRotation;
        private bool _isCanMove;

        #endregion


        #region Init

        void Awake()
        {
            myTR = transform;
            TargetType = Target.OTHER;
        }


        public void Init(float zMapSize, int incomePoints, SignalBus signalBus)
        {
            this.incomePoints = incomePoints;
            this.zMapSize = zMapSize;
            this.signalBus = signalBus;
            HP = MAX_HP;
        }


        public void Push(Vector3 dir, float speed)
        {
            _moveDirection = dir;
            _speed = speed;
            _rotateSpeed = Random.Range(0f, speed);
            _startRotation = myTR.rotation;
            _randomRotation = Random.insideUnitSphere;
            _isCanMove = true;
        }


        #endregion


        #region Update

        public void Tick()
        {
            if (!_isCanMove) return;

            myTR.position += _moveDirection * _speed * Time.deltaTime;

            _randomRotation.x += _rotateSpeed * Time.deltaTime;
            _randomRotation.y += _rotateSpeed * Time.deltaTime;
            _randomRotation.z += _rotateSpeed * Time.deltaTime;
            myTR.rotation = _startRotation * Quaternion.Euler(_randomRotation);

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


        void SignalSFX()
        {
            signalBus.Fire(new SignalGame.PlaySFX_BigAsteroidDestroy());
        }


        void CreateVFX()
        {
            BuildManager.GetInstance()
                .Spawn(PoolType.VFX, destroyVFX, transform.position,Quaternion.identity, null);
        }

        #endregion


        #region Pool

        void ReturnToPool()
        {
            if (gameObject == null) return;
            
            BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
        }


        public void OnSpawn() { }


        public void OnDespawn() => _isCanMove = false;


        public void Damage()
        {
            HP--;
        }

        #endregion    

    }
}