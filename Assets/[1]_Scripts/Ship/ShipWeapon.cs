using SA.Pool;
using UnityEngine;

namespace SA.SpaceShooter.Ship
{
    public class ShipWeapon
    {
        #region Var

        ShipParameters prm;
        Transform[] firePoints;
        float lastFireTime;

        #endregion

        #region Init

        public void Init(ShipParameters prm, Transform[] firePoints)
        {
            this.prm = prm;
            this.firePoints = firePoints;
        }

        #endregion


        #region Attack

        public void Attack(Target target)
        {
            if (Time.time < lastFireTime) return;

            //делаев выстрел со всех точек
            foreach (var point in firePoints)
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
    }
}