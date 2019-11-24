﻿using System.Collections;
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
        if(dir > 0){
            if(GridSystem.instance.grid[gridX + 1, gridY] != GridSystem.gridType.floor){
                dir *= -1;
            }
        }
        else if(dir < 0){
            if(GridSystem.instance.grid[gridX - 1, gridY] != GridSystem.gridType.floor){
                dir *= -1;
            }
        }
        
        gridX = gridX + dir;
        StartCoroutine(Rotate90(dir == 1 ? -Vector3.up : Vector3.up , new Vector3(gridX, gridY, -0.5f)));
    }

    public void activateSpawnPoint(){
        TickManager.instance.tick.RemoveListener(moveEnemy);
        StartCoroutine("transformToPlayer");
    }

    private IEnumerator Rotate90(Vector3 axis, Vector3 finalPos) {
        startOrientation = transform.rotation;
        axis = transform.InverseTransformDirection(axis);
        float amount = 0;

        while (amount < 1) {
            amount += Time.deltaTime * 4;
            transform.rotation = startOrientation*Quaternion.AngleAxis(Mathf.Lerp(0,90,amount), axis);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPos, amount/ 3);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = startOrientation * Quaternion.AngleAxis(90, axis);
        transform.localPosition = finalPos;
    }

    private IEnumerator transformToPlayer(){
        float t = 0;
        Material tmpMat = GetComponent<Renderer>().material;
        
        Color currentColor = tmpMat.GetColor("_EmissionColor");

        while (t < 0.5f)
        {
            tmpMat.EnableKeyword("_EMISSION");
            tmpMat.SetColor("_EmissionColor", Color.Lerp(Color.black, PlayerController.instance.startEmissionColor, t * 2));
            tmpMat.color = Color.Lerp(Color.black, Color.white, t * 2);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        this.gameObject.SetActive(false);
        yield return new WaitForEndOfFrame();
    }
}
