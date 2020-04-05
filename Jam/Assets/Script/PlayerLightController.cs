using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightController : MonoBehaviour
{
    public Light followLight;

    private bool isHolding = false;
    private float speed = 20f;
    private float defaultAngle;
    private float _defaultIntensity;

    void Start() {
        defaultAngle = followLight.spotAngle;
        _defaultIntensity = followLight.intensity;
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

    public void toggleLight(bool _IO){
        if(_IO){
            followLight.intensity = _defaultIntensity;
        }
        else if(!_IO){
            followLight.intensity = 0;
        }
    }

    private void resetLight(){
        followLight.spotAngle = defaultAngle;
    }

}
