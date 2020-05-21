using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pyramid : MonoBehaviour
{
    public GameObject projectilePrefab;
    private int cooldown = 4;
    private int turnCounter = 0;
    private ObjectManager _ObjectManager;

    // Start is called before the first frame update
    void Start()
    {
        TickManager.instance.tick.AddListener(shootCheck);
    }

    public void setPyramid(ObjectManager _pooling){
        _ObjectManager = _pooling;
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
        GameObject tmp = _ObjectManager.getProjectile();
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Forward, _ObjectManager);

        tmp = _ObjectManager.getProjectile();
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Back, _ObjectManager);

        tmp = _ObjectManager.getProjectile();
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Left, _ObjectManager);

        tmp = _ObjectManager.getProjectile();
        tmp.GetComponent<Projectile>().SetProjectile(transform.localPosition, MoveDirection.Right, _ObjectManager);
    }
}
