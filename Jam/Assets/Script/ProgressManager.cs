using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{   
    public GameManager _GameManager;
    public UIManager _UIManager;
    private objective levelObjective;

    private int enteredSlotCount = 0;
    private int slotGoal = 0;
    private GameObject endPortalGFX;
    private bool isObjectiveCompleted = false;

    public void increaseSlotScore(){
        enteredSlotCount ++;
        _UIManager.SetScore(enteredSlotCount, slotGoal);
        checkProgress();
    }

    public void setLevel(objective o){
        levelObjective = o;
        //UIManager.instance.objectiveCount.text = o.slotCollect.ToString();
    }

    public void increaseGoal(){
        slotGoal ++;
        _UIManager.SetScore(enteredSlotCount, slotGoal);
    }

    public void setEndGFX(GameObject _portal){
        endPortalGFX = _portal;
        endPortalGFX.SetActive(false);
    }

    public bool objectiveStatus(){
        return isObjectiveCompleted;
    }

    private void checkProgress(){
        if(slotGoal == enteredSlotCount){
            endPortalGFX.SetActive(true);
            isObjectiveCompleted = true;
        }
    }
    
}
