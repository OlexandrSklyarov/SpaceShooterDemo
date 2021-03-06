﻿using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

namespace SA.SpaceShooter
{
    public class GameManager : MonoActionTimer
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

        GameSettings settings;

        int points;

        //asteroids
        int maxDestroyAsteroids;
        int countDestroyAsteroids;

        //pool
        int completedPopulateProcess;
        int maxPopulateProcess;
        bool isPopulate;
        
        bool isInit;

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

            settings = GameSettings.GetInstance();

            Subscription();

            PopulatePoolObjects();
        }


        void Subscription()
        {
            //add points
            signalBus.Subscribe((SignalGame.AddPoints s) =>
            {
                AddPoints(s.PointSum);
            });

            //pause
            signalBus.Subscribe((SignalGame.OnClickPauseButton s) =>
            {
                Pause();
            });

            //continue game
            signalBus.Subscribe((SignalGame.OnClickContinueGameButton s) =>
            {
                StartGame();
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
                GameOver();
            });

            //destroy asteroid
            signalBus.Subscribe<SignalGame.DestroyAsteroid>(() =>
            {
                countDestroyAsteroids++;

                if (countDestroyAsteroids >= maxDestroyAsteroids)
                {
                    Win();
                }
            });
        }


        void CreateGame()
        {
            Setup();
            StartGame();
        }


        void Setup()
        {
            CreateUnitmanager();
            CreateAsteroidGenerator();
            SetupStartGamePrm();

            isInit = true;
        }


        void CreateUnitmanager()
        {
            if (unitManager != null) return;

            unitManager = new UnitManager(
                dataGame,playerSpawnPoint,enemySpawnPoints,signalBus);
        }


        void CreateAsteroidGenerator()
        {
            if (asteroidGenerator != null) return;

            asteroidGenerator = new AsteroidGenerator(
                dataGame, asteroidSpawnPoints, signalBus);
        }


        //устанавливает стартовые значения очков и количества уничтоженых астероидов
        void SetupStartGamePrm()
        {
            if(isInit) return;

            points = 0;

            //count asteroids
            var curLevel = settings.Levels[settings.CurrentLevelIndex]; 
            maxDestroyAsteroids = curLevel.maxDestroyAsteroids;
            countDestroyAsteroids = 0;
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
            return CurrentGameMode == GameMode.GAME && isInit;
        }


        #endregion


        #region Game progress


        void SetGameMode(GameMode mode)
        {
            Debug.Log($"Set game mode:{mode}");
            CurrentGameMode = mode;
        }


        void ResetGame()
        {
            Time.timeScale = 1f;
        }


        void StartGame()
        {
            if (!isInit) Setup();
            SignalPlayGameMusic();
            ResetGame();
            SetGameMode(GameMode.GAME);
        }


        void Pause()
        {
            SetGameMode(GameMode.PAUSE);
            SignalPlayMenuMusic();
            Time.timeScale = 0f;
        }        

        

        void GameRestart()
        {
            ResetGame();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }


        void GameOver()
        {
            SetGameMode(GameMode.GAME_OVER);
            signalBus.Fire(new SignalGame.PlaySFX_GameOver());
        }


        void Win()
        {
            if (CurrentGameMode == GameMode.GAME_WIN) return;

            SetGameMode(GameMode.GAME_WIN);
            signalBus.Fire(new SignalGame.PlayMusic_Win());

            SetStatusLevel();

            //выходим в главное меню по таймеру
            ActionTimer(config.WinDeleyTime, LoadMeinMenu);
        }


        void SetStatusLevel()
        {
            var curIndex = settings.CurrentLevelIndex;

            //устанавливаем статус текущему уровню как пройденый
            settings.Levels[curIndex].status = Level.LevelStatus.COMPLETED;

            //если текущий индекс уровня не последний
            //открываем следующий уровень
            if (curIndex < settings.Levels.Length-1)
            {
                settings.Levels[curIndex + 1].status = Level.LevelStatus.OPEN;
            }
        }


        void LoadMeinMenu()
        {
            ResetGame();
            SceneManager.LoadSceneAsync(StaticPrm.Scene.MAIN_MENU);
        }


        void AddPoints(int sum)
        {
            points += sum;

            //если мы обновили рекорд, перезаписываем его
            if (points > GameSettings.GetInstance().PointRecord)
                GameSettings.GetInstance().PointRecord = points;

            signalBus.Fire(new SignalGame.UpdatePointSum() { Sum = points });
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
            DestroyAllBullets();
            unitManager?.Clear();
            asteroidGenerator?.Clear();
            BuildManager.GetInstance().Clear();

            OnDispose();

            isInit = false;
        }


        void DestroyAllBullets()
        {
            var bullets = FindObjectsOfType<Bullet>();

            for (int i = 0; i < bullets.Length; i++)
                if (bullets[i].gameObject.activeInHierarchy)
                    Destroy(bullets[i].gameObject);
        }


        void OnApplicationQuit()
        {
            Debug.Log("Quit");
        }

        #endregion

    }
}