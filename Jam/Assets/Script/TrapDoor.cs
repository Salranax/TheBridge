using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public GameObject[] trapDoors;
    public Vector2 gridCoord;
    private GridSystem _GridSystem;
    private List<GameObject> trapDoorGfx = new List<GameObject>(); 
    private bool doorStatus = false;
    private int round = 0;

    void Start()
    {
        TickManager.instance.tick.AddListener(trapdoorToggle);
        TickManager.instance.gameStart.AddListener(setTrapdoorGfx);
        _GridSystem = transform.parent.GetComponent<GridSystem>();
    }

    public void setTrapDoor(GridSystem _Grid, Vector2 _coord){
        gridCoord = _coord;
        _GridSystem = _Grid;
    }

    void trapdoorToggle(){
        if(!doorStatus && round > 1){
            foreach (GameObject item in trapDoorGfx)
            {
                item.SetActive(false);    
            }
            foreach (GameObject item in trapDoors)
            {
                item.SetActive(true);
            }
            round = 0;
            doorStatus = !doorStatus;
        }
        else if(!doorStatus){
            round ++;
        }
        else if(doorStatus){
            foreach (GameObject item in trapDoorGfx)
            {
                item.SetActive(true);    
            }
            foreach (GameObject item in trapDoors)
            {
                item.SetActive(false);
            }
            round ++;
            doorStatus = !doorStatus;
        }
        
    }

    public void setTrapdoorGfx(){
        Vector2[] gfxCoords = new Vector2[4]{new Vector2(0,+1), new Vector2(0,-1), new Vector2(+1,0), new Vector2(-1,0)};

        if(_GridSystem != null){
            foreach (Vector2 coord in gfxCoords)
            {
                if(_GridSystem.getGridType(Mathf.FloorToInt(gridCoord.x + coord.x), Mathf.FloorToInt(gridCoord.y + coord.y)) == gridType.floor){
                    GameObject _tmp = _GridSystem.getGridGameobject(Mathf.FloorToInt(gridCoord.x + coord.x), Mathf.FloorToInt(gridCoord.y + coord.y));
                    if(_tmp != null){
                        trapDoorGfx.Add(_tmp);
                    }
                }
            }
        }
    }
}
