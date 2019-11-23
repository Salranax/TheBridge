using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private bool isSwiped = false;
    SwipeDirection dir;

    private int startX = 6, startY = 2;
    public int gridX, gridY;
    Quaternion startOrientation;

    void Awake() {
        if(instance == null){
            instance = this;
        }    
    }
    // Start is called before the first frame update
    void Start()
    {
        gridX = 6;
        gridY = 2;
        TickManager.instance.tick.AddListener(movePlayer);
        transform.localPosition = new Vector3(gridX, gridY, -0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movePlayer(){
        if(isSwiped){
            if(dir == SwipeDirection.Left){
                gridType tmpType = GridSystem.instance.grid[gridX - 1, gridY];
                if(tmpType == gridType.floor){
                    gridX -= 1;
                    //transform.localPosition = new Vector3(gridX, gridY, -0.5f);
                    StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                }
                else if(tmpType == gridType.enemy){
                    resetPlayer();
                }
            }
            else if(dir == SwipeDirection.Right){
                gridType tmpType = GridSystem.instance.grid[gridX + 1, gridY];
                if(tmpType == gridType.floor){
                    gridX += 1;
                    //transform.localPosition = new Vector3(gridX, gridY, -0.5f);
                    StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                }
                else if(tmpType == gridType.enemy){
                    resetPlayer();
                }
            }
            isSwiped = false;
        }
        else{
            gridType tmpType = GridSystem.instance.grid[gridX + 1, gridY];
            if(tmpType == gridType.floor){
                gridY += 1;
                //transform.localPosition = new Vector3(gridX, gridY, -0.5f);
                StartCoroutine(Rotate90(Vector3.right, new Vector3(gridX, gridY, -0.5f)));
            }
            else if(tmpType == gridType.enemy){
                resetPlayer();
            }
        }
    }

    public void getSwipe(SwipeData dt){
        isSwiped = true;
        dir = dt.Direction;
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
        transform.localPosition = new Vector3(startX, startY, -0.5f);
        gridX = startX;
        gridY = startY;
    }
 
    private IEnumerator Rotate90(Vector3 axis, Vector3 finalPos) {
        startOrientation = transform.rotation;
        axis = transform.InverseTransformDirection(axis);
        float amount = 0;

        while (amount < 1) {
            yield return new WaitForEndOfFrame();
            amount += Time.deltaTime * 4;
            transform.rotation = startOrientation*Quaternion.AngleAxis(Mathf.Lerp(0,90,amount), axis);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, amount/ 3);
        }
        transform.rotation = startOrientation * Quaternion.AngleAxis(90, axis);
        transform.localPosition = finalPos;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy"){
            resetPlayer();
            StopAllCoroutines();
        }
    }
}
