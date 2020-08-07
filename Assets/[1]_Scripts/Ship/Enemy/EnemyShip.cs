using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace SA.SpaceShooter.Ship
{
    public class EnemyShip : BaseShip, IEnemy
    {
        #region Var

        EnemyManeuverParameters maneuverPrm;

        float targetManeuver;
        int incomePoints;
        bool isMoneuverProcces;
        Coroutine maneuverCoroutine;

        #endregion


        #region Init

        public void Init(ShipParameters shipPrm,
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
            maneuverCoroutine = StartCoroutine( StartManeuverProcess() );
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
        IEnumerator StartManeuverProcess()
        {
            isMoneuverProcces = true;

            var startDeley = Random.Range(maneuverPrm.StartManeuverTime.x,
                                            maneuverPrm.StartManeuverTime.y);

            yield return new WaitForSeconds(startDeley);

            while (isMoneuverProcces)
            {
                //устанавливаем диапазон манёвра
                var side = (myTR.position.x >= 0f) ? -1 : 1f;
                targetManeuver = Random.Range(1f, maneuverPrm.DodgeRange) * side;

                var timeManeuver = Random.Range(maneuverPrm.ManeuverTime.x, maneuverPrm.ManeuverTime.y);

                yield return new WaitForSeconds(timeManeuver);

                //обнуляем значение манёвра
                targetManeuver = 0f;

                var timePause = Random.Range(maneuverPrm.PauseManeuverTime.x,
                                                maneuverPrm.PauseManeuverTime.y);

                yield return new WaitForSeconds(timePause);
            }
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
            isMoneuverProcces = false;
            StopCoroutine(maneuverCoroutine);
            base.Deactivate();
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
                Damage();
            }
        }

        #endregion


        #region Clear

        void OnDestroy()
        {
            isMoneuverProcces = false;
        }

        #endregion

    }
}