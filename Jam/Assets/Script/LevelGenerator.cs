﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;

    public struct SpawnPoint
    {
        public Vector2 spawnCoord;
        public GridModule spawnModule;

        public SpawnPoint(Vector2 coord, GridModule md){
            spawnCoord = coord;
            spawnModule = md;
        }
    }

    private SpawnPoint activeSpawnPoint;

    public int spotOrder;
    public EnemyPatrolScript[] spawnEnemies;
    public int spawnEnemyNumber = 0;
    public bool isObjectiveComplete = false;

    public void generateLevel(GameObject[,] arry){
        
    }

    private void Start() {

    }

    private void Awake() {
        if(instance == null){
            instance = this;
        }
    }

    public Vector2 getSpawnCoord(){   
        return new Vector2(spawnEnemies[spawnEnemyNumber].gridX, spawnEnemies[spawnEnemyNumber].gridY);

    }

    public void setNextSpawnPoint(){
        if(spawnEnemies.Length - 1 > spawnEnemyNumber){
            spawnEnemyNumber ++;
        }
    }

    public void setEnemyPlayer(){
        spawnEnemies[spawnEnemyNumber].activateSpawnPoint();
    }

    public void increaseSpotOrder(){

    }

    public void setActiveSpawnPoint(GridModule mod, Vector2 point){
        activeSpawnPoint = new SpawnPoint(point, mod);
    }

    public SpawnPoint getActiveSpawnPoint(){
        return activeSpawnPoint;
    }
}
