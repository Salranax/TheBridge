using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public Light followLight;

    private bool isHolding = false;
    private float speed = 20f;
    private float defaultAngle;

    void Start() {
        defaultAngle = followLight.spotAngle;
    }

    void Update() {
        if(isHolding){
            followLight.spotAngle -= speed * Time.deltaTime;
        }
    }

    public void setHoldingStatus(bool _isHolding){
        if(_isHolding){
            isHolding = _isHolding;
        }
        else if(!_isHolding){
            isHolding = _isHolding;
            resetLight();
        }
    }

    private void resetLight(){
        followLight.spotAngle = defaultAngle;
    }

}
