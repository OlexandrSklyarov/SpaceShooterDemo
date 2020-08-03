using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter
{
    public class GameManager : MonoBehaviour
    {
        #region Var
       
        DataConfig config;
        SignalBus signalBus;
        UnitManager unitManager;

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
                                            asteroidSpawnPoints, 
                                            signalBus);
        }




        #endregion


        #region Update

        void Update()
        {
            unitManager.Tick();
        }


        void FixedUpdate()
        {
            unitManager.FixedTick();
        }

        #endregion

    }
}