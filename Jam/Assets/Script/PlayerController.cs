using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    private bool isSwiped = false;
    private bool resetTurn = false;
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
        if(!resetTurn){
            if(isSwiped){
                if(dir == SwipeDirection.Left){
                    gridType tmpType = GridSystem.instance.grid[gridX , gridY + 1];
                    if(tmpType == gridType.floor || tmpType == gridType.slot){
                        gridX -= 1;
                        StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
    
                }
                else if(dir == SwipeDirection.Right){
                    gridType tmpType = GridSystem.instance.grid[gridX , gridY + 1];
                    if(tmpType == gridType.floor || tmpType == gridType.slot){
                        gridX += 1;
                        StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }

                }
                isSwiped = false;
            }
            else{
                gridType tmpType = GridSystem.instance.grid[gridX , gridY + 1];
                if(tmpType == gridType.floor || tmpType == gridType.slot){
                    gridY += 1;
                    StartCoroutine(Rotate90(Vector3.right, new Vector3(gridX, gridY, -0.5f)));
                }

            }
        }
        else{
            resetPlayer();
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
        resetTurn = false;
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

        gridType tmpType = GridSystem.instance.grid[gridX , gridY];
        if(tmpType == gridType.slot){
            float time = 0;

            while(time < 0.1f){
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, 0.3f), time * 10);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, 0.3f), time * 10);
            GameObject tmp = Instantiate(GridSystem.instance.cubePrefab);
            tmp.transform.SetParent(GridSystem.instance.transform);
            tmp.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            GridSystem.instance.grid[gridX , gridY] = gridType.floor;

            resetTurn = true;
            //TODO: Complete bridge part
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy"){
            resetPlayer();
            StopAllCoroutines();
        }
    }
}
