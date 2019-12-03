using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightFollow : MonoBehaviour
{
    private Transform target;

    private void Start() {
        target = PlayerController.instance.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(target.position.x , target.position.y, transform.position.z);
    }
}
