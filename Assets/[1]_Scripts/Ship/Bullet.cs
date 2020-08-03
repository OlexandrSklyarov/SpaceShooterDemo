using UnityEngine;
using SA.Pool;
using UniRx;

namespace SA.SpaceShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour, IPoolable
    {
        #region Data

        public enum Target { PLAYER, OTHER }

        #endregion


        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        Target bulletTarget;
        Rigidbody rb;
        bool isPushed;

        const float SPEED = 10f;
        const double DESTROY_TIMER = 5d;

        #endregion


        #region Init

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }


        public void Push(Vector3 dir, Target target)
        {
            bulletTarget = target;

            rb.drag = 0f;
            rb.AddForce(dir * SPEED, ForceMode.Impulse);

            SelfDestroy(DESTROY_TIMER);

            isPushed = true;
        }


        void SelfDestroy(double time)
        {
            Observable.Timer(System.TimeSpan.FromSeconds(time)) // создаем timer Observable
            .Subscribe(_ =>
            { // подписываемся
                BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
            })
            .AddTo(this); // привязываем подписку к disposable
        }

        #endregion


        #region Collision

        void OnTriggerEnter(Collider other)
        {
            if (!isPushed) return;

            if (bulletTarget == Target.PLAYER && other.GetType() == typeof(PlayerShip))
            {
                other.gameObject.GetComponent<IHealth>()?.Damage();
                SelfDestroy();
            }
            else if (bulletTarget == Target.OTHER && other.GetType() != typeof(PlayerShip)) 
            {
                other.gameObject.GetComponent<IHealth>()?.Damage();
                SelfDestroy();
            }

            Debug.Log($"Collision {other.name}");
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
            rb.drag = Mathf.Infinity;
            rb.velocity = Vector3.zero;
            isPushed = false;
        }

        #endregion
    }
}