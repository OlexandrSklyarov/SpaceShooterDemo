﻿using System;
using SA.Pool;
using SA.SpaceShooter.Data;
using UnityEngine;

namespace SA.SpaceShooter
{
    public class AsteroidGenerator
    {
        #region Var

        DataAsteroid[] dataAsteroids;
        Transform asteroidSpawnPoints;
        DataGame dataGame;

        float lastPushTime;

        #endregion


        #region Init

        public AsteroidGenerator(DataGame dataGame, Transform asteroidSpawnPoints)
        {
            this.dataGame = dataGame;
            this.asteroidSpawnPoints = asteroidSpawnPoints;
        }

        #endregion


        #region Update

        public void Tick()
        {
            if (Time.time > lastPushTime) 
            {
                GenerateAsteroids();

                lastPushTime = Time.time + dataGame.SpawnAsteroidsCoooldown;
            }
        }


        void GenerateAsteroids()
        {
            var data = GetRandomData();
            var point = GetRandomPoint();
            var asteroid = CreateAsteroid(data.Prefab, point);

            var speed = UnityEngine.Random.Range(data.MinSpeed, data.MinSpeed);
            asteroid.Push(Vector3.back * speed, dataGame.AsteroidsLifeTime);
        }


        DataAsteroid GetRandomData()
        {
            var index = UnityEngine.Random.Range(0, dataGame.DataAsteroids.Length);
            return dataGame.DataAsteroids[index];
        }


        Asteroid CreateAsteroid(GameObject prefab, Transform point)
        {

            var go = BuildManager.GetInstance().Spawn(  PoolType.ENTITIES, 
                                                        prefab, 
                                                        point.position, 
                                                        Quaternion.identity, 
                                                        null);

            return go.GetComponent<Asteroid>();
        }


        Transform GetRandomPoint()
        {
            var index = UnityEngine.Random.Range(0, asteroidSpawnPoints.childCount);
            return asteroidSpawnPoints.GetChild(index);
        }

        #endregion

    }
}