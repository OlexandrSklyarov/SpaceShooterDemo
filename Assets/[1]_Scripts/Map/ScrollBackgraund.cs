using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter
{
    public class ScrollBackgraund : MonoBehaviour
    {
        #region Var

        Transform myTR;
        DataBackgraund data;

        #endregion


        #region Init

        [Inject]
        public void Construct(DataBackgraund data)
        {
            this.data = data;
            myTR = transform;
        }

        #endregion


        #region Update

        public void Tick()
        {
            //сдвигаем фон по оси Z
            myTR.position = new Vector3()
            {
                x = myTR.position.x,
                y = myTR.position.y,
                z = Mathf.Repeat(Time.time * -data.ScrollSpeed, data.TileSize)
            };
        }

        #endregion

    }
}