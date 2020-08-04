using UnityEngine;
using SA.Pool;
using UniRx;

namespace SA.SpaceShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour, IPoolable
    {
        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        Target bulletTarget;
        Rigidbody rb;
        bool isPushed;

        const float SPEED = 20f;
        const double DESTROY_TIMER = 7d;

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
            if (other.gameObject.GetComponent<Bullet>()) return;

            var target = other.gameObject.GetComponent<ITarget>();

            if (target != null)
            {
                if (bulletTarget == target.TargetType)
                {
                    other.gameObject.GetComponent<IHealth>()?.Damage();
                    SelfDestroy();
                    Debug.Log("damage player");
                }
            }
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