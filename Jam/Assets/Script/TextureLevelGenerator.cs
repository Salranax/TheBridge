using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Color Palete
//Color(150,150,150,255) Ground
//Color(255,0,0,255) Spawn Point
//Color(0,0,0,255) Enemy
//Color(255,255,0,255) Slot
//Color(100,255,0,255) Black Hole

public class TextureLevelGenerator : MonoBehaviour
{   
    public GameManager _GameManager;
    public Texture2D map;
    public ColorToPrefab[] colorMappings;

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

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    //Get over every pixel
    void GenerateLevel(){

        _GameManager._GridSystem.generateGrid(map.width, map.height);

        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
            
        }
    }

    //Generate map from Pixels
    //Pixel List:
    // 0: Ground
    // 1: Spawn Point
    // 2: Enemy
    // 3: Slot

    void GenerateTile(int x, int y){
        Color32 pixelColor = map.GetPixel(x,y);

        if(pixelColor.a == 0){
            //Ignore if pixel is transparent
            return;
        }

        if(colorMappings[0].color.Equals(pixelColor)){
            _GameManager._GridSystem.addToGrid(gridType.floor, x, y);

            Vector2 position = new Vector2(x, y);
            GameObject _tmpGrid = Instantiate(colorMappings[0].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);
            _tmpGrid.transform.localPosition = new Vector2(x, y);  
        }
        else if(colorMappings[1].color.Equals(pixelColor)){
            _GameManager._GridSystem.addToGrid(gridType.floor, x, y);

            activeSpawnPoint = new SpawnPoint(new Vector2(x, y));
            _GameManager._PlayerController.setPlayerPoint(new Vector2(x, y));
            Vector2 position = new Vector2(x, y);
            GameObject _tmpGrid = Instantiate(colorMappings[1].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);
            _tmpGrid.transform.localPosition = new Vector2(x, y);  
        }
        else if(colorMappings[2].color.Equals(pixelColor)){
            //TODO: ENEMY SPAWN
            _GameManager._GridSystem.addToGrid(gridType.empty, x, y);
        }
        else if(colorMappings[3].color.Equals(pixelColor)){
            _GameManager._GridSystem.addToGrid(gridType.slot, x, y);

            Vector2 position = new Vector2(x, y);
            GameObject _tmpGrid = Instantiate(colorMappings[3].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);
            _tmpGrid.transform.localPosition = new Vector2(x, y); 
        }
        else if(colorMappings[4].color.Equals(pixelColor)){
            Debug.Log("black hole");
            _GameManager._GridSystem.addToGrid(gridType.blackhole, x, y);

            Vector2 position = new Vector2(x, y);
            GameObject _tmpGrid = Instantiate(colorMappings[4].prefab, position, Quaternion.identity, _GameManager._GridSystem.transform);
            _tmpGrid.transform.localPosition = new Vector2(x, y); 
        }
        else{
            Debug.Log(x + " " + y);
            Debug.Log(colorMappings[4].color);
        }

        // foreach (ColorToPrefab item in colorMappings)
        // {
        //     if(item.color.Equals(pixelColor)){
        //         Debug.Log("hops");
        //         Vector2 position = new Vector2(x, y);
        //         GameObject _tmpGrid = Instantiate(item.prefab, position, Quaternion.identity, _gridSystem.transform);
        //         _tmpGrid.transform.localPosition = new Vector2(x, y);
        //     }
        // }
    }
}
