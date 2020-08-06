using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;
using UniRx;
using System;
using SA.Pool;

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

                signalBus.Fire(new SignalGame.ChangeGameMode() { Mode = _currentGameMode });
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

            PopulatePoolObjects();

            //стартуем создание игры с задержкой
            ActionTimer(0.1f, CreateGame);
        }


        void Subscription()
        {
            //add points
            signalBus.Subscribe((SignalGame.AddPoints s) =>
            {
                points += s.PointSum;
                signalBus.Fire(new SignalGame.UpdatePointSum() { Sum = points });
            });

            //pause
            signalBus.Subscribe((SignalGame.OnClickPauseButton s) =>
            {
                SetGameMode(GameMode.PAUSE);
                Time.timeScale = 0f;
            });

            //continue game
            signalBus.Subscribe((SignalGame.OnClickContinueGameButton s) =>
            {
                SetGameMode(GameMode.GAME);
                Time.timeScale = 1f;
            });

            //meinMenu
            signalBus.Subscribe((SignalGame.OnClickMainMenuButton s) =>
            {
                LoadMeinMenu();
            });

            //restart game
            signalBus.Subscribe((SignalGame.OnClickRestartButton s) =>
            {
                GameRestart();
            });

            //palyer destroyed
            signalBus.Subscribe((SignalGame.PlayerDestroy s) =>
            {
                SetGameMode(GameMode.STOP);
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
            Time.timeScale = 1f;
        }

        #endregion


        #region Pool

        void PopulatePoolObjects()
        {
            int amount = 2;
            int amountPerTick = 2;
            int tickSize = 5;

            var enemys = dataGame.DataEnemys;

            for (int i = 0; i < enemys.Length; i++)
            {               
                PopulateEntitys(enemys[i].Prefab, amount, amountPerTick, tickSize);
            }

            var asteroids = dataGame.DataAsteroids;

            for (int i = 0; i < asteroids.Length; i++)
            {
                PopulateEntitys(asteroids[i].Prefab, amount, amountPerTick, tickSize);
            }

        }


        void PopulateEntitys(GameObject prefab, int amount, int amountPerTick, int tickSize)
        {
            PoolManager.GetInstance()
                    .Addpool(PoolType.ENTITIES)
                    .PopulateWith(prefab, amount, amountPerTick, tickSize)
                    .OnCompletedPopulateEvent += () =>
                    {
                        Debug.Log("Populate entitys");
                    };
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


        void SetGameMode(GameMode mode)
        {
            CurrentGameMode = mode;
        }

        void GameRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        void LoadMeinMenu()
        {
            SceneManager.LoadScene(StaticPrm.Scene.MAIN_MENU);
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

        void ClearScene()
        {
            unitManager?.Clear();
            asteroidGenerator?.Clear();
            Pool.PoolManager.GetInstance().Dispose();
        }


        void OnDestroy()
        {
            ClearScene();
        }


        void OnApplicationQuit()
        {
            Debug.Log("Quit");
        }

        #endregion

    }
}