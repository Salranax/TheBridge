using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    //A crappy tutorial system which must coded from scratch
    public GameObject swipeTutorial;

    public void levelOneTutorial(){
        StartCoroutine(tutorialSequence());
        
    }

    public void completeTutorial(){
        swipeTutorial.SetActive(false);
    }

    IEnumerator tutorialSequence(){
        float _timer = 0;

        while (_timer < 3f)
        {
            _timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        swipeTutorial.SetActive(true);

        _timer = 0;

        while (_timer < 6f)
        {
            _timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        completeTutorial();

        yield return new WaitForEndOfFrame();
    }
}
