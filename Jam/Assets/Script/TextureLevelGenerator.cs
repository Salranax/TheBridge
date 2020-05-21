using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Color Palete
//Color(150,150,150,255) Ground
//Color(255,0,0,255) Spawn Point
//Color(0,0,0,255) Enemy
//Color(255,255,0,255) Slot
//Color(100,255,0,255) Black Hole

public class TextureLevelGenerator : MonoBehaviour
{   
    public UnityEvent generationCallback;
    public GameManager _GameManager;
    public ObjectManager _ObjectManager;
    public Texture2D map;
    public ColorToPrefab[] colorMappings;

    private List<GameObject> _EndingFloors = new List<GameObject>();

    public struct SpawnPoint
    {
        public Vector2 spawnCoord;

        public SpawnPoint(Vector2 coord){
            spawnCoord = coord;
        }
    }

    private SpawnPoint activeSpawnPoint;

    public SpawnPoint getActiveSpawnPoint(){
        return activeSpawnPoint;
    }

    //Get over every pixel
    public void GenerateLevel(string levelData){
        map = Resources.Load("Levels/" + levelData) as Texture2D;

        _GameManager._GridSystem.generateGrid(map.width, map.height);

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
            
        }

        generationCallback.Invoke();
    }

    //Generate map from Pixels
    //Pixel List:
    // 0: Ground
    // 1: Spawn Point
    // 2: Enemy
    // 3: Slot

    void GenerateTile(int x, int y){
        Color32 pixelColor = map.GetPixel(x,y);
        Vector2 position = new Vector2(x, y);

        if(pixelColor.a == 0){
            //Ignore if pixel is transparent
            return;
        }

        if(colorMappings[0].color.Equals(pixelColor)){
            //FLOOR
            _GameManager._GridSystem.addToGrid(gridType.floor, x, y);
            
            GameObject _tmpGrid = _ObjectManager.getCubeGameObject(position, Quaternion.identity);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);
        }
        else if(colorMappings[1].color.Equals(pixelColor)){
            //SPAWN POINT
            _GameManager._GridSystem.addToGrid(gridType.floor, x, y);

            activeSpawnPoint = new SpawnPoint(new Vector2(x, y));
            _GameManager._PlayerController.setPlayerPoint(new Vector2(x, y));

            GameObject _tmpGrid = _ObjectManager.getCubeGameObject(position, Quaternion.identity);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);
        }
        else if(colorMappings[2].color.Equals(pixelColor)){
            //ENEMY
            _GameManager._GridSystem.addToGrid(gridType.floor, x, y);

            GameObject _tmpEnemy = Instantiate(colorMappings[2].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);
            GameObject _tmpFloor = _ObjectManager.getCubeGameObject(position, Quaternion.identity);

            _tmpEnemy.GetComponent<EnemyPatrolScript>().setEnemy(_GameManager._GridSystem, x, y);

            _GameManager._GridSystem.addToCubegrid(_tmpFloor, x, y);
        }
        else if(colorMappings[3].color.Equals(pixelColor)){
            //SLOT
            _GameManager._GridSystem.addToGrid(gridType.slot, x, y);

            GameObject _tmpGrid = Instantiate(colorMappings[3].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);

            _tmpGrid.transform.localPosition = new Vector2(x, y);
            _GameManager._ProgressManager.increaseGoal(); 
        }
        else if(colorMappings[4].color.Equals(pixelColor)){
            //BLACK HOLE
            _GameManager._GridSystem.addToGrid(gridType.blackhole, x, y);

            GameObject _tmpGrid = Instantiate(colorMappings[4].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);

            _tmpGrid.transform.localPosition = new Vector2(x, y); 
        }
        else if(colorMappings[5].color.Equals(pixelColor)){
            //PILLAR OF DARKNESS
            _GameManager._GridSystem.addToGrid(gridType.pillarofdarkness, x, y);

            GameObject _tmpGrid = Instantiate(colorMappings[5].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);

            _tmpGrid.transform.localPosition = new Vector2(x, y); 
        }
        else if(colorMappings[6].color.Equals(pixelColor)){
            _GameManager._GridSystem.addToGrid(gridType.pyramid, x, y);

            GameObject _tmpGrid = Instantiate(colorMappings[6].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);

            _tmpGrid.transform.localPosition = new Vector2(x, y); 
            _tmpGrid.GetComponent<Pyramid>().setPyramid(_ObjectManager);
        }
        else if(colorMappings[7].color.Equals(pixelColor)){
            _GameManager._GridSystem.addToGrid(gridType.trapdoor, x, y);

            GameObject _tmpGrid = Instantiate(colorMappings[7].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);
            _tmpGrid.GetComponent<TrapDoor>().setTrapDoor(_GameManager._GridSystem, new Vector2(x,y));
            generationCallback.AddListener(_tmpGrid.GetComponent<TrapDoor>().setTrapdoorGfx);

            _tmpGrid.transform.localPosition = new Vector2(x, y); 
        }
        else if(colorMappings[8].color.Equals(pixelColor)){
            _GameManager._GridSystem.addToGrid(gridType.stompball, x, y);

            GameObject _tmpGrid = Instantiate(colorMappings[8].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);
            _tmpGrid.GetComponent<StompBall>().gridCoord = new Vector2(x,y);

            _tmpGrid.transform.localPosition = new Vector2(x, y); 
        }
        else if(colorMappings[9].color.Equals(pixelColor)){
            _GameManager._GridSystem.addToGrid(gridType.endfloor, x, y);

            GameObject _tmpGrid = Instantiate(colorMappings[9].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);
            

            _GameManager._GridSystem.addToCubegrid(_tmpGrid, x, y);

            _tmpGrid.transform.localPosition = new Vector2(x, y); 

            _GameManager._ProgressManager.setEndGFX(_tmpGrid.transform.GetChild(0).gameObject);
            //_EndingFloors.Add(_tmpGrid);
        }
        else{

        }
    }
}
