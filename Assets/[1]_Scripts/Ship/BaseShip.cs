using System;
using SA.Pool;
using UnityEngine;
using Zenject;
using IPoolable = SA.Pool.IPoolable;

namespace SA.SpaceShooter.Ship
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseShip : MonoBehaviour, IHealth, IPoolable, ITarget
    {
        #region Properties

        public virtual int HP
        {
            get { return currentHP; }
            protected set
            {
                currentHP = Mathf.Clamp(value, 0, shipPrm.MaxHP);
                if (currentHP <= 0)
                {                   
                    DestroyShip(); 
                }
            }
        }

        public int PoolID { get; set; }

        public Target TargetType {get; protected set;}

        #endregion


        #region Var

        [SerializeField] Transform[] firePoints;

        protected Transform myTR;
        protected SignalBus signalBus;
        protected Rigidbody rb;
        protected ShipParameters shipPrm;
        protected MapSize mapSize;
        protected ShipMoving shipMoving;
        protected ShipWeapon shipWeapon;

        protected int currentHP;

        protected bool isFire;

        #endregion


        #region Events

        public event Action<BaseShip> OnShipDestroy;

        #endregion


        #region Init

        protected virtual void Awake()
        {
            myTR = transform;
            rb = GetComponent<Rigidbody>();
            shipMoving = new ShipMoving();
            shipWeapon = new ShipWeapon();
        }


        protected void ShipInit(ShipParameters shipPrm, MapSize mapSize, SignalBus signalBus)
        {
            this.shipPrm = shipPrm;
            currentHP = shipPrm.MaxHP;
            this.signalBus = signalBus;
            this.mapSize = mapSize;

            InitRB();
            InitWeapon();
            shipMoving.Init(rb, shipPrm);
        }


        void InitWeapon()
        {
            shipWeapon.Init(shipPrm, firePoints);
            shipWeapon.OnShoot += () => SignalShootSFX();
        }


        void InitRB()
        {
            rb.mass = shipPrm.Mass;
            rb.drag = shipPrm.Drag;
            rb.angularDrag = shipPrm.AngularDrag;
        }

        #endregion


        #region Update

        public abstract void Tick();
        public abstract void FixedTick();

        #endregion


        #region Weapon

        protected void Fire(Target targetType)
        {
            shipWeapon.Attack(targetType);
        }

        #endregion


        #region Damage

        public void Damage()
        {
            HP--;
        }

        protected virtual void DestroyShip()
        {
            CreateDestroyVFX();
            SignalDestroySFX();
            Deactivate();
        }


        protected virtual void Deactivate()
        {
            OnShipDestroy?.Invoke(this);
            ReturnToPool();
        }


        void CreateDestroyVFX()
        {
            BuildManager.GetInstance().Spawn(   PoolType.VFX, 
                                                shipPrm.ShipDestroyVFX, 
                                                myTR.position, 
                                                Quaternion.identity, 
                                                null);
        }

        #endregion


        #region Signal

        void SignalDestroySFX()
        {
            signalBus.Fire(new SignalGame.PlaySFX_ShipDestroy());
        }


        void SignalShootSFX()
        {
            signalBus.Fire(new SignalGame.PlaySFX_BulletShoot());
        }

        #endregion


        #region Collision

        void OnTriggerEnter(Collider other)
        {
            OnCollision(other);
        }


        protected abstract void OnCollision(Collider other);

        #endregion


        #region Pool

        protected void ReturnToPool()
        {
           BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
        }


        public void OnSpawn() { }


        public void OnDespawn() { }

        #endregion

    }
}