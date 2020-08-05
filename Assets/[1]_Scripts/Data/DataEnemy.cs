using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataEnemy", menuName = "Data/DataEnemy")]
    public class DataEnemy : ScriptableObject
    {
        #region Properties

        public GameObject Prefab => prefab;
        public ShipParameters ShipPrameters => shipPrameters;
        public EnemyManeuverParameters EnemyParameters => enemyParameters;

        #endregion


        #region Var

        [SerializeField] GameObject prefab;
        [SerializeField] ShipParameters shipPrameters;
        [SerializeField] EnemyManeuverParameters enemyParameters;

        #endregion
    }
}
