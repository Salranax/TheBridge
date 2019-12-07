using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridModule : MonoBehaviour
{
    private int gridSizeX, gridSizeY;
    private int offset;
    private gridType[] gridArrangement;
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
        }
        else if(GridSystem.instance.activeModules.Count == 2 && isFirst){
            gridSizeX = 5;
            transform.localPosition = new Vector3(-2,5,0.7f);
        }
        else{
            Vector2 lastModuleCoord = GridSystem.instance.activeModules[GridSystem.instance.activeModules.Count - 2].gameObject.transform.position;

            transform.localPosition = new Vector3(
                lastModuleCoord.x + offset, 
                lastModuleCoord.y + GridSystem.instance.activeModules[GridSystem.instance.activeModules.Count - 2].gridSizeY, 
                0.7f);
        }

        for (int y = 0; y < gridSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                GameObject tmp = objectM.getGameObject();
                tmp.transform.SetParent(this.transform);
                tmp.transform.localPosition = new Vector3(x, y, -0.5f);
            }
        }
    }
}
