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

    public void setCameraDirection(MoveDirection _dir){
        //Resets All cam priorities to Default
        resetCamPriority();

        //Increase the priority of cam whick wanted to be the main
        if(_dir == MoveDirection.Forward){
            playerCam.Priority = 10;
        }
        else if(_dir == MoveDirection.Right){
            playerCamRIGHT.Priority = 10;
        }
        else if(_dir == MoveDirection.Left){
            playerCamLEFT.Priority = 10;
        }
        else if(_dir == MoveDirection.Back){
            playerCam.Priority = 10;
        }
    }

    private void resetCamPriority(){
        startCam.Priority = playerCam.Priority = playerCamTOP.Priority = playerCamLEFT.Priority = playerCamRIGHT.Priority = 8;
    }

    public void stopFollowing(){
        playerCam.Follow = null;
    }

    public void startFollowing(Transform _follow){
        playerCam.Follow = _follow;
    }

    public void moveToTopCam(){
        playerCamTOP.Priority = 10;
    }

}
