using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePointerUI : MonoBehaviour
{   
    private List<GameObject> slots = new List<GameObject>();
    public GameObject target;
    public RectTransform pointerRectTransform;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null){
            float andgle = Vector3.Angle(Camera.main.transform.position, target.transform.position);

            pointerRectTransform.localEulerAngles = new Vector3(0, 0, andgle);
        }

    }

    public void updateObjectives(List<GameObject> _slots){
        slots = _slots;
    }

    // private float calculateUIAngle(){

    // }
}
