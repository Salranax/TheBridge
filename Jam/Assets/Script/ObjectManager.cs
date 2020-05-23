using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameManager _GameManager;
    public GameObject objectPrefab;
    public GameObject projectilePrefab;

    public List<GameObject> idleObjects;
    private List<GameObject> projectiles = new List<GameObject>();


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
            tmp.SetActive(true);
            return tmp;
        }
    }

    public GameObject getProjectile(){
        if(projectiles.Count == 0){
            GameObject _newGenerated = Instantiate(projectilePrefab, _GameManager._GridSystem.transform) as GameObject;
            return _newGenerated;
        }
        else{
            GameObject tmp = projectiles[0];
            projectiles.RemoveAt(0);
            tmp.SetActive(true);
            return tmp;
        }
    }

    public void retireObject(GameObject obj){
        if(obj != null){
            obj.SetActive(false);
            idleObjects.Add(obj); 
        }
    }

    public void retireProjectile(GameObject pj){
        pj.SetActive(false);
        projectiles.Add(pj);
    }
}
