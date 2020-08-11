using UnityEngine;

namespace SA.SpaceShooter.Data
{
    [CreateAssetMenu(fileName = "DataLevelUI", menuName = "Data/DataLevelUI")]
    public class DataLevelUI : ScriptableObject
    {
        #region Properties

        public GameObject LevelTemplatePrefab => levelTemplatePrefab;
        public Sprite CompletedLevelBGR => completedLevelBGR;
        public Sprite OpenLevelBGR => openLevelBGR;
        public Sprite CloseLevelBGR => closeLevelBGR;
        public Sprite CompletedLevelIcon => completedLevelIcon;
        public Sprite OpenLevelIcon => openLevelIcon;
        public Sprite CloseLevelIcon => closeLevelIcon;

        #endregion


        #region Var

       [Header("Level template")]
        [SerializeField] GameObject levelTemplatePrefab;

        [Space]
        [Header("Button backgraunds")]
        [SerializeField] Sprite completedLevelBGR;
        [SerializeField] Sprite openLevelBGR;
        [SerializeField] Sprite closeLevelBGR;

        [Space]
        [Header("Button icons")]
        [SerializeField] Sprite completedLevelIcon;
        [SerializeField] Sprite openLevelIcon;
        [SerializeField] Sprite closeLevelIcon;

        #endregion
    }
}
