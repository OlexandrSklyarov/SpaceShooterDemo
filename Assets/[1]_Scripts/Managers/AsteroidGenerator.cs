using System;
using System.Collections;
using System.Collections.Generic;
using SA.SpaceShooter.Data;
using UnityEngine;

public class AsteroidGenerator : MonoBehaviour
{
    #region Var

    private DataAsteroid[] dataAsteroids;
    private Transform asteroidSpawnPoints;
    private DataGame dataGame;

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
        GenerateAsteroids();
    }


    void GenerateAsteroids()
    {

    }

    #endregion

}
