using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameManager _GameManager;
    public GameObject objectPrefab;

    public List<GameObject> idleObjects;


    public GameObject getCubeGameObject(Vector2 _localPos, Quaternion _rotation){
        if(idleObjects.Count == 0){
            GameObject _newGenerated = Instantiate(objectPrefab, _GameManager._GridSystem.transform) as GameObject;
            _newGenerated.transform.localPosition = _localPos;
            return _newGenerated;
        }
        else{
            GameObject tmp = idleObjects[0];
            idleObjects.RemoveAt(0);
            tmp.transform.SetParent(_GameManager._GridSystem.transform);
            tmp.transform.localPosition = _localPos;
            return tmp;
        }
    }

    public void retireObject(GameObject obj){
        obj.SetActive(false);
        idleObjects.Add(obj); 
    }
}
