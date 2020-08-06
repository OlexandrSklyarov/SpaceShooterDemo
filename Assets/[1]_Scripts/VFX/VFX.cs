using UnityEngine;
using SA.Pool;

namespace SA.SpaceShooter
{
    public class VFX : MonoActionTimer, IPoolable
    {
        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        [SerializeField] ParticleSystem particle;

        float timeDestroy;

        #endregion


        #region Init

        void Awake()
        {
            timeDestroy = particle.time * 0.97f;

            ActionTimer(timeDestroy, ReturtToPool);
        }

        #endregion


        #region Pool

        public void OnSpawn()
        {
            particle.Play();

            ActionTimer(timeDestroy, ReturtToPool);
        }


        public void OnDespawn()
        {
            particle.Stop();
            OnDispose();
        }


        void ReturtToPool()
        {
            BuildManager.GetInstance().Despawn(PoolType.VFX, this.gameObject);
        }

        #endregion


        #region Clear

        void OnDestroy()
        {
            OnDispose();
        }

        #endregion
    }
}