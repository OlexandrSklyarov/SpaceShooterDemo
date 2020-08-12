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

        [Space]
        [Header("WIN")]
        [SerializeField] Canvas winCanvas;

        [Space]
        [Header("pauseMenu")]
        [SerializeField] Canvas pauseMenuCanvas;
        [SerializeField] Button continueGameButton;

        [Space]
        [Header("ButtonCanvas")]
        [SerializeField] Canvas buttonCanvas;
        [SerializeField] Button mainMenuButton;
        [SerializeField] Button restartButton;

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
            InitWinPanel();
            InitPauseMenu();
            InitButtonsPanel();

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
                        EnabledButtonsCanvas(false);
                        EnabledGameInterfaceCanvas(true);
                        break;
                    case GameMode.PAUSE:
                        EnabledGameInterfaceCanvas(false);
                        EnabledPauseMenuCanvas(true);
                        EnabledButtonsCanvas(true);
                        break;
                    case GameMode.GAME_OVER:
                        EnabledGameInterfaceCanvas(false);
                        EnabledGameOverCanvas(true);
                        EnabledButtonsCanvas(true);
                        break;
                    case GameMode.GAME_WIN:
                        EnabledGameInterfaceCanvas(false);
                        EnabledWinCanvas(true);
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
            EnabledGameOverCanvas(false);
        }


        void EnabledGameOverCanvas(bool flag)
        {
            gameOverCanvas.enabled = flag;
        }

        #endregion


        #region Win panel

        void InitWinPanel()
        {
            EnabledWinCanvas(false);
        }


        void EnabledWinCanvas(bool flag)
        {
            winCanvas.enabled = flag;
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


        #region Pause menu

        void InitPauseMenu()
        {
            continueGameButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnClickContinueGameButton());
            });

            EnabledPauseMenuCanvas(false);
        }


        void EnabledPauseMenuCanvas(bool flag)
        {
            pauseMenuCanvas.enabled = flag;
        }

        #endregion


        #region Buttons canvas

        void InitButtonsPanel()
        {
            //restart
            restartButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnClickRestartButton());
            });

            //main menu
            mainMenuButton.onClick.AddListener(() =>
            {
                signalBus.Fire(new SignalGame.OnClickMainMenuButton());
            });

            EnabledPauseMenuCanvas(false);
        }


        void EnabledButtonsCanvas(bool flag)
        {
            buttonCanvas.enabled = flag;
        }

        #endregion
    }
}