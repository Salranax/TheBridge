using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureLevelGenerator : MonoBehaviour
{
    public Texture2D map;

    // Start is called before the first frame update
    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel(){
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateTile(x, y);
            }
            
        }
    }

    void GenerateTile(int x, int y){
        Color pixelColor = map.GetPixel(x,y);
    }
}
