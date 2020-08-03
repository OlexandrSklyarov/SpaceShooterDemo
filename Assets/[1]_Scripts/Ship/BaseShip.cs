using SA.Pool;
using UnityEngine;
using Zenject;
using IPoolable = SA.Pool.IPoolable;

namespace SA.SpaceShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class BaseShip : MonoBehaviour, IHealth, IPoolable
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

        #endregion


        #region Var

        [SerializeField] Transform[] firePoints;

        protected Transform myTR;
        protected SignalBus signalBus;
        protected Rigidbody rb;
        protected ShipParameters prm;
        protected MapSize mapSize;

        protected int currentHP;

        protected float horizontal;
        protected float vertical;
        protected bool isFire;

        protected float lastFireTime;

        #endregion


        #region Init

        void Awake()
        {
            myTR = transform;
            rb = GetComponent<Rigidbody>();
        }


        public virtual void Init(ShipParameters prm, MapSize mapSize, SignalBus signalBus)
        {
            this.prm = prm;
            currentHP = prm.MaxHP;
            this.signalBus = signalBus;
            this.mapSize = mapSize;

            InitRB();
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


        #region Moving

        protected void Move()
        {
            rb.velocity = new Vector3(horizontal, 0f, vertical) * prm.Speed;
        }


        protected void Rotation()
        {
            rb.rotation = Quaternion.Euler(0f, 0f, rb.velocity.x * -prm.MaxRotateAngle);
        }

        #endregion


        #region Attack

        protected void Attack(Bullet.Target target)
        {
            if (Time.time < lastFireTime) return;

            //делаев выстрел со всех точек
            foreach(var point in firePoints)
            {
                var go = BuildManager.GetInstance().Spawn(PoolType.ENTITIES,
                                                    prm.BulletPrefab,
                                                    point.position,
                                                    point.rotation,
                                                    null);

                go.GetComponent<Bullet>().Push(point.forward, target);
            }

            lastFireTime = Time.time + prm.FireCooldown;
        }

        #endregion


        #region Damage

        public void Damage()
        {
            HP--;
        }


        void SelfDestroy()
        {
            BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
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