using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataEnemy", menuName = "Data/DataEnemy")]
    public class DataEnemy : ScriptableObject
    {
        #region Properties

        public GameObject Prefab => prefab;
        public ShipParameters ShipPrameters => shipPrameters;
        public EnemyParameters EnemyParameters => enemyParameters;

        #endregion


        #region Var

        [SerializeField] GameObject prefab;
        [SerializeField] ShipParameters shipPrameters;
        [SerializeField] EnemyParameters enemyParameters;

        #endregion
    }
}
