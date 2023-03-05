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
        [SerializeField] [Range(1f, 1000f)] float minSpeed = 0.1f;
        [SerializeField] [Range(1f, 1000f)] float maxSpeed = 0.1f;

        #endregion
    }
}
