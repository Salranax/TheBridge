using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance;

    [SerializeField] private float gridSize = 1.1f;
    public gridType[,] grid = new gridType[12, 30];
    public Cube[,] cubeGrid = new Cube[12, 30];
    public GameObject cubePrefab;

    void Awake()
    {
        if (instance == null)
        {
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
                if (((i >= 0 && i <= 3) && (j >= 3 && j <= 17) || (i >= 9 && i <= 12) && (j >= 3 && j <= 17)))
                {
                    grid[i, j] = gridType.empty;
                }
                else
                {
                    if ((i == 5 && j == 7))
                    {
                        grid[i, j] = gridType.slot;
                    }
                    else if (i == 7 && j == 11)
                    {
                        grid[i, j] = gridType.slot;
                    }
                    else if (i == 8 && j == 14)
                    {
                        grid[i, j] = gridType.slot;
                    }
                    else
                    {
                        GameObject tmp = Instantiate(cubePrefab);
                        tmp.transform.SetParent(this.transform);
                        tmp.transform.localPosition = new Vector3(i, j);
                        cubeGrid[i, j] = tmp.GetComponent<Cube>();
                        tmp.name = i + "/" + j;
                        grid[i, j] = gridType.floor;
                        if (j < 3 && cubeGrid[i, j] != null)
                        {
                            cubeGrid[i, j].setColor(new Color(1, 1, 1, 1));
                        }

                        else if ((j > 2 && j < 18) && cubeGrid[i, j] != null)
                        {
                            float a = 255 / 15;
                            cubeGrid[i, j].setColor(new Color((255 - a * j) / 255, (255 - a * j) / 255,
                                (255 - a * j) / 255, 1));

                            // cubeGrid[i,j].setColor(new Color32(138,138,138,255));
                        }
                        else if (j > 17 && cubeGrid[i, j] != null)
                        {
                            cubeGrid[i, j].setColor(new Color32(0,0,0,1));
                        }

                    }
                }
            }
        }

        transform.position = new Vector3(-grid.GetLength(0) / 2, -grid.GetLength(1) / 5 + 7.1f, 0);
        GameObject enemy1 = Instantiate(Resources.Load("Enemy")) as GameObject;
        enemy1.transform.SetParent(this.transform);
        enemy1.transform.localPosition = new Vector3(5, 9, -0.5f);
        enemy1.GetComponent<EnemyPatrolScript>().enemy(5, 9);
        GameObject enemy2 = Instantiate(Resources.Load("Enemy")) as GameObject;
        enemy2.transform.SetParent(this.transform);
        enemy2.transform.localPosition = new Vector3(4, 11, -0.5f);
        enemy2.GetComponent<EnemyPatrolScript>().enemy(4, 11);
        LevelGenerator.instance.spawnEnemies = new EnemyPatrolScript[2]{enemy1.GetComponent<EnemyPatrolScript>(), enemy2.GetComponent<EnemyPatrolScript>()};
    }

    public void whiten(int y)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                float a = 200 / (18 - y);
                if (j <= y && (grid[i, j] != gridType.empty && grid[i, j] != gridType.slot) )
                {
                    cubeGrid[i, j].setColor(Color.white);
                }
                else if (j < 17 && j > y && (grid[i, j] != gridType.empty && grid[i, j] != gridType.slot) &&
                         grid[i, j] != gridType.enemy)
                {
                    cubeGrid[i, j].setColor(new Color((255 - a * j) / 255, (255 - a * j) / 255,
                        (255 - a * j) / 255, 1));
                }

            }
        }
    }


    public enum gridType
    {
        empty,
        slot,
        enemy,
        floor
    }
}
