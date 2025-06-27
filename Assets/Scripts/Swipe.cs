using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    Forward,
    Back, 
    Left, 
    Right,
    None
}
public class Swipe : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    [SerializeField]
    [Range(10f, 100f)]
    private float minimumSwipeDistance;

    public Direction GetSwipeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) 
        {
            endTouchPosition = Input.mousePosition;
            Vector2 direction = (endTouchPosition - startTouchPosition);
            if (direction.magnitude >= minimumSwipeDistance)
            {
                direction.Normalize();
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    if (direction.x > 0)
                        return Direction.Right;
                    if (direction.x < 0)
                        return Direction.Left;
                }
                else
                {
                    if (direction.y > 0)
                        return Direction.Forward;
                    if (direction.y < 0)
                        return Direction.Back;
                }

            }    
        }
        return Direction.None;
    }
}
