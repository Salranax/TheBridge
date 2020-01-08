﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public int gridX, gridY;
    private GridModule currentModule;
    private Vector2 currentGridCoord;


    Quaternion startOrientation;
    SwipeDirection dir;

    private bool isSwiped = false;
    private bool resetTurn = false;
    private bool isTooMuch = false;
    private bool isFalling = false;
    private int startX = 5, startY = 3;
    private float tickInterval;
    public Color startEmissionColor;

    void Awake() {
        if(instance == null){
            instance = this;
        }    
    }
    // Start is called before the first frame update
    void Start()
    {

        TickManager.instance.tick.AddListener(movePlayer);
        TickManager.instance.tickTimeChanged.AddListener(intervalChanged);

        tickInterval = TickManager.instance.GetTickInterval();

        //transform.localPosition = new Vector3(0, 4, -0.5f);
        startEmissionColor = GetComponent<Renderer>().material.GetColor("_EmissionColor");
    }

    public void setPlayer(GridModule module){
        this.transform.SetParent(module.gameObject.transform);
        gridY = 3;
        gridX = 5;
        transform.localPosition = new Vector3(5, 3, -0.5f);
    }

    public void movePlayer(){
        if(!resetTurn && !isTooMuch && !isFalling){
            if(isSwiped && dir != SwipeDirection.Down){
                if(dir == SwipeDirection.Left){
                    if(gridX == 0){
                        Debug.Log("Fall");
                        gridX--;
                        isFalling = true;
                        StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
                    else{
                        gridX--;
                        StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
                }
                else if(dir == SwipeDirection.Right){
                    if(currentModule.gridSizeX - 1 == gridX){
                        Debug.Log("Fall");
                        gridX++;
                        isFalling = true;
                        StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
                    else{
                        gridX++;
                        StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
                }

                isSwiped = false;
            }
            else{
                if(currentModule.getGridSizeY() - 1 == gridY){
                    //Move to next Grid Module
                    GridModule tmpOld = currentModule;
                    currentModule = GridSystem.instance.getNextModule();
                    GridSystem.instance.moveToNextModule();

                    this.transform.SetParent(currentModule.gameObject.transform);
                    
                    if(GridSystem.instance.canPassToNext(tmpOld, currentModule, new Vector2(gridX, gridY))){
                        Vector2 newGridCoord = GridSystem.instance.positionOnNextGrid(tmpOld, currentModule, new Vector2(gridX, gridY));
                        gridX = Mathf.FloorToInt(newGridCoord.x);
                        gridY = Mathf.FloorToInt(newGridCoord.y);
                        StartCoroutine(Rotate90(Vector3.right, new Vector3(gridX, gridY, -0.5f)));
                    }
                    else{
                        Debug.Log("Fall");
                        gridY++;
                        isFalling = true;
                        StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
                    
                }
                else{
                    gridType tmpType = currentModule.gridArrangement[gridY + 1, gridX];
                    if(tmpType == gridType.floor || tmpType == gridType.slot){
                        gridY += 1;
                        // if(LevelGenerator.instance.slots[LevelGenerator.instance.spotOrder].getCoord().y < gridY - 1  && !LevelGenerator.instance.isObjectiveComplete){
                        //     isTooMuch = true;
                        //     StartCoroutine("passedSlot");
                        // }
                        StartCoroutine(Rotate90(Vector3.right, new Vector3(gridX, gridY, -0.5f)));
                        // if((LevelGenerator.instance.spotOrder == 0 || (LevelGenerator.instance.spotOrder < LevelGenerator.instance.slots.Length && gridY > LevelGenerator.instance.slots[LevelGenerator.instance.spotOrder - 1].getCoord().y)) && !LevelGenerator.instance.isObjectiveComplete){
                        //     StartCoroutine("dimLight");
                        // }
                    }
                }
            }
        }
        else{
            resetPlayer();
        }

    }

    public IEnumerator dimLight(){
        Material tmpMat = GetComponent<Renderer>().material;
        float t = 0;
        
        Color currentColor = tmpMat.GetColor("_EmissionColor");
        Color targetColor = new Color(currentColor.r * 0.7f, currentColor.g * 0.7f, currentColor.b * 0.7f, currentColor.a);

        while(t < tickInterval / 2){
            tmpMat.SetColor("_EmissionColor", Color.Lerp(currentColor, targetColor, t / tickInterval * 2 * 5));
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }        
    }

    public void getSwipe(SwipeData dt){
        dir = dt.Direction;
        if(dir == SwipeDirection.Left || dir == SwipeDirection.Right){
            isSwiped = true;
        }
    }

    public void swipeRight(){
        isSwiped = true;
        dir = SwipeDirection.Right;
    }

    public void swipeLeft(){
        isSwiped = true;
        dir = SwipeDirection.Left;
    }

    public void resetPlayer(){
        

        GetComponent<Renderer>().material.SetColor("_EmissionColor", startEmissionColor);
        currentModule = LevelGenerator.instance.getActiveSpawnPoint().spawnModule;
        currentGridCoord = LevelGenerator.instance.getActiveSpawnPoint().spawnCoord;

        transform.SetParent(currentModule.gameObject.transform);
        transform.localPosition = currentGridCoord;

        gridX = (int)currentGridCoord.x;
        gridY = (int)currentGridCoord.y;

        GridSystem.instance.setActivemodule(currentModule);
        transform.rotation = Quaternion.identity;
        resetTurn = false;
    }

    private IEnumerator Rotate90(Vector3 axis, Vector3 finalPos) {
        startOrientation = transform.rotation;
        axis = transform.InverseTransformDirection(axis);
        float amount = 0;

        while (amount < tickInterval / 2) {
            amount += Time.deltaTime;
            transform.rotation = startOrientation*Quaternion.AngleAxis(Mathf.Lerp(0,90,amount / tickInterval * 2), axis);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, amount / tickInterval * 2 / 3);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = startOrientation * Quaternion.AngleAxis(90, axis);
        transform.localPosition = finalPos;

        if(!isFalling){
            gridType tmpType = currentModule.gridArrangement[gridY, gridX];
            if(tmpType == gridType.slot){
                float time = 0;

                GameObject tmp = Instantiate(GridSystem.instance.spotEffect) as GameObject;
                tmp.transform.position = new Vector3(transform.position.x, transform.position.y, 0.5f);

                while(time < tickInterval / 5){
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, 0.3f), time / tickInterval * 5);
                    time += Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }

                //resetPlayer();
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.3f);
                // GameObject tmp = Instantiate(GridSystem.instance.cubePrefab);
                // tmp.transform.SetParent(GridSystem.instance.transform);
                // tmp.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                
                currentModule.convertSlotToFloor(gridX, gridY);
                currentModule.gridArrangement[gridY, gridX] = gridType.floor;
                LevelGenerator.instance.setActiveSpawnPoint(currentModule, new Vector2(gridX,gridY));

                resetTurn = true;
                
                LevelGenerator.instance.increaseSpotOrder();
                //GridSystem.instance.whiten(gridY);
            }
        }
        else if(isFalling){
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy"){
            StopAllCoroutines();
            StartCoroutine("enemyHitAnim");
        }
    }

    IEnumerator enemyHitAnim(){
        float t = 0;
        Material tmpMat = GetComponent<Renderer>().material;
        
        Color currentColor = tmpMat.GetColor("_EmissionColor");

        while(t < tickInterval / 2){
            transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(0,0,0),t / tickInterval * 4);
            tmpMat.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.black, t / tickInterval * 2));
            tmpMat.color = Color.Lerp(Color.white, Color.black, t * 4);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetPlayer();

        while(t < tickInterval){
            transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(1,1,1),t / tickInterval * 2);
            tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, startEmissionColor, t / tickInterval));
            tmpMat.color = Color.Lerp(Color.black, Color.white, t * 2);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tmpMat.SetColor("_EmissionColor", startEmissionColor);
        tmpMat.color = Color.white;

        yield return new WaitForEndOfFrame();
    }

    IEnumerator passedSlot(){
        float t = 0;
        Material tmpMat = GetComponent<Renderer>().material;
        
        Color currentColor = tmpMat.GetColor("_EmissionColor");

        while(t < tickInterval){
            tmpMat.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.black, t / tickInterval * 2));
            tmpMat.color = Color.Lerp(Color.white, Color.black, t / tickInterval * 2);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetPlayer();

        t = 0;

        while(t < tickInterval){
            tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, startEmissionColor, t / tickInterval));
            tmpMat.color = Color.Lerp(Color.black, Color.white, t / tickInterval);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tmpMat.SetColor("_EmissionColor", startEmissionColor);
        tmpMat.color = Color.white;

        isTooMuch = false;

        yield return new WaitForEndOfFrame();
    }

    private void intervalChanged(){
        tickInterval = TickManager.instance.GetTickInterval();
    }

    public void setCurrentModule(GridModule gm){
        currentModule = gm;
    }

    public GridModule getCurrentModule(){
        return currentModule;
    }
}
