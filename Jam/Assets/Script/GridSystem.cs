using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance;

    //Old System
    [SerializeField] private float gridSize = 1.1f;
    public gridType[,] grid = new gridType[12, 30];
    public Cube[,] cubeGrid = new Cube[12, 30];
    //Old System END

    //Generation System
    public List<GridModule> activeModules;
    public int currentModuleIndex = 0;
    public GameObject cubePrefab;
    public GameObject scriptableModule;

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
        //TEST LEVEL CUSTOM
        // for (int i = 0; i < grid.GetLength(0); i++)
        // {
        //     for (int j = 0; j < grid.GetLength(1); j++)
        //     {
        //         if (((i >= 0 && i <= 3) && (j >= 3 && j <= 17) || (i >= 9 && i <= 12) && (j >= 3 && j <= 17)))
        //         {
        //             grid[i, j] = gridType.empty;
        //         }
        //         else
        //         {
        //             // if ((i == 5 && j == 7))
        //             // {
        //             //     grid[i, j] = gridType.slot;
        //             //     GameObject tmp = Instantiate(Resources.Load("Slot")) as GameObject;
        //             //     tmp.transform.SetParent(this.gameObject.transform);
        //             //     tmp.transform.localPosition = new Vector2(5, 7);
        //             //     LevelGenerator.instance.slots[0] = tmp.GetComponent<Slot>();
        //             //     tmp.GetComponent<Slot>().setCoord(new Vector2(5, 7));
        //             // }
        //             // else if (i == 7 && j == 11)
        //             // {
        //             //     grid[i, j] = gridType.slot;
        //             //     GameObject tmp = Instantiate(Resources.Load("Slot")) as GameObject;
        //             //     tmp.transform.SetParent(this.gameObject.transform);
        //             //     tmp.transform.localPosition = new Vector2(7, 11);
        //             //     LevelGenerator.instance.slots[1] = tmp.GetComponent<Slot>();
        //             //     tmp.GetComponent<Slot>().setCoord(new Vector2(7, 11));
        //             // }
        //             // else if (i == 8 && j == 14)
        //             // {
        //             //     grid[i, j] = gridType.slot;
        //             //     GameObject tmp = Instantiate(Resources.Load("Slot")) as GameObject;
        //             //     tmp.transform.SetParent(this.gameObject.transform);
        //             //     tmp.transform.localPosition = new Vector2(8, 14);
        //             //     LevelGenerator.instance.slots[2] = tmp.GetComponent<Slot>();
        //             //     tmp.GetComponent<Slot>().setCoord(new Vector2(8, 14));
        //             // }

        //             GameObject tmp = Instantiate(cubePrefab);
        //             tmp.transform.SetParent(this.transform);
        //             tmp.transform.localPosition = new Vector3(i, j);
        //             cubeGrid[i, j] = tmp.GetComponent<Cube>();
        //             tmp.name = i + "/" + j;
        //             grid[i, j] = gridType.floor;
        //             // if (j < 3 && cubeGrid[i, j] != null)
        //             // {
        //             //     cubeGrid[i, j].setColor(new Color(1, 1, 1, 1));
        //             // }

        //             // else if ((j > 2 && j < 18) && cubeGrid[i, j] != null)
        //             // {
        //             //     float a = 255 / 15;
        //             //     cubeGrid[i, j].setColor(new Color((255 - a * j) / 255, (255 - a * j) / 255,
        //             //         (255 - a * j) / 255, 1));

        //             //     // cubeGrid[i,j].setColor(new Color32(138,138,138,255));
        //             // }
        //             // else if (j > 17 && cubeGrid[i, j] != null)
        //             // {
        //             //     cubeGrid[i, j].setColor(new Color32(0,0,0,1));
        //             // }

                    
        //         }
        //     }
        // }

        //transform.position = new Vector3(-grid.GetLength(0) / 2, -grid.GetLength(1) / 5 + 7.1f, 0);
        //transform.position = new Vector3(-5, 0, 0);
        // GameObject enemy1 = Instantiate(Resources.Load("Enemy")) as GameObject;
        // enemy1.transform.SetParent(this.transform);
        // enemy1.transform.localPosition = new Vector3(5, 9, -0.5f);
        // enemy1.GetComponent<EnemyPatrolScript>().enemy(5, 9);
        // GameObject enemy2 = Instantiate(Resources.Load("Enemy")) as GameObject;
        // enemy2.transform.SetParent(this.transform);
        // enemy2.transform.localPosition = new Vector3(4, 11, -0.5f);
        // enemy2.GetComponent<EnemyPatrolScript>().enemy(4, 11);
        // LevelGenerator.instance.spawnEnemies = new EnemyPatrolScript[2]{enemy1.GetComponent<EnemyPatrolScript>(), enemy2.GetComponent<EnemyPatrolScript>()};
        //TEST LEVEL CUSTOM END

        generateNewPart(true);  //Starting platform 10,4
        generateNewPart(true);  //First custom module min height 5 and offset to 0

        PlayerController.instance.setPlayer(activeModules[currentModuleIndex]);

        TickManager.instance.tick.AddListener(checkNewModuleNeed);

        LevelGenerator.instance.setActiveSpawnPoint(activeModules[0], new Vector2(5, 3));
    }

    public void generateNewPart(bool first = false){
        GameObject tmpModule = Instantiate(scriptableModule) as GameObject;
        tmpModule.transform.SetParent(this.transform);
        GridModule tmpModuleScript = tmpModule.GetComponent<GridModule>();

        activeModules.Add(tmpModuleScript);
        int os = Random.Range(-2,2); //Offset of module
        tmpModuleScript.construct(Random.Range(3,6) , Random.Range(8,15), os, first);
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

    private void checkNewModuleNeed(){
        if(activeModules.Count < 4){
            generateNewPart();
        }
        //Destroy modules before last spawn point
    }

    public GridModule getNextModule(){
        return activeModules[currentModuleIndex + 1];
    }

    public void setActivemodule(GridModule gm){
        currentModuleIndex = activeModules.IndexOf(gm);
    }

    public void moveToNextModule(){
        currentModuleIndex ++;
    }

    public bool canPassToNext(GridModule cur, GridModule nxt, Vector2 cCoord){
        int nextOffset = Mathf.Abs(nxt.offset);
        if(cCoord.x - nxt.offset < nxt.gridSizeX && cCoord.x - nxt.offset >= 0){
            return true;
        }
        else{
            return false;
        }
    }

    public Vector2 positionOnNextGrid(GridModule cur, GridModule nxt, Vector2 cCoord){
        Debug.Log(nxt.offset + " / " + cCoord.x);
        return new Vector2(cCoord.x - nxt.offset, 0);
    }

}

public enum gridType
{
    empty,
    slot,
    enemy,
    floor,
    trap
}
