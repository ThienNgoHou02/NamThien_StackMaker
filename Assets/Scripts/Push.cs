using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    private enum PushDirection
    {
        [InspectorName("Forward")]
        Forward,
        [InspectorName("Back")]
        Back,
        [InspectorName("Left")]
        Left,
        [InspectorName("Right")]
        Right
    }
    [SerializeField] private PushDirection pushDirection;

    public Vector3 direction { get; private set; }

    private void Start()
    {
        direction = GetPushDirection(pushDirection);
    }
    private Vector3 GetPushDirection(PushDirection push)
    {
        switch (push)
        {
            case PushDirection.Forward:
                return Vector3.forward;
            case PushDirection.Back:
                return Vector3.back;
            case PushDirection.Left: 
                return Vector3.left;
            case PushDirection.Right:
                return Vector3.right;
        }
        return Vector3.zero;
    }
}
