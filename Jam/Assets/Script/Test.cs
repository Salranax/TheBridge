using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GridSystem _GridSystem;
    GameObject tmp;

    public void testObjectManager(){
        //tmp = ObjectManager.instance.getGameObject();
    }

    public void resetGrid(){
        _GridSystem.resetGrid();
    }
}
