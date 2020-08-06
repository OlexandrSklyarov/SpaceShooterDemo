using System.Collections.Generic;
using SA.Pool;
using SA.SpaceShooter.Data;
using UnityEngine;
using Zenject;

namespace SA.SpaceShooter
{
    public class AsteroidGenerator
    {
        #region Var

        DataAsteroid[] dataAsteroids;
        Transform asteroidSpawnPoints;
        DataGame dataGame; 
        SignalBus signalBus;

        List<Asteroid> asteroids;

        float lastPushTime;

        #endregion


        #region Init

        public AsteroidGenerator(DataGame dataGame, Transform asteroidSpawnPoints, SignalBus signalBus)
        {
            this.dataGame = dataGame;
            this.asteroidSpawnPoints = asteroidSpawnPoints;
            this.signalBus = signalBus;

            asteroids = new List<Asteroid>();
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
            asteroid.Push(Vector3.back * speed, dataGame.AsteroidsLifeTime, dataGame.AddPoints, signalBus);
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

            //подписываемся на событие удаления и добавляем в список
            var asteroid = go.GetComponent<Asteroid>();
            asteroid.OnDestroyAsteroids += (ast) => asteroids.Remove(ast);
            asteroids.Add(asteroid);

            return asteroid;
        }


        Transform GetRandomPoint()
        {
            var index = UnityEngine.Random.Range(0, asteroidSpawnPoints.childCount);
            return asteroidSpawnPoints.GetChild(index);
        }

        #endregion


        #region Clear

        public void Clear()
        {
            if (asteroids != null)
            {
                for (int i = 0; i < asteroids.Count; i++)
                {
                    if (asteroids[i] != null)
                    {
                        UnityEngine.Object.Destroy(asteroids[i].gameObject);
                    }
                }

                asteroids.Clear();
            }
        }

        #endregion

    }
}