using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public Vector2[] spots;
    public int spotOrder;
    public EnemyPatrolScript[] spawnEnemies;
    public int spawnEnemyNumber = 0;
    public bool isObjectiveComplete = false;

    public void generateLevel(GameObject[,] arry){
        
    }

    private void Start() {
        spots = new Vector2[3]{new Vector2(5,7), new Vector2(7,11), new Vector2(8,14)};
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
        if(spotOrder + 2 <= spots.Length){
           spotOrder ++;
        }
        else{
            isObjectiveComplete = true;
        }
    }
}
