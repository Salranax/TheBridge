using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    Quaternion startOrientation;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    public void startGame(){
        StartCoroutine("startRoutine");
    }

    public IEnumerator startRoutine(){
        float degree = 0;
        Vector3 axis = transform.InverseTransformDirection(Vector3.left);

        while(degree < 1){
            degree += Time.deltaTime;
            transform.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x, Mathf.Lerp(90,0,degree), 0);
            //transform.rotation = startOrientation*Quaternion.AngleAxis(Mathf.Lerp(90,0,degree), axis);
            yield return new WaitForEndOfFrame();
        }

        transform.eulerAngles = new Vector3(transform.localRotation.eulerAngles.x,0, 0);
        //transform.rotation = startOrientation * Quaternion.AngleAxis(0, axis);

        TickManager.instance.startGame();

        yield return new WaitForEndOfFrame();
    }
}
