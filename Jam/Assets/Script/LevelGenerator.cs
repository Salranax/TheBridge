using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public Slot[] slots;
    public int spotOrder;
    public EnemyPatrolScript[] spawnEnemies;
    public int spawnEnemyNumber = 0;
    public bool isObjectiveComplete = false;

    public void generateLevel(GameObject[,] arry){
        
    }

    private void Start() {
        slots = new Slot[3];
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
        if(spotOrder + 2 <= slots.Length){
           spotOrder ++;
        }
        else{
            isObjectiveComplete = true;
        }
    }
}
