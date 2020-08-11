using UnityEngine;

namespace SA.SpaceShooter
{
    [System.Serializable]
    public class Level
    {
        #region Data

        public enum LevelStatus { CLOSE, OPEN, COMPLETED }

        #endregion


        #region Var

        [Range(1, 1000)] public int maxDestroyAsteroids = 10;
        public LevelStatus status;

        #endregion
    }
}