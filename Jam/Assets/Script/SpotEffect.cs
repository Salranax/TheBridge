using UnityEngine;

public class SpotEffect : MonoBehaviour
{
    private float spawntime = 0;
    public Light pointL;

    void Update()
    {
        spawntime += Time.deltaTime;

        if(spawntime < 0.4f){
            pointL.intensity = spawntime * 2;
        }
        else if(spawntime > 0.4f){
            pointL.intensity = 2 - (spawntime); 
        }

        if(spawntime > 1.2f){
            spawntime = 0;
            this.gameObject.SetActive(false);
        }
    }
}
