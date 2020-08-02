using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataEnemy", menuName = "Data/DataEnemy")]
    public class DataEnemy : ScriptableObject
    {
        #region Properties

        public GameObject Prefab => prefab;
        public float Speed => speed;
        public int MaxHP => maxHP;

        #endregion


        #region Var

        [SerializeField] GameObject prefab;
        [SerializeField] [Range(0.1f, 20f)] float speed;
        [SerializeField] [Range(1, 10)] int maxHP;

        #endregion
    }
}
