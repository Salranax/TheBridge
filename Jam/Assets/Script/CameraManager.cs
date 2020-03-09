using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera startCam;
    public CinemachineVirtualCamera playerCam;
    public CinemachineVirtualCamera playerCamTOP;
    public CinemachineVirtualCamera playerCamLEFT;
    public CinemachineVirtualCamera playerCamRIGHT;
    public CinemachineVirtualCamera playerCamBACK;

    public void activateStartCam(){
        resetCams();
        startCam.Priority = 10;
    }

    public void activatePlayerCam(){
        resetCams();
        playerCam.Priority = 10;
    }

    public void activateLeftCam(){
        resetCams();
        playerCamLEFT.Priority = 10;
    }

    public void activateRightCam(){
        resetCams();
        playerCamRIGHT.Priority = 10;
    }

    public void activateBackCam(){
        resetCams();
        playerCamBACK.Priority = 10;
    }

    public void activateTopCam(){
        resetCams();
        playerCamTOP.Priority = 10;
    }

    private void resetCams(){
        startCam.Priority = playerCam .Priority = playerCamTOP.Priority = playerCamLEFT.Priority = playerCamRIGHT.Priority = playerCamBACK.Priority = 8;
    }
}
