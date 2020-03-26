using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    private void Update()
    {   
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
                PlayerController.instance.setTouchHoldStatus(true);
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }

        if(Input.GetKeyDown(KeyCode.P)){
            TickManager.instance.SetIsGameStarted(true);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            PlayerController.instance.swipeLeft();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            PlayerController.instance.swipeRight();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            PlayerController.instance.swipeUp();
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            PlayerController.instance.swipeDown();
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            PlayerController.instance.setTouchHoldStatus(true);
        }
        if(Input.GetKeyUp(KeyCode.Space)){
            PlayerController.instance.setTouchHoldStatus(false);
        }
    }

    private void DetectSwipe()
    {
        PlayerController.instance.setTouchHoldStatus(false);
        if (SwipeDistanceCheckMet())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
            fingerUpPosition = fingerDownPosition;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private bool SwipeDistanceCheckMet()
    {
        return VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe;
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
        PlayerController.instance.getSwipe(swipeData);
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

