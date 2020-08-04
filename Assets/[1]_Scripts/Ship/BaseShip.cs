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

        public int HP
        {
            get { return currentHP; }
            private set
            {
                currentHP = Mathf.Clamp(value, 0, prm.MaxHP);
                if (currentHP <= 0) SelfDestroy();
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
        protected ShipParameters prm;
        protected MapSize mapSize;
        protected ShipMoving shipMoving;
        protected ShipWeapon shipWeapon;

        protected int currentHP;

        protected float horizontal;
        protected float vertical;
        protected bool isFire;

        #endregion


        #region Events

        public event Action<BaseShip> OnShipDestroy;

        #endregion


        #region Init

        void Awake()
        {
            myTR = transform;
            rb = GetComponent<Rigidbody>();
        }


        protected void ShipInit(ShipParameters prm, MapSize mapSize, SignalBus signalBus)
        {
            this.prm = prm;
            currentHP = prm.MaxHP;
            this.signalBus = signalBus;
            this.mapSize = mapSize;

            InitRB();

            shipMoving = new ShipMoving(rb, prm);
            shipWeapon = new ShipWeapon(prm, firePoints);
        }


        void InitRB()
        {
            rb.mass = prm.Mass;
            rb.drag = prm.Drag;
            rb.angularDrag = prm.AngularDrag;
        }

        #endregion


        #region Update

        public abstract void Tick();
        public abstract void FixedTick();

        #endregion      


        #region Damage

        public void Damage()
        {
            HP--;
        }


        void SelfDestroy()
        {
            OnShipDestroy?.Invoke(this);
            CreateDestroyVFX();
            BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
        }


        void CreateDestroyVFX()
        {
            BuildManager.GetInstance().Spawn(   PoolType.VFX, 
                                                prm.ShipDestroyVFX, 
                                                myTR.position, 
                                                myTR.rotation, 
                                                null);
        }

        #endregion


        #region Pool

        public void OnSpawn()
        {

        }

        public void OnDespawn()
        {

        }

        #endregion
    }
}