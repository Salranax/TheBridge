using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    private Vector2 coord;
    private GridModule moduleOn;

    void Start()
    {
       
    }
    
    public void setCoord(Vector2 v, GridModule on){
        coord = v;
        moduleOn = on;
    }

    public Vector2 getCoord(){
        return coord;
    }
}
