using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Controller Scripts")]
    public PlayerController _PlayerController;
    public GridSystem _GridSystem;
    [Header("----------------------")]
    public CinemachineVirtualCamera playerCam;
    private string LevelPrefName = "LevelProgress";
    private string UnlockPrefNsame = "LastUnlocked";

    void Awake() {
        if(instance == null){
            instance = this;
        }    
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey(LevelPrefName)){
            setLevel(1,1);
            PlayerPrefs.SetString(UnlockPrefNsame, formatSetter(1,1));
        }

        int[] lvl = formatGetter(getLevel());
        UIManager.instance.setLevel(lvl[0], lvl[1]);

    }

    public void startLevel(){
        playerCam.Priority = 11;
        int[] lvl = formatGetter(getLevel());
        setLevelData(lvl[0], lvl[1]);
    }

    public void increaseLevel(){
        int[] lvlTmp = new int[2];
        lvlTmp = formatGetter(getLevel());
        
        if(lvlTmp[1] == 10){
            lvlTmp[0] ++;
            lvlTmp[1] = 1;
        }
        else{
            lvlTmp[1] ++;
        }

        UIManager.instance.setLevel(lvlTmp[0], lvlTmp[1]);

        setLevel(lvlTmp[0], lvlTmp[1]);
    }

    public void decreaseLevel(){
        int[] lvlTmp = new int[2];
        lvlTmp = formatGetter(getLevel());

        if(lvlTmp[1] == 1 && lvlTmp[0] > 1){
            lvlTmp[1] = 10;
            lvlTmp[0] --;
        }
        else if(lvlTmp[0] >= 1 && lvlTmp[1] > 1){
            lvlTmp[1] --;
        }

        UIManager.instance.setLevel(lvlTmp[0], lvlTmp[1]);

        setLevel(lvlTmp[0], lvlTmp[1]);
    }

    private string formatSetter(int dozen, int figure){
        return dozen.ToString() + "." + figure.ToString();
    }

    private int[] formatGetter(string format){
        int[] level = new int[2];
        string[] formatSplit = new string[2];

        formatSplit = format.Split('.');

        level[0] = int.Parse(formatSplit[0]);
        level[1] = int.Parse(formatSplit[1]);

        return level;
    }

    private void setLevel(int dozen, int figure){
        PlayerPrefs.SetString(LevelPrefName, formatSetter(dozen,figure));
    }

    private string getLevel(){
        return PlayerPrefs.GetString(LevelPrefName);
    }

    private void setLevelData(int dozen, int figure){
        ProgressManager.instance.setLevel(Objectives.levelData(dozen, figure));
    }

}
