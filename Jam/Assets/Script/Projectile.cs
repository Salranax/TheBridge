using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   
    private ObjectManager _ObjectManager;
    private MoveDirection projectileDirection;
    private Vector3 moveVector;
    private int moveDistance = 3;
    private int moveAmount = 0;
    private float tickInterval;

    public void SetProjectile(Vector3 _startpos, MoveDirection _direction, ObjectManager _pooling){
        TickManager.instance.tick.AddListener(moveProjectile);

        _ObjectManager = _pooling;

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

        transform.localPosition = _startpos;
        StartCoroutine(projectileMove(_startpos + moveVector));  
    }

    void Start()
    {
        tickInterval = TickManager.instance.GetTickInterval();
    }

    void moveProjectile(){
        if(moveAmount == moveDistance){
            moveAmount = 0;
            StopAllCoroutines();
            TickManager.instance.tick.RemoveListener(moveProjectile);
            _ObjectManager.retireProjectile(this.gameObject);
        }
        else{
            moveAmount ++;
            StartCoroutine(projectileMove(transform.localPosition + moveVector));
        }
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

    IEnumerator projectileMove(Vector3 _targetPos){
        float _timer = 0;
        Vector3 _startpos = transform.localPosition;
        float _completeTime = tickInterval * 2 / 3;

        while (_timer < _completeTime)
        {   
            _timer += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(_startpos, _targetPos, _timer / _completeTime);
            yield return new WaitForEndOfFrame();
        }

        transform.localPosition = _targetPos;
    }

    
}
