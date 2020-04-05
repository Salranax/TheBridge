using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private MoveDirection projectileDirection;
    private Vector3 moveVector;
    private int moveDistance = 3;

    public void SetProjectile(Vector3 _startpos, MoveDirection _direction){
        _startpos += new Vector3(0,0, -0.40f);
        transform.SetParent(GridSystem.instance.transform);
        projectileDirection = _direction;
        moveVector = positionCalculation(_direction);

        if(_direction == MoveDirection.Forward || _direction == MoveDirection.Back){
            transform.eulerAngles = new Vector3(90,0,0);
        }
        else if(_direction == MoveDirection.Left || _direction == MoveDirection.Right){
            transform.eulerAngles = new Vector3(0,90,0);
        }

        transform.localPosition = _startpos + moveVector;
    }

    void Start()
    {
        TickManager.instance.tick.AddListener(moveProjectile);
    }

    void moveProjectile(){
        transform.localPosition += moveVector;

    }

    private Vector3 positionCalculation(MoveDirection _dir){
        Vector3 _moveVector = Vector3.zero;

        if(_dir == MoveDirection.Forward){
            _moveVector = new Vector3(0,1,0);
        }
        else if(_dir == MoveDirection.Back){
            _moveVector = new Vector3(0,-1,0);
        }
        else if(_dir == MoveDirection.Left){
            _moveVector = new Vector3(-1,0,0);
        }
        else if(_dir == MoveDirection.Right){
            _moveVector = new Vector3(1,0,0);
        }

        return _moveVector;
    }

    
}
