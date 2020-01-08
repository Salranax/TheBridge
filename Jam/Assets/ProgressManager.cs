using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public Slider progressSlider;
    public Image iconImage;

    private int progressStartPoint;

    private string lastProgress = "empty";

    private List<string> names = new List<string>(){"empty","fire", "glass","paper","balloon","windmills"} ;
    private List<int> check = new List<int>(){0 ,10, 20, 40, 80, 160  };
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.DeleteAll();
        progressStartPoint =  Mathf.FloorToInt(PlayerController.instance.transform.position.y);
        
        if (PlayerPrefs.HasKey("Progress"))
        {
            lastProgress = PlayerPrefs.GetString("Progress");
            progressStartPoint = check[names.IndexOf(lastProgress)];
            progressSlider.maxValue = check[names.IndexOf(lastProgress) + 1];
        }
        else
        {
            progressSlider.maxValue = check[names.IndexOf(lastProgress) + 1];

        }
        Debug.Log(lastProgress);

    }

    // Update is called once per frame
    void Update()
    {
       
        if (Mathf.FloorToInt(PlayerController.instance.transform.position.y) > progressStartPoint)
        {
            progressStartPoint = Mathf.FloorToInt(PlayerController.instance.transform.position.y);
            progressSlider.value++;
            
            if (Mathf.FloorToInt(progressSlider.value ) == Mathf.FloorToInt(progressSlider.maxValue))
            {
                progressSlider.value = 0;
                progressSlider.maxValue = check[names.IndexOf(lastProgress) + 1];
                PlayerPrefs.SetString("Progress",lastProgress);
                lastProgress = names[names.IndexOf(lastProgress) + 1];
                Debug.Log(lastProgress);
                iconImage.sprite = Resources.Load(lastProgress, typeof(Sprite)) as Sprite;
            } 
        }

    }
    
}
