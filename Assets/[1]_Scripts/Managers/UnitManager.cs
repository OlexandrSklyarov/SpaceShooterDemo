using System;
using System.Collections.Generic;
using SA.Pool;
using SA.SpaceShooter;
using SA.SpaceShooter.Data;
using SA.SpaceShooter.Ship;
using UnityEngine;
using Zenject;

public class UnitManager
{
    #region Var

    DataGame dataGame;
    Transform playerSpawnPoint;
    Transform enemySpawnPoints;
    SignalBus signalBus;
    
    List<BaseShip> ships;

    float lastSpawnTime;

    bool isInit;

    #endregion


    #region Init

    public UnitManager( DataGame dataGame, 
                        Transform playerSpawnPoint, 
                        Transform enemySpawnPoints, 
                        SignalBus signalBus)
    {
        this.dataGame = dataGame;
        this.playerSpawnPoint = playerSpawnPoint;
        this.enemySpawnPoints = enemySpawnPoints;
        this.signalBus = signalBus;

        ships = new List<BaseShip>();

        CreatePlayer();

        isInit = true;
    }


    //создаём игрока через пул и настраиваем его
    void CreatePlayer()
    {
        var go = BuildManager.GetInstance().Spawn(  PoolType.ENTITIES,
                                                    dataGame.DataPlayer.Prefab,
                                                    playerSpawnPoint.position,
                                                    playerSpawnPoint.rotation,
                                                    null);

        var player = go.GetComponent<PlayerShip>();
        player.Init(dataGame.DataPlayer.ShipPrameters, dataGame.MapSize, signalBus);

        //подписываемся на удаление данного коробля из списка
        player.OnShipDestroy += (ship) =>
        {
            ships.Remove(ship);
        };

        ships.Add(player);
    }


    #endregion


    #region Update

    public void Tick() 
    {
        if (!isInit) return;

        GenerateEnemy();

        for (int i = 0; i < ships.Count; i++)
        {
            ships[i].Tick();
        }
    }


    public void FixedTick() 
    {
        if (!isInit) return;

        for (int i = 0; i < ships.Count; i++)
        {
            ships[i].FixedTick();
        }
    }

    #endregion


    #region Spawn

    void GenerateEnemy()
    {
        if (Time.time > lastSpawnTime)
        {
            var enemyShip = CreateEnemyShip();

            //подписываемся на удаление данного коробля из списка
            enemyShip.OnShipDestroy += (ship) =>
            {
                ships.Remove(ship);
            };

            ships.Add(enemyShip);

            lastSpawnTime = Time.time + dataGame.SpawnEnemyCoooldown;
        }
    }


    //истекло ли currentTime
    bool IsTimeEnd(ref float curentTime, float timer)
    {
        if (Time.time > curentTime)
        {
            curentTime = Time.time + timer;
            return true;
        }

        return false;
    }


    BaseShip CreateEnemyShip()
    {
        var enemyData = RandomEnemyData();

        Transform spawnPoint = RandomSpawnPoint();

        var go = BuildManager.GetInstance().Spawn( PoolType.ENTITIES,
                                                   enemyData.Prefab,
                                                   spawnPoint.position,
                                                   spawnPoint.rotation,
                                                   null);

        var ship = go.GetComponent<EnemyShip>();

        ship.Init(  enemyData.ShipPrameters, 
                    dataGame.MapSize, 
                    signalBus, 
                    enemyData.EnemyParameters,
                    dataGame.AddPoints);

        return ship;
    }


    //возвращает данные случайного врага
    DataEnemy RandomEnemyData()
    {
        int enemyIndex = UnityEngine.Random.Range(0, dataGame.DataEnemys.Length);
        return dataGame.DataEnemys[enemyIndex];
    }


    //возвращает случайную spawn точку
    Transform RandomSpawnPoint()
    {
        int index = UnityEngine.Random.Range(0, enemySpawnPoints.childCount);
        return enemySpawnPoints.GetChild(index);
    }

    #endregion


    #region Clear

    public void Clear()
    {
        if (ships != null)
        {
            for(int i = 0; i < ships.Count; i++)
            {
                if (ships[i] != null)
                {
                    UnityEngine.Object.Destroy(ships[i].gameObject);
                }
            }

            ships.Clear();
        }
    }

    #endregion
}
