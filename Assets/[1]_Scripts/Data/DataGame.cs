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
        public MapSize MapSize => mapSize;
        public float SpawnEnemyCoooldown => spawnEnemyCoooldown;
        public float SpawnAsteroidsCoooldown => spawnAsteroidsCoooldown;

        #endregion


        #region Var

        [SerializeField] DataPlayer dataPlayer;
        [SerializeField] DataEnemy[] dataEnemys;
        [SerializeField] DataAsteroid[] dataAsteroids;
        [SerializeField] MapSize mapSize;

        [Space]
        [SerializeField] [Range(0.1f, 10f)] float spawnEnemyCoooldown = 0.7f;

        [Space]
        [SerializeField] [Range(0.1f, 10f)] float spawnAsteroidsCoooldown = 0.7f;

        #endregion
    }
}
