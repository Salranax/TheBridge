using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public GameManager _GameManager;
    public CameraManager _CameraManager;
    private PlayerLightController _PlayerLightController;
    
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
    private bool isHolding = false;
    private int startX = 5, startY = 3;
    private float tickInterval;
    public Color startEmissionColor;
    public GameObject slotEffect;

    //Player Values
    private float timePower = 10f;
    private float timePowerDecreaseSpeed = 2f;

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

        //startEmissionColor = GetComponent<Renderer>().material.GetColor("_EmissionColor");

        _PlayerLightController = GetComponent<PlayerLightController>();
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
            if(timer > 2f){
                SceneManager.LoadSceneAsync(0);
            }
        }

        // if(isHolding){
        //     timePower -= Time.deltaTime * timePowerDecreaseSpeed;
        //     _GameManager._UIManager.lightSlider.value = timePower;
        //     if(timePower < 0){
        //         TickManager.instance.SetIsGameStarted(false);
        //         _GameManager._UIManager.fail();  
        //     }
        // }
    }

    public void setTouchHoldStatus(bool _isHolding){
        // isHolding = _isHolding;
        // _PlayerLightController.setHoldingStatus(isHolding);
    }

    public void movePlayer(){
        if(!resetTurn && !isTooMuch && !isFalling && !isHolding){
            if(isSwiped){
                //_CameraManager.setCameraDirection(nextMoveDirection);

                moveDirection = nextMoveDirection;
                movePlayerPhysically(moveDirection);

                isSwiped = false;
            }
            else{
                movePlayerPhysically(moveDirection);
            }
        }
        else if(resetTurn){
            resetTurn =! resetTurn;
        }
    }

    private void fallNow(){
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(new Vector3(0,0,500));

        _GameManager.playerCamTOP.Priority = 12;
        TickManager.instance.tick.RemoveListener(movePlayer);
    }

    private void movePlayerPhysically(MoveDirection _dir){
        GridSystem _GridSystem = _GameManager._GridSystem;
        
        if(moveDirection == MoveDirection.Forward){
            gridY ++;
            gridCalc(gridX, gridY, Vector3.right, new Vector3(gridX, gridY + 1, -0.7f), MoveDirection.Forward);
        }
        else if(moveDirection == MoveDirection.Back){
            gridY --;
            gridCalc(gridX, gridY, -Vector3.right, new Vector3(gridX, gridY - 1, -0.7f), MoveDirection.Back);
        }
        else if(moveDirection == MoveDirection.Left){
            gridX --;
            gridCalc(gridX, gridY, Vector3.up, new Vector3(gridX - 1, gridY, -0.7f), MoveDirection.Left);
        }
        else if(moveDirection == MoveDirection.Right){
            gridX ++;
            gridCalc(gridX, gridY, -Vector3.up, new Vector3(gridX + 1, gridY, -0.7f), MoveDirection.Right);
        }
    }

    private void gridCalc(int x, int y, Vector3 axis, Vector3 finalPos, MoveDirection _dir = MoveDirection.Forward){
        GridSystem _GridSystem = _GameManager._GridSystem;

        if(_GridSystem.getGridType(x, y) == gridType.floor){
            StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir));
        }
        else if(_GridSystem.getGridType(x, y) == gridType.empty){
            StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir));
            isFalling = true;
        }
        else if(_GridSystem.getGridType(x, y) == gridType.blackhole){
            StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir));
            _PlayerLightController.toggleLight(false);
        }
        else if(_GridSystem.getGridType(x, y) == gridType.slot){
            StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir, gridType.slot));
        }
        else if(_GridSystem.getGridType(x, y) == gridType.endfloor){
            if(_GameManager._ProgressManager.objectiveStatus()){
                StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir, gridType.endfloor));
            }
            else{
                StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir));
            }
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
        if(dir == SwipeDirection.Left){
            isSwiped = true;
            nextMoveDirection = MoveDirection.Left;
        }
        else if(dir == SwipeDirection.Right){
            isSwiped = true;
            nextMoveDirection = MoveDirection.Right;
        }
        else if(dir == SwipeDirection.Up){
            isSwiped = true;
            nextMoveDirection = MoveDirection.Forward;
        }
    }
    
    //For editor use
    public void swipeRight(){
        isSwiped = true;
        nextMoveDirection = MoveDirection.Right;
    }

    public void swipeLeft(){
        isSwiped = true;
        nextMoveDirection = MoveDirection.Left;
    }

    public void swipeUp(){
        isSwiped = true;
        nextMoveDirection = MoveDirection.Forward;
    }

    public void swipeDown(){
        isSwiped = true;
        nextMoveDirection = MoveDirection.Back;
    }
    //

    public void resetPlayer(){

    }

    private IEnumerator Rotate90(Vector3 axis, Vector3 finalPos, MoveDirection _dir = MoveDirection.Forward,gridType _type = gridType.floor) {
        startOrientation = transform.rotation;
        axis = transform.InverseTransformDirection(axis);
        float speed = 0;
        int _dirCoeff = 1;

        Vector3 _rotatePoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.5f);;
        Vector3 _rotateAxis = Vector3.right;;

        if(_dir == MoveDirection.Forward){
            _rotatePoint = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z + 0.5f);
            _rotateAxis = Vector3.right;
        }
        else if(_dir == MoveDirection.Back){
            _rotatePoint = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z + 0.5f);
            _rotateAxis = Vector3.right;
            _dirCoeff = -1;
        }
        else if(_dir == MoveDirection.Left){
            _rotatePoint = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z + 0.5f);
            _rotateAxis = Vector3.up;
        }
        else if(_dir == MoveDirection.Right){
            _rotatePoint = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z + 0.5f);
            _rotateAxis = Vector3.up;
            _dirCoeff = -1;
        }
        float totalRotation = 0;

        while(totalRotation < 90){
            speed += 180 / (tickInterval * tickInterval * 4 / 9) * Time.deltaTime;

            transform.RotateAround(_rotatePoint, _rotateAxis, speed * Time.deltaTime * _dirCoeff);
            totalRotation += speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = startOrientation * Quaternion.AngleAxis(90, axis);
        transform.localPosition = finalPos;

        if(_type == gridType.floor){
            _PlayerLightController.toggleLight(true);
        }
        else if(_type == gridType.slot){
            _PlayerLightController.toggleLight(true);
            resetTurn = true;
            //ADD SCORE
            float _time = 0;
            Vector3 _StartPos = transform.localPosition;
            Vector3 _FinalPos = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.31f);

            while (_time < tickInterval / 3)
            {
                transform.localPosition = Vector3.Lerp(_StartPos, _FinalPos, _time / tickInterval * 3);
                _time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = _FinalPos;

            GameObject _gridObj = _GameManager._GridSystem.getGridGameobject(gridX, gridY);
            Destroy(_gridObj);

            _gridObj = Instantiate(_GameManager._TextureLevelGenerator.colorMappings[0].prefab, _GameManager._GridSystem.transform);
            _gridObj.transform.localPosition = new Vector2(gridX, gridY);

            _GameManager._CameraManager.stopFollowing();
            float _zCoord = -0.7f;
            _StartPos = transform.localPosition = new Vector3(gridX, gridY, -10f);
            _FinalPos = new Vector3(gridX, gridY, _zCoord);

            _time = 0;
            while (_time < tickInterval * 2 / 3)
            {
                transform.localPosition = Vector3.Lerp(_StartPos, _FinalPos, _time * 3 / tickInterval / 2);
                _time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            slotEffect.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.1f);
            slotEffect.SetActive(true);

            transform.localPosition = _FinalPos;
            transform.rotation = Quaternion.identity;
            _GameManager._ProgressManager.increaseSlotScore();
            _GameManager._CameraManager.startFollowing(transform);
        }
        else if(_type == gridType.endfloor){
            _GameManager._TickManager.SetIsGameStarted(false);
            _GameManager.endGame();
        }
        
        if(isFalling){
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0,0,500));

            _GameManager.playerCamTOP.Priority = 12;
        }

        yield return new WaitForEndOfFrame();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && !isFalling && !resetTurn){
            StopAllCoroutines();
            StartCoroutine("enemyHitAnim");
        }
        else if(other.CompareTag("PillarOfDarkness") && !isFalling && !resetTurn){
            StopAllCoroutines();
            StartCoroutine(pillarCollisionEffect());
        }
        else if(other.CompareTag("Projectile") && !isFalling && !resetTurn){
            StopAllCoroutines();
            Debug.Log("LOSE");
        }
        else if(other.CompareTag("TrapDoor") && !isFalling && !resetTurn){
            isFalling = true;
            //fallNow();
            Debug.Log("Trap Door");
        }
    }

    IEnumerator enemyHitAnim(){
        float t = 0;
        Material tmpMat = GetComponent<Renderer>().material;
        
        // Color currentColor = tmpMat.GetColor("_EmissionColor");

        while(t < tickInterval / 2){
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetPlayer();

        resetTurn = true;

        // while(t < tickInterval){
        //     transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(1,1,1),t / tickInterval * 2);
        //     tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, startEmissionColor, t / tickInterval));
        //     tmpMat.color = Color.Lerp(Color.black, Color.white, t * 2);
        //     t += Time.deltaTime;
        //     yield return new WaitForEndOfFrame();
        // }

        // tmpMat.SetColor("_EmissionColor", startEmissionColor);
        // tmpMat.color = Color.white;

        resetTurn = false;

        yield return new WaitForEndOfFrame();
    }

    IEnumerator pillarCollisionEffect(){
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
}

public enum MoveDirection
{
    Left,
    Right,
    Forward,
    Back
}