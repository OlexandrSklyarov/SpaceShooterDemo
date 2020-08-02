using System;
using System.Collections;
using System.Collections.Generic;
using SA.Pool;
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

        PlayerShip player;
        EnemyController enemyController;
        AsteroidGenerator asteroidGenerator;

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

            CreatePlayer(dataGame.DataPlayer, playerSpawnPoint);
        }


        //создаём игрока через пул и настраиваем его
        void CreatePlayer(DataPlayer dataPlayer, Transform playerSpawnPoint)
        {
            var go = BuildManager.GetInstance().Spawn(  PoolType.ENTITIES, 
                                                        dataPlayer.Prefab, 
                                                        playerSpawnPoint.position, 
                                                        playerSpawnPoint.rotation, 
                                                        null);
            player = go.GetComponent<PlayerShip>();
            player.Init(dataPlayer.Speed, dataPlayer.MaxHP, signalBus);
        }


        #endregion


        #region Update

        void Update()
        {
            player.Tick();
        }

        #endregion

    }
}