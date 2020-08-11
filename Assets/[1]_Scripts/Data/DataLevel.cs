using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataLevel", menuName = "Data/DataLevel")]
    public class DataLevel : ScriptableObject
    {
        #region Properties

        public Level[] Levels => levels;

        #endregion


        #region Var

        [Space]
        [Header("Game levels data")]
        [SerializeField] Level[] levels;

        #endregion
    }
}