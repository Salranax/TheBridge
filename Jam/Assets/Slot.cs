using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Vector2 coord;

    void Start()
    {
        
    }
    
    public void setCoord(Vector2 v){
        coord = v;
    }

    public Vector2 getCoord(){
        return coord;
    }
}
