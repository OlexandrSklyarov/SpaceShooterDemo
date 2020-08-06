using SA.SpaceShooter.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using System.Text;
using System;

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

        [Space]
        [Header("GameOverPanel")]
        [SerializeField] Button restartButton;
        [SerializeField] GameObject gameOverPanel;


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

            liveString = new StringBuilder();
            pointString = new StringBuilder();

            InitMobileInput();
            InitTopPanel();
            InitGameOverPanel();

            Subscription();
        }


        void Subscription()
        {
            //game mode
            signalBus.Subscribe((SignalGame.ChangeGameMode s) =>
            {
                switch(s.Mode)
                {
                    case GameMode.GAME:
                        SetActiveGameOverPanel(false);
                        break;
                    case GameMode.PAUSE:
                        Debug.Log("Show pause menu :)");
                        break;
                    case GameMode.STOP:
                        SetActiveGameOverPanel(true);
                        break;
                }
            });
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


        #region Game over panel

        void InitGameOverPanel()
        {
            restartButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnPressedRestartButton());
            });

            SetActiveGameOverPanel(false);
        }

        void SetActiveGameOverPanel(bool flag)
        {
            gameOverPanel.SetActive(flag);
        }

        #endregion



        #region Top panel

        void InitTopPanel()
        {
            pauseButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnPressedPauseButton());
            });

            SetLiveText("0");
            SetPointText("0");

            //points
            signalBus.Subscribe((SignalGame.UpdatePointSum s) =>
            {
                SetPointText(s.Sum.ToString());
            });

            //player HP
            signalBus.Subscribe((SignalGame.ChangePlayerHP s) =>
            {
                SetLiveText(s.AmountHP.ToString());
            });
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