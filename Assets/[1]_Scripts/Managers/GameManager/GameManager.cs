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
                SignalChangeMode();
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

        int completedPopulateProcess;
        int maxPopulateProcess;
        bool isPopulate;

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
                SignalPlayMenuMusic();
                Time.timeScale = 0f;
            });

            //continue game
            signalBus.Subscribe((SignalGame.OnClickContinueGameButton s) =>
            {
                PlayGame();
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

            PlayGame();
        }


        void PlayGame()
        {
            SetGameMode(GameMode.GAME);
            SignalPlayGameMusic();
            Time.timeScale = 1f;
        }

        #endregion


        #region Pool

        //заполняет пул обектами до начала игры
        void PopulatePoolObjects()
        {
            var bm = BuildManager.GetInstance();

            int amount = config.Amount;
            int amountPerTick = config.AmountPerTick;
            int tickSize = config.TickSize;

            var enemys = dataGame.DataEnemys;
            var asteroids = dataGame.DataAsteroids;

            maxPopulateProcess = asteroids.Length + enemys.Length;

            for (int i = 0; i < enemys.Length; i++)
            {
               bm.PopulateEntitys(Pool.PoolType.ENTITIES, 
                                   enemys[i].Prefab, 
                                   amount, 
                                   amountPerTick, 
                                   tickSize, 
                                   CheckPopulateProcess);
            }

            for (int i = 0; i < asteroids.Length; i++)
            {
                bm.PopulateEntitys(Pool.PoolType.ENTITIES,
                                    asteroids[i].Prefab,
                                    amount,
                                    amountPerTick,
                                    tickSize,
                                    CheckPopulateProcess);
            }
        }


        //проверяет завершены ли все процессы по заполнению пула, и стартует игру
        void CheckPopulateProcess()
        {
            completedPopulateProcess++;
                       
            if (completedPopulateProcess >= maxPopulateProcess && !isPopulate)
            {
                //стартуем создание игры с задержкой
                ActionTimer(0.1f, CreateGame);
                isPopulate = true;
            }
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


        #region Signal

        void SignalChangeMode()
        {
            signalBus.Fire(new SignalGame.ChangeGameMode() { Mode = CurrentGameMode });
        }


        void SignalPlayGameMusic()
        {
            signalBus.Fire(new SignalGame.PlayMusic_Game());
        }


        void SignalPlayMenuMusic()
        {
            signalBus.Fire(new SignalGame.PlayMusicGameMenu());
        }

        #endregion


        #region Exit

        void OnDestroy()
        {
            unitManager?.Clear();
            asteroidGenerator?.Clear();
            BuildManager.GetInstance().Clear();
        }


        void OnApplicationQuit()
        {
            Debug.Log("Quit");
        }

        #endregion

    }
}