using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Material mat;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setColor(Color c){
        if (mat == null)
        {
            mat = GetComponent<Renderer>().material;
            mat.color = c;
        }
        else
        {
            mat.color = c;
        }
    }
}
