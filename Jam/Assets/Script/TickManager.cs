using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TickManager : MonoBehaviour
{
    public static TickManager instance;

    private float tickInterval = 0.5f;
    public UnityEvent tick;
    public UnityEvent tickTimeChanged;
    
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

    public void SetTickInterval(float i){
        tickInterval = i;
        tickTimeChanged.Invoke();
    }

    public float GetTickInterval(){
        return tickInterval;
    }
}
