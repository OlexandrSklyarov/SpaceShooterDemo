using SA.SpaceShooter.Input;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter.Ship
{
    public class PlayerShip : BaseShip
    {
        #region Var

        PlayerInput input;

        #endregion


        #region Init

        public void Init(ShipParameters shipPrm, MapSize mapSize, SignalBus signalBus)
        {
            ShipInit(shipPrm, mapSize, signalBus);

            TargetType = Target.PLAYER;

            input = new PlayerInput();
        }


        #endregion


        #region Moving

        void Bound()
        {
            rb.position = new Vector3()
            {
                x = Mathf.Clamp(rb.position.x, mapSize.Left, mapSize.Right),
                y = 0f,
                z = Mathf.Clamp(rb.position.z, mapSize.Down, mapSize.Up),
            };
        }

        #endregion


        #region Attack

        void Fire()
        {
            if (input.IsFire)
            {
                shipWeapon.Attack(Target.OTHER);
            }
        }

        #endregion


        #region Update

        public override void Tick()
        {
            input.Tick();
            Fire();
        }


        public override void FixedTick()
        {
            shipMoving.Rotation();
            shipMoving.Move(input.Horizontal, input.Vertical);
            Bound();
        }

        #endregion
    }
}