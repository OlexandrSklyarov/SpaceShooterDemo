using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using UniRx;
using System;

namespace SA.SpaceShooter
{
    public class GameManager : MonoBehaviour
    {
        #region Properties

        GameMode CurrentGameMode
        {
            get => _currentGameMode;
            set
            {
                _currentGameMode = value;

                signalBus.Fire(new SignalGame.ChangeGameMode() { Mode = _currentGameMode});
            }
        }

        #endregion


        #region Var

        DataGame dataGame;
        DataConfig config;
        SignalBus signalBus;
        ScrollBackgraund backgraund;
        UnitManager unitManager;
        AsteroidGenerator asteroidGenerator;

        Transform playerSpawnPoint;
        Transform enemySpawnPoints;
        Transform asteroidSpawnPoints;

        GameMode _currentGameMode;

        int points;

        #endregion


        #region Init

        [Inject]
        public void Construct(DataGame dataGame,
                                DataConfig config,
                                SignalBus signalBus,
                                ScrollBackgraund backgraund,
                                [Inject(Id = "Player_SP")] Transform playerSpawnPoint,
                                [Inject(Id = "Enemy_SP")] Transform enemySpawnPoints,
                                [Inject(Id = "Asteroid_SP")] Transform asteroidSpawnPoints)
        {
            this.dataGame = dataGame;
            this.config = config;
            this.signalBus = signalBus;
            this.backgraund = backgraund;

            this.playerSpawnPoint = playerSpawnPoint;
            this.enemySpawnPoints = enemySpawnPoints;
            this.asteroidSpawnPoints = asteroidSpawnPoints;

            Subscription();

            //стартуем создание игры с задержкой
            ActionTimer(0.1f, CreateGame);
        }


        void Subscription()
        {
            signalBus.Subscribe((SignalGame.AddPoints s) =>
            {
                points += s.PointSum;
                signalBus.Fire(new SignalGame.UpdatePointSum() { Sum = points});

            });

            //pause
            signalBus.Subscribe((SignalGame.OnPressedPauseButton s) =>
            {
                CurrentGameMode = GameMode.PAUSE;
            });

            //pause
            signalBus.Subscribe((SignalGame.OnPressedRestartButton s) =>
            {
                GameRestart();
            });

            //palyer destroyed
            signalBus.Subscribe((SignalGame.PlayerDestroy s) =>
            {
                CurrentGameMode = GameMode.STOP;
            });
        }

        void CreateGame()
        {
            unitManager = new UnitManager(dataGame,
                                           playerSpawnPoint,
                                           enemySpawnPoints,
                                           signalBus);

            asteroidGenerator = new AsteroidGenerator(dataGame, asteroidSpawnPoints, signalBus);

            CurrentGameMode = GameMode.GAME;
        }

        #endregion


        #region Update

        void Update()
        {
            if (!IsGame()) return;

            backgraund.Tick();
            unitManager.Tick();
            asteroidGenerator.Tick();
        }


        void FixedUpdate()
        {
            if (!IsGame()) return;

            unitManager.FixedTick();
        }


        bool IsGame()
        {
            return CurrentGameMode == GameMode.GAME;
        }

        #endregion


        #region Game procces

        void GameRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #endregion


        #region Timer

        protected void ActionTimer(float time, Action act)
        {
            Observable.Timer(TimeSpan.FromSeconds(time))
            .Subscribe(_ =>
            {
                act?.Invoke();
            })
            .AddTo(this);
        }


        #endregion


        #region Exit

        void OnApplicationQuit()
        {
            Debug.Log("Quit");
        }

        #endregion

    }
}