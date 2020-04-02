using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    public TriggerType triggerObjType;
    public TriggerObjectData PillarofDarkness;
    private float triggerDistance;
    private bool isSingleTrigger;
    private int duration;
    private int cooldown;
    private bool isTriggered;
    private PlayerController _playerController;

    void Start()
    {   
        if(triggerObjType == TriggerType.PillarofDarkness){
            triggerDistance = PillarofDarkness.triggerDistance;
            isSingleTrigger = PillarofDarkness.singleTrigger;
            duration = PillarofDarkness.duration;
            cooldown = PillarofDarkness.cooldown;
        }

        TickManager.instance.tick.AddListener(checkDistance);

        _playerController = PlayerController.instance;
    }

    public void checkDistance(){
        if(!isTriggered){
            Vector3 _playerPos = _playerController.gameObject.transform.position;
            if(Vector3.Distance(_playerPos, this.transform.position) <= triggerDistance){
                triggerEvent();
            }else{
                return;
            }
        }
    }

    private void triggerEvent(){
        isTriggered = true;
        Debug.Log(transform.localPosition.x + " " + transform.localPosition.y);
    }
}

public enum TriggerType{
    PillarofDarkness

}
