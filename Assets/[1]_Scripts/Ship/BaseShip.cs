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
                currentHP = Mathf.Clamp(value, 0, shipPrm.MaxHP);
                if (currentHP <= 0)
                {
                    CreateDestroyVFX();
                    Deactivate(); 
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

        void Awake()
        {
            myTR = transform;
            rb = GetComponent<Rigidbody>();
        }


        protected void ShipInit(ShipParameters shipPrm, MapSize mapSize, SignalBus signalBus)
        {
            this.shipPrm = shipPrm;
            currentHP = shipPrm.MaxHP;
            this.signalBus = signalBus;
            this.mapSize = mapSize;

            InitRB();

            shipMoving = new ShipMoving(rb, shipPrm);
            shipWeapon = new ShipWeapon(shipPrm, firePoints);
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


        #region Damage

        public void Damage()
        {
            HP--;
        }


        protected virtual void Deactivate()
        {
            OnShipDestroy?.Invoke(this);
            BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
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