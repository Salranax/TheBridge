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
        // if(isHolding){
        //     followLight.spotAngle -= speed * Time.deltaTime;
        // }
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

    /// <summary>
    /// Set light intensity as float value, between 0 and 1
    /// </summary>
    public void changeLightIntensity(float _intensity){
        followLight.intensity = Mathf.Lerp(0, _defaultIntensity, _intensity);
    }

    public void resetLight(){
        followLight.intensity = _defaultIntensity;
        followLight.spotAngle = defaultAngle;
    }

}
