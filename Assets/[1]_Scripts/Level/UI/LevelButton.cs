using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SA.SpaceShooter.UI
{
    public class LevelButton : MonoBehaviour
    {
        #region Properties

        public Button SelectButton => btn; 

        #endregion


        #region Var

        [SerializeField] Button btn;
        [SerializeField] Image backgraundImage;
        [SerializeField] Image statusImage;
        [SerializeField] TextMeshProUGUI nameText;       

        #endregion


        #region Init

        public void Init(Sprite backgraund, Sprite statusIcon, int index)
        {
            backgraundImage.sprite = backgraund;
            statusImage.sprite = statusIcon;
            nameText.text = index.ToString();
        }

        #endregion
    }
}