using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Timer Mode Status")]
    public bool TimerMode;

    [Header("Timer Mode Status")]
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
    private Vector2Int spawnPoint;

    private bool isAlive = true;
    private bool isSwiped = false;
    private bool resetTurn = false;
    private bool isTooMuch = false;
    private bool isFalling = false;
    private bool steppedEmpty = false;
    private bool isHolding = false;
    private int startX = 5, startY = 3;
    private float tickInterval;
    private Color startEmissionColor;
    private Color materialColor;
    private Material playerMaterial;
    public GameObject slotEffect;

    //Player Values
    private float timePower = 10f;
    private float timePowerDecreaseSpeed = 2f;

    private float timer = 0;

    public void setPlayerPoint(Vector2Int _pos){
        transform.localPosition = new Vector3(_pos.x, _pos.y, -0.7f);
        gridX = _pos.x;
        gridY = _pos.y;

        spawnPoint = new Vector2Int(_pos.x, _pos.y);
    }

    void Awake() {
        if(instance == null){
            instance = this;
        }    
    }

    void Start()
    {   
        if(TimerMode){
            TickManager.instance.tick.AddListener(movePlayer);
            TickManager.instance.tickTimeChanged.AddListener(intervalChanged);

            tickInterval = TickManager.instance.GetTickInterval();
        }
        else if(!TimerMode){
            tickInterval = 0.4f;
        }

        playerMaterial = GetComponent<Renderer>().material;
        startEmissionColor = GetComponent<Renderer>().material.GetColor("Color_C63B14FB");
        materialColor = GetComponent<Renderer>().material.GetColor("Color_871271A7");

        _PlayerLightController = GetComponent<PlayerLightController>();
    }

    void Update()
    {   
        if(TickManager.instance.GetIsGameStarted()){
            if((steppedEmpty || isFalling) && transform.position.z > 10f){
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                TickManager.instance.SetIsGameStarted(false);
                isFalling = false;
                steppedEmpty = false;
                loseCondition(); 
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
        if(!resetTurn && !isTooMuch && !isFalling && !isHolding && !steppedEmpty){
            if(isSwiped){
                //_CameraManager.setCameraDirection(nextMoveDirection);

                moveDirection = nextMoveDirection;
                movePlayerPhysically(moveDirection);

                if(TimerMode){
                    isSwiped = false;
                }
            }
            else{
                movePlayerPhysically(moveDirection);
            }
        }
        else if(resetTurn){
            resetTurn =! resetTurn;
        }
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
            steppedEmpty = true;
        }
        else if(_GridSystem.getGridType(x, y) == gridType.blackhole){
            StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir, gridType.blackhole));
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
        if(!isSwiped || TimerMode){
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
            else if(dir == SwipeDirection.Down){
                isSwiped = true;
                nextMoveDirection = MoveDirection.Back;
            }

            if(!TimerMode && !isFalling && !steppedEmpty && isAlive){
                movePlayer();
            }
        }

    }
    
    //For editor use
    public void swipeRight(){
        if(!isSwiped || TimerMode){
            isSwiped = true;
            nextMoveDirection = MoveDirection.Right;

            if(!TimerMode && !isFalling && !steppedEmpty && isAlive){
                movePlayer();
            }
        }
    }

    public void swipeLeft(){
        if(!isSwiped || TimerMode){
            isSwiped = true;
            nextMoveDirection = MoveDirection.Left;

            if(!TimerMode && !isFalling && !steppedEmpty && isAlive){
                movePlayer();
            }
        }
    }

    public void swipeUp(){
        if(!isSwiped || TimerMode){
            isSwiped = true;
            nextMoveDirection = MoveDirection.Forward;

            if(!TimerMode && !isFalling && !steppedEmpty && isAlive){
                movePlayer();
            }
        }
    }

    public void swipeDown(){
        if(!isSwiped || TimerMode){
            isSwiped = true;
            nextMoveDirection = MoveDirection.Back;

            if(!TimerMode && !isFalling && !steppedEmpty && isAlive){
                movePlayer();
            }
        }
    }

    public void resetPlayer(){
        _GameManager.playerCamTOP.Priority = 7;

        transform.localPosition = new Vector3(spawnPoint.x, spawnPoint.y, -0.7f);
        gridX = spawnPoint.x;
        gridY = spawnPoint.y;

        moveDirection = MoveDirection.Forward;
        nextMoveDirection = MoveDirection.Forward;

        playerMaterial.SetColor("Color_C63B14FB", startEmissionColor);
        playerMaterial.SetColor("Color_871271A7", materialColor);

        transform.rotation = Quaternion.identity;

        isAlive = true;

        _PlayerLightController.resetLight();
        TickManager.instance.tick.AddListener(movePlayer);
    }

    private void loseCondition(){
        isAlive = false;
        TickManager.instance.tick.RemoveListener(movePlayer);
        StartCoroutine(loseAction());
    }

    private IEnumerator loseAction(){
        float _timer = 0;

        while(_timer < 1f){
            _timer += Time.deltaTime;
            playerMaterial.SetColor("Color_C63B14FB" ,Color.Lerp(startEmissionColor, Color.black, _timer));
            playerMaterial.SetColor("Color_871271A7", Color.Lerp(materialColor, Color.black, _timer));
            _PlayerLightController.changeLightIntensity(1 - _timer);
            yield return new WaitForEndOfFrame();
        }

        TickManager.instance.SetIsGameStarted(false);
        _GameManager._UIManager.fail();

        yield return new WaitForEndOfFrame();
    }

    private IEnumerator Rotate90(Vector3 axis, Vector3 finalPos, MoveDirection _dir = MoveDirection.Forward,gridType _type = gridType.floor) {
        startOrientation = transform.rotation;
        axis = transform.InverseTransformDirection(axis);
        float speed = 0;
        int _dirCoeff = 1;

        yield return new WaitForSeconds(0.1f);

        if(isFalling){
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0,0,500));

            _GameManager.playerCamTOP.Priority = 12;

            StopAllCoroutines();
        }

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
            totalRotation += speed * Time.deltaTime;
            if(totalRotation > 90){
                break;
            }
            transform.RotateAround(_rotatePoint, _rotateAxis, speed * Time.deltaTime * _dirCoeff);
            
            speed += 180 / (tickInterval * tickInterval * 4 / 9) * Time.deltaTime;

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

            _GameManager._GridSystem.clearSlot(gridX, gridY, _gridObj);

            transform.localPosition = _FinalPos;
            transform.rotation = Quaternion.identity;
            _GameManager._ProgressManager.increaseSlotScore();
            _GameManager._CameraManager.startFollowing(transform);
        }
        else if(_type == gridType.endfloor){
            TickManager.instance.tick.RemoveListener(movePlayer);
            _GameManager._TickManager.SetIsGameStarted(false);
            _GameManager.endGame();
        }
        
        if(isFalling || steppedEmpty){
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0,0,500));

            _GameManager.playerCamTOP.Priority = 12;
        }

        if(!TimerMode){
            isSwiped = false;
            resetTurn = false;
        }

        yield return new WaitForEndOfFrame();
    }

    void OnTriggerEnter(Collider other)
    {
        if(isAlive){
            if(other.CompareTag("Enemy") && !isFalling && !resetTurn){
                StopAllCoroutines();
                loseCondition();
                //StartCoroutine("enemyHitAnim");
            }
            else if(other.CompareTag("PillarOfDarkness") && !isFalling && !resetTurn){
                StopAllCoroutines();
                loseCondition();
                //StartCoroutine(pillarCollisionEffect());
            }
            else if(other.CompareTag("Projectile") && !isFalling && !resetTurn){
                StopAllCoroutines();
                loseCondition();
            }
            else if(other.CompareTag("TrapDoor") && !isFalling && !resetTurn){
                isFalling = true;
            }
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