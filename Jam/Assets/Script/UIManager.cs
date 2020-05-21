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

    [Header("Start Screen Objects")]
    public GameObject previousButton;
    public GameObject nextButton;
    
    [Header("Game UI Objects")]
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
        failScreen.SetActive(false);
        winScreen.SetActive(false);
    }

    public void toggleDecrease(bool _toogle){
        previousButton.SetActive(_toogle);
    }

    public void toggleIncrease(bool _toogle){
        nextButton.SetActive(_toogle);
    }   

    //BUTTON Functions
    public void reloadLevel(){

    } 
}
