using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolScript : MonoBehaviour
{
    private enum DirAxis
    {
        x,
        y
    }

    public int gridX,gridY;
    private GridSystem _GridSystem;
    private int dir = 1;
    private DirAxis axis = DirAxis.y;
    private bool transforming = false;
    private float tickInterval;
    Quaternion startOrientation;

    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(moveEnemy);
        TickManager.instance.tickTimeChanged.AddListener(intervalChange);

        tickInterval = TickManager.instance.GetTickInterval();
    }

    public void setEnemy(GridSystem _system, int x, int y){
        _GridSystem = _system;
        gridY = y;
        gridX = x;
    }

    public void moveEnemy(){
        if(!transforming){
            int redirectChance = Random.Range(0,100);

            if(redirectChance >= 0 && redirectChance < 20){
                axis = changeAxis(axis);
                dir = 1;
            }

            if(axis == DirAxis.x){
                if(dir == 1){
                    if(_GridSystem.getGridType(gridX + 1, gridY) == gridType.floor){
                        moveEnemyPhysically(MoveDirection.Right);
                    }
                    else if(_GridSystem.getGridType(gridX - 1, gridY) == gridType.floor){
                        dir *= -1;
                        moveEnemyPhysically(MoveDirection.Left);
                    }
                    else if(_GridSystem.getGridType(gridX, gridY + 1) == gridType.floor){
                        axis = changeAxis(axis);
                        dir = 1;
                        moveEnemyPhysically(MoveDirection.Forward);
                    }
                    else{
                        //moveEnemyPhysically(MoveDirection.Back);
                    }
                }
                else if(dir == -1){
                    if(_GridSystem.getGridType(gridX - 1, gridY) == gridType.floor){
                        moveEnemyPhysically(MoveDirection.Left);
                    }
                    else if(_GridSystem.getGridType(gridX + 1, gridY) == gridType.floor){
                        dir *= -1;
                        moveEnemyPhysically(MoveDirection.Right);
                    }
                    else if(_GridSystem.getGridType(gridX, gridY + 1) == gridType.floor){
                        axis = changeAxis(axis);
                        dir = 1;
                        moveEnemyPhysically(MoveDirection.Forward);
                    }
                    else{
                        //moveEnemyPhysically(MoveDirection.Back);
                    }
                }
            }
            else if(axis == DirAxis.y){
                if(dir == 1){
                    if(_GridSystem.getGridType(gridX, gridY + 1) == gridType.floor){
                        moveEnemyPhysically(MoveDirection.Forward);
                    }
                    else if(_GridSystem.getGridType(gridX, gridY - 1) == gridType.floor){
                        dir *= -1;
                        moveEnemyPhysically(MoveDirection.Back);
                    }
                    else if(_GridSystem.getGridType(gridX + 1, gridY) == gridType.floor){
                        axis = changeAxis(axis);
                        dir = 1;
                        moveEnemyPhysically(MoveDirection.Right);
                    }
                    else{
                        //moveEnemyPhysically(MoveDirection.Left);
                    }
                }
                else if(dir == -1){
                    if(_GridSystem.getGridType(gridX, gridY - 1) == gridType.floor){
                        moveEnemyPhysically(MoveDirection.Back);
                    }
                    else if(_GridSystem.getGridType(gridX, gridY + 1) == gridType.floor){
                        dir *= -1;
                        moveEnemyPhysically(MoveDirection.Forward);
                    }
                    else if(_GridSystem.getGridType(gridX + 1, gridY) == gridType.floor){
                        axis = changeAxis(axis);
                        dir = 1;
                        moveEnemyPhysically(MoveDirection.Right);
                    }
                    else{
                        // axis = changeAxis(axis);
                        // dir = -1;
                        // moveEnemyPhysically(MoveDirection.Left);
                    }
                }
            }
        }
    }

    private DirAxis changeAxis(DirAxis _axis){
        if(_axis == DirAxis.x){
            return DirAxis.y;
        }
        else{
            return DirAxis.x;
        }
    }

    private void gridCalc(int x, int y, Vector3 axis, Vector3 finalPos, MoveDirection _dir = MoveDirection.Forward){
        StartCoroutine(Rotate90(axis, new Vector3(x, y, -0.7f), _dir));
    }

    private void moveEnemyPhysically(MoveDirection _dir){
        
        if(_dir == MoveDirection.Forward){
            gridY ++;
            gridCalc(gridX, gridY, Vector3.right, new Vector3(gridX, gridY + 1, -0.7f), MoveDirection.Forward);
        }
        else if(_dir == MoveDirection.Back){
            gridY --;
            gridCalc(gridX, gridY, -Vector3.right, new Vector3(gridX, gridY - 1, -0.7f), MoveDirection.Back);
        }
        else if(_dir == MoveDirection.Left){
            gridX --;
            gridCalc(gridX, gridY, Vector3.up, new Vector3(gridX - 1, gridY, -0.7f), MoveDirection.Left);
        }
        else if(_dir == MoveDirection.Right){
            gridX ++;
            gridCalc(gridX, gridY, -Vector3.up, new Vector3(gridX + 1, gridY, -0.7f), MoveDirection.Right);
        }
    }

    public void activateSpawnPoint(){
        transforming = true;
        TickManager.instance.tick.RemoveListener(moveEnemy);
        StartCoroutine("transformToPlayer");
    }

    private IEnumerator Rotate90(Vector3 axis, Vector3 finalPos, MoveDirection _dir = MoveDirection.Forward) {
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

        if(!transforming){
            transform.rotation = startOrientation * Quaternion.AngleAxis(90, axis);
            transform.localPosition = finalPos;
        }
    }

    private IEnumerator transformToPlayer(){
        float t = 0;
        Material tmpMat = GetComponent<Renderer>().material;
        
        Color currentColor = tmpMat.GetColor("_EmissionColor");

        while (t < tickInterval)
        {
            // tmpMat.EnableKeyword("_EMISSION");
            // tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, PlayerController.instance.startEmissionColor, t / tickInterval * 2));
            // tmpMat.color = Color.Lerp(Color.black, Color.white, t / tickInterval * 2);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
    }

    private void intervalChange(){
        tickInterval = TickManager.instance.GetTickInterval();
    }
}
