﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator instance;
    public Vector2[] spots;
    public int spotOrder;

    public void generateLevel(GameObject[,] arry){
        
    }

    private void Start() {
        spots = new Vector2[3]{new Vector2(5,7), new Vector2(7,11), new Vector2(8,14)};
    }

    private void Awake() {
        if(instance == null){
            instance = this;
        }
    }
}
