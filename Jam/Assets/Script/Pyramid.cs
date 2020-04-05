using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    public GameObject projectilePrefab;
    private int cooldown = 4;
    private int turnCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(shootCheck);
    }

    private void shootCheck(){
        if(turnCounter <= 0){
            turnCounter = cooldown - 1;
            shootProjectile();
        }
        else{
            turnCounter--;
        }
    }

    private void shootProjectile(){
        GameObject tmp = Instantiate(projectilePrefab);
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Forward);

        tmp = Instantiate(projectilePrefab);
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Back);

        tmp = Instantiate(projectilePrefab);
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Left);

        tmp = Instantiate(projectilePrefab);
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Right);
    }
}
