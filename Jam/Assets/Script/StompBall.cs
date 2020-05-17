using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompBall : MonoBehaviour
{
    public GameObject ballGfx;
    public int stompCooldown;
    public Vector2 gridCoord;
    public GameObject[] stompColliders = new GameObject[4];
    private GridSystem _GridSystem;
    private int cooldownRound = 0;
    private Vector2[] stompCoord = new Vector2[4];
    private bool[] stompStatus = new bool[4]{true, true, true, true};
    private Vector2[] stompMoveDirections = new Vector2[4]{new Vector2(0,1), new Vector2(0,-1), new Vector2(1,0), new Vector2(-1,0)};

    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(checkStomp);
        TickManager.instance.gameStart.AddListener(stompSetup);

        _GridSystem = transform.parent.GetComponent<GridSystem>();

        for (int i = 0; i < 4; i++)
        {
            stompCoord[i] = gridCoord;
        }
    }

    void stompSetup(){

    }

    private Vector2 calculateNext(int x, int y, Vector2 nextDir){
        if(_GridSystem.getGridType(Mathf.FloorToInt(x + nextDir.x), Mathf.FloorToInt(y + nextDir.y)) == gridType.floor){
            return new Vector2(x + nextDir.x, y + nextDir.y);
        }
        else{
            return gridCoord;
        }
    }

    void checkStomp(){
        if(cooldownRound == 0){
            //STOMP EFFECT
            for (int i = 0; i < 4; i++)
            {
                stompColliders[i].transform.position = new Vector3(this.transform.position.x, this.transform.position.y, stompColliders[i].transform.position.z);

                stompStatus[i] = true;

                _GridSystem.getGridGameobject(Mathf.FloorToInt(stompCoord[i].x), Mathf.FloorToInt(stompCoord[i].y)).SetActive(true);
                stompCoord[i] = new Vector2(gridCoord.x, gridCoord.y);
            }
            GetComponent<Animator>().Play(0);
            cooldownRound = stompCooldown;
        }
        else if(cooldownRound > 0){
            for (int i = 0; i < 4; i++)
            {
                if(stompStatus[i] == true){
                    Vector2 _tmp = calculateNext(Mathf.FloorToInt(stompCoord[i].x),Mathf.FloorToInt(stompCoord[i].y), stompMoveDirections[i]);

                    if(_tmp == gridCoord){
                        GameObject _tmpObj = _GridSystem.getGridGameobject(Mathf.FloorToInt(stompCoord[i].x), Mathf.FloorToInt(stompCoord[i].y));
                        stompColliders[i].transform.position = new Vector3(this.transform.position.x, this.transform.position.y, stompColliders[i].transform.position.z);
                        _tmpObj.SetActive(true);
                        stompStatus[i] = false;
                    }
                    else{
                        Vector2 _currentCoord = stompCoord[i];
                        _GridSystem.getGridGameobject(Mathf.FloorToInt(_currentCoord.x), Mathf.FloorToInt(_currentCoord.y)).SetActive(true);
                    
                        stompCoord[i] = _tmp;
                        _GridSystem.getGridGameobject(Mathf.FloorToInt(stompCoord[i].x), Mathf.FloorToInt(stompCoord[i].y)).SetActive(false);

                        Vector3 _localPos = stompColliders[i].transform.position;
                        Vector3 _objectCoord = _GridSystem.getGridGameobject(Mathf.FloorToInt(_currentCoord.x), Mathf.FloorToInt(_currentCoord.y)).transform.position;
                        stompColliders[i].transform.position = new Vector3(_tmp.x, _tmp.y, _localPos.z);
                    }
                }
            }
            cooldownRound--;
        }

    }
}
