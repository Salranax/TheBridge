using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance;

    [SerializeField]
    private float gridSize = 1.1f;
    public gridType[,] grid = new gridType[12,30];
    public Cube[,] cubeGrid = new Cube[12,30];
    public GameObject cubePrefab;

    void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if(((i >= 0 && i <= 3) && (j >= 3 && j <= 17) || (i >= 9 && i <= 12) && (j >= 3 && j <= 17))){
                    grid[i, j] = gridType.empty;
                }
                else{
                    if((i == 5 && j == 7)){
                        grid[i, j] = gridType.slot;
                    }
                    else if(i == 7 && j == 11){
                        grid[i, j] = gridType.slot;
                    }
                    else if(i == 8 && j == 14){
                        grid[i, j] = gridType.slot;
                    }
                    else{
                        GameObject tmp = Instantiate(cubePrefab);
                        tmp.transform.SetParent(this.transform);
                        tmp.transform.localPosition = new Vector3(i, j);
                        cubeGrid[i,j] = tmp.GetComponent<Cube>();
                        tmp.name = i + "/" + j;
                        grid[i, j] = gridType.floor;
                    }
                }
            }
        }

        transform.position = new Vector3(-grid.GetLength(0) / 2, -grid.GetLength(1) / 5 + 1.4f, 0);
        GameObject enemy1 = Instantiate(Resources.Load("Enemy")) as GameObject;
        enemy1.transform.SetParent(this.transform);
        enemy1.transform.localPosition = new Vector3(5, 9, -0.5f);
        enemy1.GetComponent<EnemyPatrolScript>().enemy(5, 9);
        grid[5,9] = gridType.enemy;
    }
}

public enum gridType
{
    empty,
    slot,
    enemy,
    floor
}
