using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridModule : MonoBehaviour
{
    public int gridSizeX, gridSizeY;
    public int offset;
    public gridType[,] gridArrangement;
    private GameObject[,] gridObjects;
    private bool isFirst;

    //Set Data && Trigger Generation
    public void construct(int x, int y, int os, bool first = false){
        gridSizeX = x;
        gridSizeY = y;
        offset = os;
        isFirst = first;
        Generate();
    }

    //Generate Module With Given Data
    private void Generate()
    {
        ObjectManager objectM = ObjectManager.instance;

        if(GridSystem.instance.activeModules.Count == 1 && isFirst){
            gridSizeX = 10;
            gridSizeY = 4;
            transform.localPosition = new Vector3(-5,1,0.7f);
            PlayerController.instance.setCurrentModule(this);
        }
        else if(GridSystem.instance.activeModules.Count == 2 && isFirst){
            gridSizeX = 5;
            offset = 3;
            transform.localPosition = new Vector3(-2,5,0.7f);
        }
        else{
            Vector2 lastModuleCoord = GridSystem.instance.activeModules[GridSystem.instance.activeModules.Count - 2].gameObject.transform.position;

            transform.localPosition = new Vector3(
                lastModuleCoord.x + offset, 
                lastModuleCoord.y + GridSystem.instance.activeModules[GridSystem.instance.activeModules.Count - 2].gridSizeY, 
                0.7f);
        }

        gridObjects = new GameObject[gridSizeY+ 1, gridSizeX + 1];
        gridArrangement = new gridType[gridSizeY+ 1, gridSizeX + 1];

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                GameObject tmp = objectM.getGameObject();
                gridObjects[y,x] = tmp;
                gridArrangement[y,x] = gridType.floor;
                tmp.transform.SetParent(this.transform);
                tmp.transform.localPosition = new Vector3(x, y, 0.2f);
            }
        }
    }

    public void retireModule(){
        foreach (GameObject item in gridObjects)
        {
            ObjectManager.instance.retireObject(item);
        }

        Destroy(this.gameObject);
    }

    public int getGridSizeX(){
        return gridSizeX;
    }

    public int getGridSizeY(){
        return gridSizeY;
    }
}
