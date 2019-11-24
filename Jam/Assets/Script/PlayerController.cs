using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    
    public int gridX, gridY;


    Quaternion startOrientation;
    SwipeDirection dir;

    private bool isSwiped = false;
    private bool resetTurn = false;
    private bool isTooMuch = false;
    private int startX = 6, startY = 2;
    public Color startEmissionColor;

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
        startEmissionColor = GetComponent<Renderer>().material.GetColor("_EmissionColor");
    }

    public void movePlayer(){
        if(!resetTurn && !isTooMuch){
            if(isSwiped){
                if(dir == SwipeDirection.Left){
                    GridSystem.gridType tmpType = GridSystem.instance.grid[gridX , gridY + 1];
                    if(tmpType == GridSystem.gridType.floor || tmpType == GridSystem.gridType.slot){
                        gridX -= 1;
                        StartCoroutine(Rotate90(Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }
    
                }
                else if(dir == SwipeDirection.Right){
                    GridSystem.gridType tmpType = GridSystem.instance.grid[gridX , gridY + 1];
                    if(tmpType == GridSystem.gridType.floor || tmpType == GridSystem.gridType.slot){
                        gridX += 1;
                        StartCoroutine(Rotate90(-Vector3.up, new Vector3(gridX, gridY, -0.5f)));
                    }

                }
                isSwiped = false;
            }
            else{
                //Debug.Log(gridX + " " + gridY);
                GridSystem.gridType tmpType = GridSystem.instance.grid[gridX , gridY + 1];
                if(tmpType == GridSystem.gridType.floor || tmpType == GridSystem.gridType.slot){
                    gridY += 1;
                    if(LevelGenerator.instance.spots[LevelGenerator.instance.spotOrder].y < gridY - 1  && !LevelGenerator.instance.isObjectiveComplete){
                        isTooMuch = true;
                        StartCoroutine("passedSlot");
                    }
                    StartCoroutine(Rotate90(Vector3.right, new Vector3(gridX, gridY, -0.5f)));
                    if(LevelGenerator.instance.spotOrder == 0 || (LevelGenerator.instance.spotOrder < LevelGenerator.instance.spots.Length && gridY > LevelGenerator.instance.spots[LevelGenerator.instance.spotOrder - 1].y)){
                        StartCoroutine("dimLight");
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

        while(t < 0.2f){
            tmpMat.SetColor("_EmissionColor", Color.Lerp(currentColor, targetColor, t * 5));
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
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
        if(resetTurn){
            if(LevelGenerator.instance.spotOrder == 1){
                transform.localPosition = new Vector3(startX, startY, -0.5f);
                gridX = startX;
                gridY = startY;
            }
            else{
                Vector2 v = LevelGenerator.instance.getSpawnCoord();
                transform.localPosition = new Vector3(v.x, v.y, -0.5f);
                gridX = Mathf.FloorToInt(v.x);
                gridY = Mathf.FloorToInt(v.y);
                startX = gridX;
                startY = gridY;

                LevelGenerator.instance.setEnemyPlayer();
                LevelGenerator.instance.setNextSpawnPoint();
            }
            GetComponent<Renderer>().material.SetColor("_EmissionColor", startEmissionColor);
            
        }
        else{
            transform.localPosition = new Vector3(startX, startY, -0.5f);
            gridX = startX;
            gridY = startY;
        }
        
        transform.rotation = Quaternion.identity;
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

        GridSystem.gridType tmpType = GridSystem.instance.grid[gridX , gridY];
        if(tmpType == GridSystem.gridType.slot){
            float time = 0;

            while(time < 0.1f){
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, 0.3f), time * 10);
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0.3f);
            GameObject tmp = Instantiate(GridSystem.instance.cubePrefab);
            tmp.transform.SetParent(GridSystem.instance.transform);
            tmp.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            
            GridSystem.instance.grid[gridX , gridY] = GridSystem.gridType.floor;
            GridSystem.instance.cubeGrid[gridX, gridY] = tmp.GetComponent<Cube>();
            resetTurn = true;
            
            LevelGenerator.instance.increaseSpotOrder();
            GridSystem.instance.whiten(gridY);
        }
        if(LevelGenerator.instance.spotOrder == LevelGenerator.instance.spots.Length - 1 && LevelGenerator.instance.isObjectiveComplete){
            for (int i = 0; i < GridSystem.instance.cubeGrid.GetLength(0); i++)
            {
                Cube tmpCube = GridSystem.instance.cubeGrid[i,gridY];
                if(tmpCube != null){
                    tmpCube.setColor(Color.white);
                }
            }
            if(gridY >= 22){
                TickManager.instance.SetIsGameStarted(false);
                UIManager.instance.win();
            }
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

        while(t < 0.25f){
            transform.localScale = Vector3.Lerp(new Vector3(1,1,1), new Vector3(0,0,0),t * 4);
            tmpMat.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.black, t * 4));
            tmpMat.color = Color.Lerp(Color.white, Color.black, t * 4);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetPlayer();

        while(t < 0.5f){
            transform.localScale = Vector3.Lerp(new Vector3(0,0,0), new Vector3(1,1,1),t * 2);
            tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, startEmissionColor, t * 2));
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

        while(t < 0.5f){
            tmpMat.SetColor("_EmissionColor", Color.Lerp(currentColor, Color.black, t * 2));
            tmpMat.color = Color.Lerp(Color.white, Color.black, t * 4);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        resetPlayer();

        t = 0;

        while(t < 0.5f){
            tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, startEmissionColor, t * 2));
            tmpMat.color = Color.Lerp(Color.black, Color.white, t * 2);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        tmpMat.SetColor("_EmissionColor", startEmissionColor);
        tmpMat.color = Color.white;

        isTooMuch = false;

        yield return new WaitForEndOfFrame();
    }


}
