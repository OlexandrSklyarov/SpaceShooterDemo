using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter
{
    public class PlayerShip : BaseShip
    {
        #region Init

        public void Init(float speed, int hp, SignalBus signalBus)
        {
            this.speed = speed;
            currentHP = maxHP = hp;
            this.signalBus = signalBus;
        }

        #endregion


        #region Update

        public override void Tick()
        {

        }

        #endregion
    }
}