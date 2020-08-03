using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataPlayer", menuName = "Data/DataPlayer")]
    public class DataPlayer : ScriptableObject
    {
        #region Properties

        public GameObject Prefab => prefab;
        public ShipParameters ShipPrameters => shipPrameters;

        #endregion


        #region Var

        [SerializeField] GameObject prefab;
        [SerializeField] ShipParameters shipPrameters;

        #endregion
    }
}
