using UnityEngine;
using Zenject;

namespace SA.SpaceShooter.Ship
{
    public class PlayerShip : BaseShip
    {
        #region Init

        public override void Init(ShipParameters prm, MapSize mapSize, SignalBus signalBus)
        {
            base.Init(prm, mapSize, signalBus);
            TargetType = Target.PLAYER;
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
            if (isFire)
            {
                shipWeapon.Attack(Target.OTHER);
            }
        }

        #endregion


        #region Update

        public override void Tick()
        {
            GetInput();
            Fire();
        }


        public override void FixedTick()
        {
            shipMoving.Rotation();
            shipMoving.Move(horizontal, vertical);
            Bound();
        }


        void GetInput()
        {

#if UNITY_EDITOR

            horizontal = Input.GetAxis(StaticPrm.Input.HORIZONTAL);
            vertical = Input.GetAxis(StaticPrm.Input.VERTICAL);
            isFire = Input.GetButton(StaticPrm.Input.FIRE);

#elif UNITY_ANDROID

            //mobile input

#endif
        }


#endregion
    }
}