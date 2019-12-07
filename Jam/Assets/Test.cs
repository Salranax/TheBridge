using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    GameObject tmp;

    public void testObjectManager(){
        tmp = ObjectManager.instance.getGameObject();
    }

    public void retire(){
        ObjectManager.instance.retireObject(tmp);
        tmp = null;
    }

    public void generateGridModule(){
        if(GridSystem.instance.activeModules.Count == 0){
            GridSystem.instance.generateNewPart(true);
        }
        else{
            GridSystem.instance.generateNewPart();
        }
    }
}
