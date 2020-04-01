using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    public TriggerType triggerObjType;
    public float triggerDistance;
    public TriggerObjectData PillarofDarkness;

    public void checkDistance(Vector3 _playerPos){
        if(Vector3.Distance(_playerPos, this.transform.position) <= triggerDistance){
            triggerEvent();
        }else{
            return;
        }
    }

    private void triggerEvent(){

    }
}

public enum TriggerType{
    PillarofDarkness

}
