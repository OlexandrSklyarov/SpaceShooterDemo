using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter
{
    public class GameManager : MonoBehaviour
    {
        #region Data

        enum GameMode { PAUSE, GAME }

        #endregion


        #region Var

        DataConfig config;
        SignalBus signalBus;

        UnitManager unitManager; 
        AsteroidGenerator asteroidGenerator;

        GameMode gameMode;

        #endregion


        #region Init

        [Inject]
        public void Construct(  DataGame dataGame,
                                DataConfig config,
                                SignalBus signalBus,
                                [Inject(Id = "Player_SP")] Transform playerSpawnPoint,
                                [Inject(Id = "Enemy_SP")] Transform enemySpawnPoints,
                                [Inject(Id = "Asteroid_SP")] Transform asteroidSpawnPoints)
        {
            this.config = config;
            this.signalBus = signalBus;

            unitManager = new UnitManager(  dataGame, 
                                            playerSpawnPoint, 
                                            enemySpawnPoints,  
                                            signalBus);
           
            asteroidGenerator = new AsteroidGenerator(dataGame, asteroidSpawnPoints);

            gameMode = GameMode.GAME;
        }

        #endregion


        #region Update

        void Update()
        {
            if (!IsGame()) return;

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
            return gameMode == GameMode.GAME;
        }

        #endregion

    }
}