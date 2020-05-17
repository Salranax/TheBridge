using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Screens")]
    public GameObject winScreen;
    public GameObject failScreen;
    public GameObject gameUI;
    
    [Header("UI Objects")]
    public Text levelNumber;
    public Text goalText;

    public void win(){
        winScreen.SetActive(true);
    }

    public void fail(){
        failScreen.SetActive(true);
    }

    public void setLevel(int dozen, int figure){
        levelNumber.text = dozen.ToString() + " - " + figure.ToString();
    }

    public void SetScore(int _score, int _goal){
        goalText.text = _score.ToString() + "/" + _goal.ToString();
    }

    public void activateGameUI(){
        gameUI.SetActive(true);
    }
}
