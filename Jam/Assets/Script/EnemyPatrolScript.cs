using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolScript : MonoBehaviour
{
    public int gridX,gridY;
    private int dir = 1;
    private bool transforming = false;
    private float tickInterval;
    Quaternion startOrientation;

    public void enemy(int gx, int gy){
        gridX = gx;
        gridY = gy;
    }

    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(moveEnemy);
        TickManager.instance.tickTimeChanged.AddListener(intervalChange);

        tickInterval = TickManager.instance.GetTickInterval();
    }

    public void moveEnemy(){
        if(!transforming){
            if(dir > 0){
                if(GridSystem.instance.grid[gridX + 1, gridY] != gridType.floor){
                    dir *= -1;
                }
            }
            else if(dir < 0){
                if(GridSystem.instance.grid[gridX - 1, gridY] != gridType.floor){
                    dir *= -1;
                }
            }
            
            gridX = gridX + dir;
            StartCoroutine(Rotate90(dir == 1 ? -Vector3.up : Vector3.up , new Vector3(gridX, gridY, -0.5f)));
        }
    }

    public void activateSpawnPoint(){
        transforming = true;
        TickManager.instance.tick.RemoveListener(moveEnemy);
        StartCoroutine("transformToPlayer");
    }

    private IEnumerator Rotate90(Vector3 axis, Vector3 finalPos) {
        startOrientation = transform.rotation;
        axis = transform.InverseTransformDirection(axis);
        float amount = 0;

        while (amount < tickInterval / 2) {
            amount += Time.deltaTime;
            transform.rotation = startOrientation*Quaternion.AngleAxis(Mathf.Lerp(0,90,amount / tickInterval * 2), axis);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, amount / tickInterval * 2 / 3);
            if(transforming){
                break;
            }
            yield return new WaitForEndOfFrame();
        }
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
            tmpMat.EnableKeyword("_EMISSION");
            tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, PlayerController.instance.startEmissionColor, t / tickInterval * 2));
            tmpMat.color = Color.Lerp(Color.black, Color.white, t / tickInterval * 2);
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
