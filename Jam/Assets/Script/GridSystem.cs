using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance;
    public ObjectManager _ObjectManager;
    public TextureLevelGenerator _TextureLevelGenerator;

    public gridType[,] grid;
    public GameObject[,] cubeGrid;

    //Generation System
    public GameObject endingEffect;
    private List<GameObject> endingGrid;

    void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
    }

    public void generateGrid(int sizeX, int sizeY){
        grid = new gridType[sizeX, sizeY];
        cubeGrid = new GameObject[sizeX, sizeY];
    }

    public void addToGrid(gridType _type, int _x, int _y){
        grid[_x, _y] = _type;
    }

    public void addToCubegrid(GameObject _cube, int _x, int _y){
        cubeGrid[_x, _y] = _cube;
    }

    public GameObject getGridGameobject(int x, int y){
        if(cubeGrid[x,y] != null){
            return cubeGrid[x,y];
        }
        else{
            return null;
        }

    }

    public gridType getGridType(int x, int y){
        //Validation for next position in grid borders 
        if(x >= grid.GetLength(0) || x < 0 || y < 0 || y >= grid.GetLength(1) ){
            return gridType.empty;
        }
        else{
            return grid[x,y];
        }
    }

    public void setEndingGrid(List<GameObject> _grid){
        endingGrid = _grid;

        if(_grid.Count > 0){

            float x = 0,y = 0;
            int count = 0;
            Vector2 _x = new Vector2(_grid[0].transform.localPosition.x, _grid[0].transform.localPosition.x);
            Vector2 _y = new Vector2(_grid[0].transform.localPosition.y, _grid[0].transform.localPosition.y);

            foreach (GameObject item in _grid)
            {
                x += Mathf.FloorToInt(item.transform.localPosition.x);
                y += Mathf.FloorToInt(item.transform.localPosition.y);

                _x.x = Mathf.Min(_x.x, item.transform.localPosition.x);
                _x.y = Mathf.Max(_x.y, item.transform.localPosition.x);

                _y.x = Mathf.Min(_y.x, item.transform.localPosition.y);
                _y.y = Mathf.Max(_y.y, item.transform.localPosition.y);

                count++;
            }

            Vector3 _EffectCoord = new Vector3(x / count, y / count, 0);

            GameObject _tmpEffect = Instantiate(endingEffect, transform);

            //_tmpEffect.transform.localScale = new Vector3(Mathf.Abs(_x.x - _x.y) + 1, Mathf.Abs(_y.x - _y.y) + 1, 10f);

            _tmpEffect.transform.localPosition = _EffectCoord;
        }
    }

    public void resetGrid(){
        for (int i = 0; i < cubeGrid.GetLength(0); i++)
        {
            for (int j = 0; j < cubeGrid.GetLength(1); j++)
            {
                if(grid[i,j] == gridType.floor){
                    _ObjectManager.retireObject(cubeGrid[i,j]);
                }
                else if(cubeGrid[i,j] != null){
                   Destroy(cubeGrid[i,j]);
                }
            }
        }

    }
}

public enum gridType
{
    empty,
    slot,
    enemy,
    floor,
    blackhole,
    pillarofdarkness,
    trapdoor,
    stompball,
    pyramid,
    endfloor
}
