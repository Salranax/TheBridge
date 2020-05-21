using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActions : MonoBehaviour{
    [Header("Dependency")]
    public GameManager _GameManager;

    public void reloadLevel(){
        _GameManager._UIManager.failScreen.SetActive(false);
        _GameManager.reloadLevel();
    }
}
