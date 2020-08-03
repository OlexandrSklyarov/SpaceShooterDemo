using UnityEngine;

namespace SA.SpaceShooter
{
    [System.Serializable]
    public class MapSize
    {
        #region Properties

        public float Up => up; 
        public float Down => down;
        public float Left => left;
        public float Right => right;

        #endregion


        #region Var

        [SerializeField] float up = 20f;
        [SerializeField] float down = -20f;
        [SerializeField] float left = -10f;
        [SerializeField] float right = 10f;

        #endregion
    }
}