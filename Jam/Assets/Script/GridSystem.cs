﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GridSystem : MonoBehaviour
{
    public static GridSystem instance;

    public gridType[,] grid;
    public Cube[,] cubeGrid;

    //Generation System
    public GameObject cubePrefab;
    public GameObject spotEffect;

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

    }

    public void generateGrid(int sizeX, int sizeY){
        grid = new gridType[sizeX, sizeY];
        cubeGrid = new Cube[sizeX, sizeY];
    }

    public void addToGrid(gridType _type, int _x, int _y){
        grid[_x, _y] = _type;
    }

    public void addToCubegrid(Cube _cube, int _x, int _y){
        cubeGrid[_x, _y] = _cube;
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
}

public enum gridType
{
    empty,
    slot,
    enemy,
    floor,
    trap
}
