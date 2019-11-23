using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolScript : MonoBehaviour
{
    public int gridX,gridY;
    private int dir = 1;
    Quaternion startOrientation;

    public void enemy(int gx, int gy){
        gridX = gx;
        gridY = gy;
    }

    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(moveEnemy);
    }

    public void moveEnemy(){
        if(GridSystem.instance.grid[gridX + 1, gridY] == gridType.floor){

        }
        else{
            dir *= -1;
        }
        
        
        //transform.localPosition = new Vector3(gridX + dir, gridY, -0.5f);
        gridX = gridX + dir;
        StartCoroutine(Rotate90(dir == 1 ? -Vector3.up : Vector3.up , new Vector3(gridX, gridY, -0.5f)));
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
    }
}
