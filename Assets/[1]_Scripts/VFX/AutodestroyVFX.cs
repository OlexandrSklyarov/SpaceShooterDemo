using UnityEngine;
using SA.Pool;
using UniRx;

namespace SA.SpaceShooter
{
    public class AutodestroyVFX : MonoBehaviour, IPoolable
    {
        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        [SerializeField] ParticleSystem particle;

        CompositeDisposable compositDisposable;
        double timeDestroy;

        #endregion


        #region Init

        void Awake()
        {
            timeDestroy = particle.time * 0.97d;

            Deactivate(timeDestroy);
        }

        #endregion


        #region Pool

        public void OnSpawn()
        {
            particle.Play();

            Deactivate(timeDestroy);
        }


        public void OnDespawn()
        {
            particle.Stop();
            compositDisposable?.Dispose();
        }


        void Deactivate(double time)
        {
            compositDisposable = new CompositeDisposable();

            Observable.Timer(System.TimeSpan.FromSeconds(time)) // создаем timer Observable
            .Subscribe(_ =>
            { // подписываемся
                BuildManager.GetInstance().Despawn(PoolType.VFX, this.gameObject);
            })
            .AddTo(compositDisposable); // привязываем подписку к disposable
        }

        #endregion
    }
}