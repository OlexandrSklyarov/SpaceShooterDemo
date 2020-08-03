using SA.Pool;
using SA.SpaceShooter;
using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;

public class UnitManager
{
    #region Var

    DataGame dataGame;
    Transform playerSpawnPoint;
    Transform enemySpawnPoints;
    Transform asteroidSpawnPoints;
    SignalBus signalBus;

    BaseShip player;
    EnemyController enemyController;
    AsteroidGenerator asteroidGenerator;

    #endregion


    #region Init

    public UnitManager( DataGame dataGame, 
                        Transform playerSpawnPoint, 
                        Transform enemySpawnPoints, 
                        Transform asteroidSpawnPoints, 
                        SignalBus signalBus)
    {
        this.dataGame = dataGame;
        this.playerSpawnPoint = playerSpawnPoint;
        this.enemySpawnPoints = enemySpawnPoints;
        this.asteroidSpawnPoints = asteroidSpawnPoints;
        this.signalBus = signalBus;

        CreatePlayer();
        CreateEnemyController();
        CreateAsteroidController();
    }


    private void CreateAsteroidController()
    {

    }


    private void CreateEnemyController()
    {

    }


    //создаём игрока через пул и настраиваем его
    void CreatePlayer()
    {
        var go = BuildManager.GetInstance().Spawn(  PoolType.ENTITIES,
                                                    dataGame.DataPlayer.Prefab,
                                                    playerSpawnPoint.position,
                                                    playerSpawnPoint.rotation,
                                                    null);
        player = go.GetComponent<BaseShip>();
        player.Init(dataGame.DataPlayer.ShipPrameters, dataGame.MapSize, signalBus);
    }


    #endregion


    #region Update

    public void Tick() 
    {
        player.Tick();
    }


    public void FixedTick() 
    {
        player.FixedTick();
    }

    #endregion
}
