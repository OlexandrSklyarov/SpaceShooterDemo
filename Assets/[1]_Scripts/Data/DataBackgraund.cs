using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(menuName = "Data/DataBackgraund", fileName = "DataBackgraund")]
    public class DataBackgraund : ScriptableObject
    {
        #region Properties

        public float ScrollSpeed => scrollSpeed;
        public float TileSize => tileSize;

        #endregion


        #region Var

        [SerializeField] [Range(0.1f, 60f)] float scrollSpeed = 0.1f;
        [SerializeField] [Range(1f, 500f)] float tileSize = 1f;

        #endregion
    }
}