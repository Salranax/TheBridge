using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    Quaternion startOrientation;

    private Vector3 offset;
    private Camera cam;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }

        if(cam == null){
            cam = Camera.main;
        }
    }

    void LateUpdate()
    {
        
        if(TickManager.instance.GetIsGameStarted()){
            transform.position = Vector3.Slerp(transform.position, PlayerController.instance.transform.position + offset, 1.5f * Time.deltaTime);
            //Debug.Log();
            //transform.position =  PlayerController.instance.transform.position + offset;
        }
    }

    public void startGame(){
        GameManager.instance.startLevel();
        StartCoroutine("startRoutine");
    }

    public IEnumerator startRoutine(){
        float t = 0;
        Vector3 axis = transform.InverseTransformDirection(Vector3.left);
        Color camStartC = cam.backgroundColor;


        while(t < 1){
            t += Time.deltaTime;
            transform.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x, Mathf.Lerp(90,0,t), 0);
            cam.backgroundColor = Color.Lerp(camStartC, Color.black, t);
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x,0, 0);

        TickManager.instance.startGame();

        offset = transform.position - PlayerController.instance.transform.position;

        yield return new WaitForEndOfFrame();
    }
}
