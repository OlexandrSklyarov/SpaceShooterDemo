using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace SA.SpaceShooter.Ship
{
    public class EnemyShip : BaseShip
    {
        #region Var

        float lastFireTime;

        float targetManeuver;

        EnemyParameters enemyPrm;

        #endregion


        #region Init

        public void Init(ShipParameters prm, MapSize mapSize, SignalBus signalBus, EnemyParameters enemyPrm)
        {
            ShipInit(prm, mapSize, signalBus);

            TargetType = Target.OTHER;
            this.enemyPrm = enemyPrm;

            //запускаем случайный манёвр корабля
            StartCoroutine(TargetManeuver());
        }

        #endregion


        #region Update

        public override void Tick()
        {
            Fire();
        }


        public override void FixedTick()
        {
            //horizontal = shipMoving.GetManeuverValue(targetManeuver, enemyPrm.SpeedManeuver);
            shipMoving.Move(horizontal, -prm.Speed);
            BoundHorizontal();
        }


        void BoundHorizontal()
        {
            rb.position = new Vector3()
            {
                x = Mathf.Clamp(rb.position.x, mapSize.Left, mapSize.Right),
                y = 0f,
                z = rb.position.z
            };
        }


        void Fire()
        {
            if (IsTimeEnd(ref lastFireTime, prm.FireCooldown))
            {
                shipWeapon.Attack(Target.PLAYER);
            }
        }
           

        //задаёт направление манёвра корабля в случайное время
        IEnumerator TargetManeuver()
        {
            //задержка перед стартом манёвра
            var rndTimer = Random.Range(0f, enemyPrm.StartManeuverTime);
            yield return new WaitForSeconds(rndTimer);

            while (true)
            {
                //устанавливаем диапазон манёвра
                targetManeuver = Random.Range(1f, enemyPrm.DodgeRange) * -Mathf.Sign(myTR.position.x);

                //выполняем манёвр пока не истечёт время
                var maneuverTime = Random.Range(0.1f, enemyPrm.StartManeuverTime);
                yield return new WaitForSeconds(maneuverTime);

                //обнуляем значение манёвра
                targetManeuver = 0f;

                //ставим на паузу перед следующим манёвром
                var puuseTime = Random.Range(0.5f, enemyPrm.ManeuverTime);
                yield return new WaitForSeconds(puuseTime);
            }
        }


        //истекло ли currentTime
        bool IsTimeEnd(ref float curentTime, float timer)
        {
            if (Time.time > curentTime)
            {
                curentTime = Time.time + timer;
                return true;
            }

            return false;
        }

        #endregion

    }
}