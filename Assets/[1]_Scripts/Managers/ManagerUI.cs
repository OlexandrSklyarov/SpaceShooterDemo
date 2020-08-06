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

        [Header("GameInterface")]
        [SerializeField] GameObject mobileInputPanel;

        [Space]
        [SerializeField] Canvas gameInterfaceCanvas;
        [SerializeField] Button pauseButton;
        [SerializeField] TextMeshProUGUI pointText;
        [SerializeField] TextMeshProUGUI liveText;

        [Space]
        [Header("GameOver")]
        [SerializeField] Canvas gameOverCanvas;
        [SerializeField] Button restartButton;

        [Space]
        [Header("pauseMenu")]
        [SerializeField] Canvas pauseMenuCanvas;
        [SerializeField] Button mainMenuButton;
        [SerializeField] Button continueGameButton;

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
            InitPauseMenu();

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
                        EnabledGameOverCanvas(false);
                        EnabledPauseMenuCanvas(false);
                        EnabledGameInterfaceCanvas(true);
                        break;
                    case GameMode.PAUSE:
                        EnabledPauseMenuCanvas(true);
                        break;
                    case GameMode.STOP:
                        EnabledGameOverCanvas(true);
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
                signalBus.Fire(new SignalGame.OnClickRestartButton());
            });

            EnabledGameOverCanvas(false);
        }

        void EnabledGameOverCanvas(bool flag)
        {
            gameOverCanvas.enabled = flag;
        }

        #endregion



        #region Top panel

        void InitTopPanel()
        {
            pauseButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnClickPauseButton());
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


        void EnabledGameInterfaceCanvas(bool flag)
        {
            gameInterfaceCanvas.enabled = flag;
        }

        #endregion


        #region Top panel

        void InitPauseMenu()
        {
            continueGameButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnClickContinueGameButton());
            });


            mainMenuButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnClickMainMenuButton());
            });

            EnabledPauseMenuCanvas(false);
        }


        void EnabledPauseMenuCanvas(bool flag)
        {
            pauseMenuCanvas.enabled = flag;
        }

        #endregion
    }
}