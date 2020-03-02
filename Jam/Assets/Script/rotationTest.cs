using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotationTest : MonoBehaviour
{

//      Vector3 startDragDir;
//  Vector3 currentDragDir;
//  Quaternion initialRotation;
//  float angleFromStart;
//  void OnMouseDown()
//  {
//      Debug.Log("ddd");
//      startDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
//      initialRotation = transform.rotation;
//  }
//  void OnMouseDrag()
//  {
//      currentDragDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
//      //gives you the angle in degrees the mouse has rotated around the object since starting to drag
//      angleFromStart = Vector3.Angle(startDragDir, currentDragDir);
//      transform.rotation = initialRotation;
//      transform.Rotate(0.0f, angleFromStart, 0.0f);
//  }
    private float _sensitivity;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation;
    private bool _isRotating;
    
    void Start ()
    {
        _sensitivity = 0.4f;
        _rotation = Vector3.zero;
    }
    
    void Update()
    {
        if(_isRotating)
        {
            // offset
            _mouseOffset = (Input.mousePosition - _mouseReference);
            
            // apply rotation
            // if(_mouseOffset.x > 0 && _mouseOffset.y > 0){
            //     _rotation.y = (Mathf.Abs(_mouseOffset.x) + Mathf.Abs(_mouseOffset.y)) * _sensitivity;
            // }
            // else if(_mouseOffset.x > 0 && _mouseOffset.y > 0){

            // }
            // else if(){

            // }
            
            
            _rotation.y = (Mathf.Abs(_mouseOffset.x) + Mathf.Abs(_mouseOffset.y)) * _sensitivity;
            
            // rotate
            transform.Rotate(_rotation);
            
            // store mouse
            _mouseReference = Input.mousePosition;
        }
    }
    
    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;
        
        // store mouse
        _mouseReference = Input.mousePosition;
    }
    
    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
    }
}
