using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;
    public GameObject objectPrefab;

    public List<GameObject> idleObjects;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null){
            instance = this;
        }
    }

    public GameObject getGameObject(){
        if(idleObjects.Count == 0){
            return Instantiate(objectPrefab) as GameObject;
        }
        else{
            GameObject tmp = idleObjects[0];
            idleObjects.RemoveAt(0);
            return tmp;
        }
    }

    public void retireObject(GameObject obj){
        obj.SetActive(false);
        idleObjects.Add(obj);
        Debug.Log("Retired");
    }
}
