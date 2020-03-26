using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject winScreen;
    public GameObject failScreen;
    public Text levelNumber;
    public Button nextLevel;
    public Text objectiveCount;
    public GameObject progressTable;
    public Slider lightSlider;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    public void win(){
        winScreen.SetActive(true);
    }

    public void fail(){
        failScreen.SetActive(true);
    }

    public void restart(){
        SceneManager.LoadScene(0);
    }

    public void setLevel(int dozen, int figure){
        levelNumber.text = dozen.ToString() + " - " + figure.ToString();
    }

    public void reloadScene(){
        SceneManager.LoadSceneAsync(0);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene(0);
        }
    }
}
