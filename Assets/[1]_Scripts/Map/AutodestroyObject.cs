using UnityEngine;
using SA.Pool;
using UniRx;

namespace SA.SpaceShooter
{
    public class AutodestroyObject : MonoBehaviour, IPoolable
    {
        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        [SerializeField] ParticleSystem particle;

        double timeDestroy;

        #endregion


        #region Init

        void Awake()
        {
            timeDestroy = particle.time;
        }

        #endregion


        #region Pool

        public void OnDespawn()
        {
            particle.Stop();
        }

        public void OnSpawn()
        {
            particle.Play();

            SelfDestroy(timeDestroy);
        }


        void SelfDestroy(double time)
        {
            Observable.Timer(System.TimeSpan.FromSeconds(time)) // создаем timer Observable
            .Subscribe(_ =>
            { // подписываемся
                BuildManager.GetInstance().Despawn(PoolType.VFX, this.gameObject);
            })
            .AddTo(this); // привязываем подписку к disposable
        }

        #endregion
    }
}