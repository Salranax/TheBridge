using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolScript : MonoBehaviour
{
    public int gridX,gridY;
    private int dir = 1;

    public void enemy(int gx, int gy){
        gridX = gx;
        gridY = gy;
    }

    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(moveEnemy);
    }

    public void moveEnemy(){
        Debug.Log("Enemy");
        if(GridSystem.instance.grid[gridX + 1, gridY] == gridType.floor){

        }
        else{
            dir *= -1;
        }

        transform.localPosition = new Vector3(gridX + dir, gridY, -0.5f);
        gridX = gridX + dir;
        GridSystem.instance.grid[gridX, gridY] = gridType.enemy;
    }
}
