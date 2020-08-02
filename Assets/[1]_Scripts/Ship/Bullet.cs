using UnityEngine;
using SA.Pool;

namespace SA.SpaceShooter
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        #region Data

        enum Target { PLAYER, OTHER }

        #endregion


        #region Properties

        public int PoolID { get; set; }

        #endregion


        #region Var

        [SerializeField] Target bulletTarget;

        #endregion


        #region Init

        #endregion


        #region Collision

        void OnTriggerEnter(Collider other)
        {
            if (bulletTarget == Target.PLAYER && other.GetType() == typeof(PlayerShip))
            {
                other.gameObject.GetComponent<IHealth>()?.Damage();
                SelfDestroy();
            }
            else if (other.GetType() != typeof(PlayerShip))
            {
                other.gameObject.GetComponent<IHealth>()?.Damage();
                SelfDestroy();
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

        }

        #endregion
    }
}