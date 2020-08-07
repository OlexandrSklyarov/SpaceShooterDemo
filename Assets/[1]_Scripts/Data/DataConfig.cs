using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataConfig", menuName = "Data/DataConfig")]
    public class DataConfig : ScriptableObject
    {
        #region Properties

        public int Amount => amount;
        public int AmountPerTick => amountPerTick;
        public int TickSize => tickSize;

        #endregion


        #region Var

        [Header("Pool settings")]
        [SerializeField] [Range(1, 50)]int amount = 1;
        [SerializeField] [Range(1, 50)] int amountPerTick = 1;
        [SerializeField] [Range(1, 100)] int tickSize = 1;

        #endregion
    }
}