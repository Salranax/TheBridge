using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIActions : MonoBehaviour{
    [Header("Dependency")]
    public GameManager _GameManager;
    public UIManager _UIManager;

    public void reloadLevel(){
        _UIManager.closePopups();
        _GameManager.reloadLevel();
    }

    public void continuePlay(){
        _UIManager.closePopups();
        _GameManager._PlayerController.resetPlayer();
    }
}
