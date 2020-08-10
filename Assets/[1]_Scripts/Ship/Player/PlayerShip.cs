using SA.SpaceShooter.Input;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter.Ship
{
    public class PlayerShip : BaseShip
    {
        #region Properties

        public override int HP
        {
            get { return currentHP; }
            protected set
            {
                currentHP = Mathf.Clamp(value, 0, shipPrm.MaxHP);

                SignalChangeHP();

                if (currentHP <= 0)
                {
                    DestroyShip();
                }
            }
        }

        #endregion

        #region Var

        PlayerInput input;

        #endregion


        #region Init

        protected override void Awake()
        {
            base.Awake();
            input = new PlayerInput();
        }

        public void Init(ShipParameters shipPrm, MapSize mapSize, SignalBus signalBus)
        {
            ShipInit(shipPrm, mapSize, signalBus);

            SignalChangeHP();

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
       

        #region Update

        public override void Tick()
        {
            input.Tick();

            if (input.IsFire)
                Fire(Target.OTHER);
        }


        public override void FixedTick()
        {
            shipMoving.Rotation();
            shipMoving.Move(input.Horizontal, input.Vertical);
            Bound();
        }

        #endregion


        #region Collision

        protected override void OnCollision(Collider other)
        {
            if (other.gameObject.GetComponent<IEnemy>() is IEnemy)
            {
                Damage();
            }
        }


        protected override void DestroyShip()
        {
            SignalPlayerDestroy();
            base.DestroyShip();
        }

        #endregion


        #region Signal

        void SignalChangeHP()
        {
            signalBus.Fire(new SignalGame.ChangePlayerHP()
            {
                AmountHP = HP
            });
        }


        void SignalPlayerDestroy()
        {
            signalBus.Fire(new SignalGame.PlayerDestroy());
        }

        #endregion

    }
}