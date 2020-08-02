using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataGame", menuName = "Data/DataGame")]
    public class DataGame : ScriptableObject
    {
        #region Properties

        public DataPlayer DataPlayer => dataPlayer;
        public DataEnemy[] DataEnemys => dataEnemys;
        public DataAsteroid[] DataAsteroids => dataAsteroids;

        #endregion


        #region Var

        [SerializeField] DataPlayer dataPlayer;
        [SerializeField] DataEnemy[] dataEnemys;
        [SerializeField] DataAsteroid[] dataAsteroids;

        #endregion
    }
}
