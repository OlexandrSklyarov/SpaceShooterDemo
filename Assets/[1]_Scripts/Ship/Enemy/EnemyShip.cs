using System;
using System.Collections;
using UniRx;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace SA.SpaceShooter.Ship
{
    public class EnemyShip : BaseShip, IEnemy
    {
        #region Var

        EnemyManeuverParameters maneuverPrm;
        CompositeDisposable compositeDisposable;

        float targetManeuver;
        int incomePoints;

        #endregion


        #region Init

        public void Init(   ShipParameters shipPrm, 
                            MapSize mapSize, 
                            SignalBus signalBus, 
                            EnemyManeuverParameters maneuverPrm,
                            int incomePoints)
        {
            ShipInit(shipPrm, mapSize, signalBus);

            TargetType = Target.OTHER;
            this.maneuverPrm = maneuverPrm;
            this.incomePoints = incomePoints;

            //запускаем случайный манёвр корабля
            StartManeuverProcess();
        }

        #endregion


        #region Update

        public override void Tick()
        {
            shipWeapon.Attack(Target.PLAYER);
        }


        public override void FixedTick()
        {
            shipMoving.Rotation(180f);           

            var hor = shipMoving.GetManeuverValue(targetManeuver, maneuverPrm.SpeedManeuver);           
            shipMoving.Move(hor, -shipPrm.Speed);

            BoundHorizontal();
            CheckLeavingZone(mapSize.Down);
        }

        #endregion


        #region Moving

        void BoundHorizontal()
        {
            rb.position = new Vector3()
            {
                x = Mathf.Clamp(rb.position.x, mapSize.Left, mapSize.Right),
                y = 0f,
                z = rb.position.z
            };
        }


        //проверка, если корабль вышел из зоны видимости, деактивировать его
        void CheckLeavingZone(float value)
        {
            if (rb.position.z < value)
            {
                Deactivate();
            }
        }


        //задаёт направление манёвра корабля
        void StartManeuverProcess()
        {
            ActionTimer(Random.Range(maneuverPrm.StartManeuverTime.x, maneuverPrm.StartManeuverTime.y), () =>
            {
                ExecuteManeuver();
            });
        }


        void ExecuteManeuver()
        {
            //устанавливаем диапазон манёвра
            var side = (myTR.position.x >= 0f) ? -1 : 1f;
            targetManeuver = Random.Range(1f, maneuverPrm.DodgeRange) * side;

            ActionTimer(Random.Range(maneuverPrm.ManeuverTime.x, maneuverPrm.ManeuverTime.y), () =>
            {
                //обнуляем значение манёвра
                targetManeuver = 0f;

                ActionTimer(Random.Range(maneuverPrm.PauseManeuverTime.x, maneuverPrm.PauseManeuverTime.y), () =>
                {
                    ExecuteManeuver();
                });
            });
        }


        void ActionTimer(float time, Action act)
        {
            compositeDisposable = new CompositeDisposable();

            Observable.Timer(System.TimeSpan.FromSeconds(time)) // создаем timer Observable
            .Subscribe(_ =>
            { // подписываемся
                act?.Invoke();
            })
            .AddTo(compositeDisposable); // привязываем подписку к disposable
        }

        #endregion


        #region Destroy

        protected override void DestroyShip()
        {
            SignalAddPoint();
            base.DestroyShip();
        }


        protected override void Deactivate()
        {
            base.Deactivate();

            compositeDisposable?.Dispose();
        }

        #endregion


        #region Points

        void SignalAddPoint()
        {
            signalBus.Fire(new SignalGame.AddPoints()
            {
                PointSum = incomePoints
            });
        }

        #endregion


        #region Collision

        protected override void OnCollision(Collider other)
        {
            if (other.gameObject.GetComponent<PlayerShip>() is PlayerShip)
            {
                Debug.Log("Player Collision!!!");
                Damage();
            }
        }

        #endregion

    }
}