using UnityEngine;
using SA.Pool;
using UniRx;

namespace SA.SpaceShooter
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoActionTimer, IPoolable
    {
        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        [SerializeField] GameObject destroyVFX;

        Target bulletTarget;
        Rigidbody rb;
        bool isPushed;

        const float SPEED = 20f;
        const float DESTROY_TIMER = 7f;

        #endregion


        #region Init

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }


        public void Push(Vector3 dir, Target target)
        {
            bulletTarget = target;

            rb.isKinematic = false;
            rb.AddForce(dir * SPEED, ForceMode.Impulse);

            ActionTimer(DESTROY_TIMER, ReturnToPool);

            isPushed = true;
        }

        #endregion


        #region Collision

        void OnTriggerEnter(Collider other)
        {
            if (!isPushed) return;

            var target = other.gameObject.GetComponent<ITarget>();

            if (target != null)
            {
                if (bulletTarget == target.TargetType)
                {
                    other.gameObject.GetComponent<IHealth>()?.Damage();
                    CreateVFX();
                    ReturnToPool();
                }
            }
        }


        void CreateVFX()
        {
            BuildManager.GetInstance().Spawn(   PoolType.VFX,
                                                destroyVFX,
                                                transform.position,
                                                Quaternion.identity,
                                                null);
        }

        #endregion


        #region Pool

        void ReturnToPool()
        {
            BuildManager.GetInstance().Despawn(PoolType.ENTITIES, this.gameObject);
        }

        public void OnSpawn()
        {

        }

        public void OnDespawn()
        {           
            rb.isKinematic = true;
            isPushed = false;
            OnDispose();
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