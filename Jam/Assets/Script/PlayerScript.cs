using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(movePlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movePlayer(){
        
    }
}
