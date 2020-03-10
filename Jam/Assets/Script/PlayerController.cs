using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameManager _GameManager;
    public CameraManager _CameraManager;
    
    public int gridX, gridY;
    private Vector2 currentGridCoord;

    Quaternion startOrientation;
    SwipeDirection dir;
    private MoveDirection moveDirection = MoveDirection.Forward;
    private MoveDirection nextMoveDirection = MoveDirection.Forward;

    private bool isSwiped = false;
    private bool resetTurn = false;
    private bool isTooMuch = false;
    private bool isFalling = false;
    private int startX = 5, startY = 3;
    private float tickInterval;
    public Color startEmissionColor;

    private float timer = 0;

    public void setPlayerPoint(Vector2 _pos){
        transform.localPosition = new Vector3(_pos.x, _pos.y, -0.7f);
        gridX = Mathf.FloorToInt(_pos.x);
        gridY = Mathf.FloorToInt(_pos.y);
    }

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

    void Update()
    {
        if(isFalling && transform.position.z > 10f){
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            TickManager.instance.SetIsGameStarted(false);
            isFalling = false;
        }
        else if(!isFalling && transform.position.z > 10f){
            timer += Time.deltaTime;
            UIManager.instance.fail();
            if(timer > 2f){
                SceneManager.LoadSceneAsync(0);
            }
        }
    }

    public void movePlayer(){
        if(!resetTurn && !isTooMuch && !isFalling){
            if(isSwiped){
                _CameraManager.setCameraDirection(nextMoveDirection);

                moveDirection = nextMoveDirection;
                movePlayerPhysically(moveDirection);

                isSwiped = false;
            }
            else{
                movePlayerPhysically(moveDirection);
            }
        }
        else if(resetTurn){
            //resetPlayer();
        }

    }

    private void movePlayerPhysically(MoveDirection _dir){
        GridSystem _GridSystem = _GameManager._GridSystem;
        
        //TODO: Array length check!

        if(moveDirection == MoveDirection.Forward){
            if(_GridSystem.getGridType(gridX, gridY + 1) == gridType.floor){
                gridY ++;
                StartCoroutine(Rotate90(Vector3.right, new Vector3(gridX, gridY, -0.5f)));
            }
            else if(_GridSystem.getGridType(gridX, gridY + 1) == gridType.empty){
                gridY ++;
                isFalling = true;
                StartCoroutine(Rotate90(Vector3.right, new Vector3(gridX, gridY, -0.5f)));
            }
        }
        else if(moveDirection == MoveDirection.Back){
            if(_GridSystem.getGridType(gridX, gridY - 1) == gridType.floor){
                gridY --;
                StartCoroutine(Rotate90(-Vector3.right, new Vector3(gridX, gridY, -0.5f)));
            }
            else if(_GridSystem.getGridType(gridX, gridY - 1) == gridType.empty){
                gridY --;
                isFalling = true;
                StartCoroutine(Rotate90(-Vector3.right, new Vector3(gridX, gridY, -0.5f)));
            }
        }
        else if(moveDirection == MoveDirection.Left){
            if(_GridSystem.getGridType(gridX - 1, gridY) == gridType.floor){
                gridX --;
                StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
            }
            else if(_GridSystem.getGridType(gridX - 1, gridY) == gridType.empty){
                gridX --;
                isFalling = true;
                StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
            }
        }
        else if(moveDirection == MoveDirection.Right){
            if(_GridSystem.getGridType(gridX + 1, gridY) == gridType.floor){
                gridX ++;
                StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
            }
            else if(_GridSystem.getGridType(gridX + 1, gridY) == gridType.empty){
                gridX ++;
                isFalling = true;
                StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
            }
        }
    }

    private void moveWithGridCoord(int _GridX, int _GridY){
        //TODO: move player physically ifs to here
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
            nextMoveDirection = calculateSwipeDirection(dt.Direction);
        }
    }

    public void swipeRight(){
        isSwiped = true;
        dir = SwipeDirection.Right;
        nextMoveDirection = calculateSwipeDirection(dir);
    }

    public void swipeLeft(){
        isSwiped = true;
        dir = SwipeDirection.Left;
        nextMoveDirection = calculateSwipeDirection(dir);
    }

    public void resetPlayer(){

    }

    public IEnumerator slotReset(){
        transform.localScale = new Vector3(0, 0, 0);

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.5f);

        float amount = 0;

        while (amount < tickInterval / 2) {
            amount += Time.deltaTime;
            transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), amount / tickInterval * 2);
            yield return new WaitForEndOfFrame();
        }

        // Vector3 currentPos = transform.localPosition;
        // transform.localPosition = new Vector3(currentPos.x, currentPos.y, -8);
        
        // Vector3 finalPos = new Vector3(currentPos.x, currentPos.y, -0.5f);

        // float amount = 0;

        // while (amount < tickInterval / 2) {
        //     amount += Time.deltaTime;
        //     transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, amount / tickInterval * 2 / 3);
        //     yield return new WaitForEndOfFrame();
        // }

        resetTurn = false;
        
        yield return new WaitForEndOfFrame();
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

        }
        else if(isFalling){
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0,0,500));

            _GameManager.playerCamTOP.Priority = 12;
        }

        yield return new WaitForEndOfFrame();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy" && !isFalling && !resetTurn){
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

        resetTurn = true;

        while(t < tickInterval){
            transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(1,1,1),t / tickInterval * 2);
            tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, startEmissionColor, t / tickInterval));
            tmpMat.color = Color.Lerp(Color.black, Color.white, t * 2);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tmpMat.SetColor("_EmissionColor", startEmissionColor);
        tmpMat.color = Color.white;

        resetTurn = false;

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

    //Compare swipe data and decide next moves direction
    private MoveDirection calculateSwipeDirection(SwipeDirection _dir){
        //Debug.Log(dt.Direction);
        if(_dir == SwipeDirection.Left){
            if(moveDirection == MoveDirection.Forward){
                return MoveDirection.Left;
            }
            else if(moveDirection == MoveDirection.Right){
                return MoveDirection.Forward;
            }
            else if(moveDirection == MoveDirection.Back){
                return MoveDirection.Right;
            }
            else if(moveDirection == MoveDirection.Left){
                return MoveDirection.Back;
            }
            else{
                return MoveDirection.Forward;
            }
        }
        else if(_dir == SwipeDirection.Right){
            if(moveDirection == MoveDirection.Forward){
                return MoveDirection.Right;
            }
            else if(moveDirection == MoveDirection.Right){
                return MoveDirection.Back;
            }
            else if(moveDirection == MoveDirection.Back){
                return MoveDirection.Left;
            }
            else if(moveDirection == MoveDirection.Left){
                return MoveDirection.Forward;
            }
            else{
                return MoveDirection.Forward;
            }
        }
        else{
            return MoveDirection.Forward;
        }
    }
}

public enum MoveDirection
{
    Left,
    Right,
    Forward,
    Back
}