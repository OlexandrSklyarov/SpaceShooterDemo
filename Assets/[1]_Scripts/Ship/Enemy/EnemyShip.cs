using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter.Ship
{
    public class EnemyShip : BaseShip
    {

        #region Var


        #endregion


        #region Init

        public override void Init(ShipParameters prm, MapSize mapSize, SignalBus signalBus)
        {
            base.Init(prm, mapSize, signalBus);
            TargetType = Target.OTHER;
        }

        #endregion


        #region Update

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }


        public override void FixedTick()
        {
            throw new System.NotImplementedException();
        }

        #endregion

    }
}