using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace SA.SpaceShooter.UI
{
    public class LevelButton : MonoBehaviour
    {       
        #region Var

        [SerializeField] Button clickButton;
        [SerializeField] Image backgraundImage;
        [SerializeField] Image statusImage;
        [SerializeField] TextMeshProUGUI nameText;       

        #endregion


        #region Init

        public void Init(Sprite backgraund, Sprite statusIcon, int index, Action callback)
        {
            backgraundImage.sprite = backgraund;
            statusImage.sprite = statusIcon;
            nameText.text = index.ToString();

            clickButton.onClick.AddListener(() =>
            {
                callback?.Invoke();
            });
        }    

        #endregion
    }
}