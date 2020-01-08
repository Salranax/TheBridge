using UnityEngine;

public class SpotEffect : MonoBehaviour
{
    private float spawntime = 0;
    public Light pointL;

    void Update()
    {
        spawntime += Time.deltaTime;

        if(spawntime < 1f){
            pointL.intensity = spawntime * 2;
        }
        else if(spawntime > 1f){
            pointL.intensity = 2 - (spawntime); 
        }

        if(spawntime > 2f){
            Destroy(this.gameObject);
        }
    }
}
