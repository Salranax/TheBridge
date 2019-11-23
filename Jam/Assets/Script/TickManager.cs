using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickManager : MonoBehaviour
{
    public static TickManager instance;

    public float tickInterval = 1f;
    public UnityEvent tick;


    private float tickTime = 0;
    private bool isGameStarted = false;

//GetterSetters
    public bool GetIsGameStarted(){
        return isGameStarted;
    }

    public void SetIsGameStarted(bool tmp){
        isGameStarted = tmp;
    }
//

    public void startGame(){
        SetIsGameStarted(true);
    }
    
    void Awake()
    {
        if(instance == null){
            instance = this;
        }    
    }

    // Update is called once per frame
    void Update()
    {
        if(isGameStarted){
            tickTime += Time.deltaTime;

            if(tickTime >= tickInterval){
                tick.Invoke();
                tickTime = 0f;
            }
        }
    }
}
