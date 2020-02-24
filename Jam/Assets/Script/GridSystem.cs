using System.Collections;
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
