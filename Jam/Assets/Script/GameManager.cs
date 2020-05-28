using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{

    [Header("Controller Scripts")]
    [SerializeField]
    public PlayerController _PlayerController;
    public GridSystem _GridSystem;
    public TickManager _TickManager;
    public UIManager _UIManager;
    public TextureLevelGenerator _TextureLevelGenerator;
    public CameraManager _CameraManager;
    public ProgressManager _ProgressManager;
    public TutorialController _TutorialController;
    [Header("----------------------")]
    public CinemachineVirtualCamera startCam;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera playerCamTOP;
    private string LevelPrefName = "LevelProgress";
    private string UnlockPrefNsame = "LastUnlocked";
    private string LastLevel = "1.9";
    int[] lastlvl;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey(LevelPrefName)){
            setLevel(1,1);
            PlayerPrefs.SetString(UnlockPrefNsame, formatSetter(1,1));
        }

        lastlvl = formatGetter(LastLevel);
        int[] lvl = formatGetter(getLevel());

        if(lvl[0] == 1 && lvl[1] == 1){
            _UIManager.toggleDecrease(false);
        }
        else if(lastlvl[0] == lvl[0] && lastlvl[1] == lvl[1]){
            _UIManager.toggleIncrease(false);
        }

        _UIManager.setLevel(lvl[0], lvl[1]);
    }

    public void startLevel(){
        _TextureLevelGenerator.GenerateLevel(getLevel());
        playerCam.Priority = 11;
        int[] lvl = formatGetter(getLevel());
        setLevelData(lvl[0], lvl[1]);

        _TickManager.gameStart.Invoke();
        _UIManager.activateGameUI();

        StartCoroutine(startCoroutine());

        if(lvl[0] == 1 && lvl[1] == 1){
            _TutorialController.levelOneTutorial();
        }
    }

    public void endGame(){
        _UIManager.win();
    }

    public void increaseLevel(){
        _UIManager.toggleDecrease(true);

        
        int[] lvlTmp = new int[2];
        lvlTmp = formatGetter(getLevel());

        if(lvlTmp[1] == 10){
            lvlTmp[0] ++;
            lvlTmp[1] = 1;
        }
        else{
            lvlTmp[1] ++;
        }

        _UIManager.setLevel(lvlTmp[0], lvlTmp[1]);

        setLevel(lvlTmp[0], lvlTmp[1]);
        
        if(lastlvl[0] == lvlTmp[0] && lastlvl[1] == lvlTmp[1]){
            _UIManager.toggleIncrease(false);
        }
    }

    public void decreaseLevel(){
        _UIManager.toggleIncrease(true);

        int[] lvlTmp = new int[2];
        lvlTmp = formatGetter(getLevel());

        if(lvlTmp[1] == 1 && lvlTmp[0] > 1){
            lvlTmp[1] = 10;
            lvlTmp[0] --;
        }
        else if(lvlTmp[0] >= 1 && lvlTmp[1] > 1){
            lvlTmp[1] --;
        }

        _UIManager.setLevel(lvlTmp[0], lvlTmp[1]);

        setLevel(lvlTmp[0], lvlTmp[1]);

        if(lvlTmp[0] == 1 && lvlTmp[1] == 1){
            _UIManager.toggleDecrease(false);
        }
    }

    IEnumerator startCoroutine(){
        yield return new WaitForSeconds(2f);
        _TickManager.startGame();

        yield return new WaitForEndOfFrame();
    }

    public void loadNextLevel(){
        //TODO: Remove enemy on reload
        _PlayerController.resetPlayer();
        _GridSystem.resetGrid();
        _ProgressManager.resetProgress();
        _UIManager.resetGameUI();
        increaseLevel();
        startLevel();
    }

    public void reloadLevel(){
        //TODO: Remove enemy on reload
        _PlayerController.resetPlayer();
        _GridSystem.resetGrid();
        _ProgressManager.resetProgress();
        _UIManager.resetGameUI();
        startLevel();
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
        _ProgressManager.setLevel(Objectives.levelData(dozen, figure));
    }

}
