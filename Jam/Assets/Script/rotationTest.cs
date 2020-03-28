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
    private Vector3 _rotationAnchor;
    private bool _isRotating;
    
    void Start ()
    {
        _sensitivity = 0.4f;
    }
    
    void Update()
    {
        if(_isRotating)
        {
            transform.RotateAround(_rotationAnchor, Vector3.back, Time.deltaTime * 90);
            //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y - 2, transform.position.z + 2), Vector3.right);
        }
    }
    
    void OnMouseDown()
    {
        _rotationAnchor = new Vector3(transform.position.x + 2, transform.position.y - 2, transform.position.z);
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
