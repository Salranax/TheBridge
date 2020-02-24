using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public int gridX, gridY;
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

    private float timer = 0;

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
            if(isSwiped && dir != SwipeDirection.Down){
                if(dir == SwipeDirection.Left){
                    if(gridX == 0){
                        gridX--;
                        isFalling = true;
                        StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
                    else{
                        gridX--;
                        StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
                }
                else if(dir == SwipeDirection.Right){

                }

                isSwiped = false;
            }
            else{

            }
        }
        else if(resetTurn){
            //resetPlayer();
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
}