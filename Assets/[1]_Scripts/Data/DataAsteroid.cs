using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataAsteroid", menuName = "Data/DataAsteroid")]
    public class DataAsteroid : ScriptableObject
    {
        #region Properties

        public GameObject Prefab => prefab;
        public float MinSpeed => minSpeed;
        public float MaxSpeed => maxSpeed;

        #endregion


        #region Var

        [SerializeField] GameObject prefab;
        [SerializeField] [Range(0.1f, 20f)] float minSpeed;
        [SerializeField] [Range(0.1f, 20f)] float maxSpeed;

        #endregion
    }
}
