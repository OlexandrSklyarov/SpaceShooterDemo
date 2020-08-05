using SA.SpaceShooter.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using System.Text;

namespace SA.SpaceShooter.UI
{
    public class ManagerUI : MonoBehaviour
    {
        #region Var

        [Header("Input")]
        [SerializeField] GameObject mobileInputPanel;

        [Space]
        [Header("TopPanel")]
        [SerializeField] Button pauseButton;
        [SerializeField] TextMeshProUGUI pointText;
        [SerializeField] TextMeshProUGUI liveText;


        DataConfig config;
        SignalBus signalBus;

        UnitManager unitManager;
        AsteroidGenerator asteroidGenerator;

        StringBuilder liveString;
        StringBuilder pointString;

        #endregion


        #region Init

        [Inject]
        public void Construct(DataConfig config, SignalBus signalBus)
        {
            this.config = config;
            this.signalBus = signalBus;

            InitMobileInput();
            InitTopPanel();
        }

        #endregion


        #region Input panel

        void InitMobileInput()
        {

#if UNITY_EDITOR
            mobileInputPanel.SetActive(false);
#endif

        }

        #endregion


        #region Top panel

        void InitTopPanel()
        {
            pauseButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnPressedPauseButton());
            });

            liveString = new StringBuilder();
            pointString = new StringBuilder(); 

            SetLiveText("0");
            SetPointText("0");
        }


        void SetLiveText(string txt)
        {
            liveString.Clear();
            liveString.Append(txt);
            liveText.text = liveString.ToString();
        }


        void SetPointText(string txt)
        {
            pointString.Clear();
            pointString.Append(txt);
            pointText.text = pointString.ToString();
        }

        #endregion
    }
}