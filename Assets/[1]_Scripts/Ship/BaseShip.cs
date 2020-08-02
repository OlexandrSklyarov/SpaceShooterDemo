using SA.Pool;
using UnityEngine;
using Zenject;
using IPoolable = SA.Pool.IPoolable;

namespace SA.SpaceShooter
{
    public abstract class BaseShip : MonoBehaviour, IHealth, IPoolable
    {
        #region Properties

        public int HP
        {
            get { return currentHP; }
            private set
            {
                currentHP = Mathf.Clamp(value, 0, maxHP);
                if (currentHP <= 0) SelfDestroy();
            }
        }

        public int PoolID { get; set; }

        #endregion


        #region Var

        [SerializeField] GameObject bulletPrefab;
        [SerializeField] GameObject destroyVFX;

        protected SignalBus signalBus;

        protected int currentHP;
        protected int maxHP;
        protected float speed;

        #endregion


        #region Update

        public abstract void Tick();

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