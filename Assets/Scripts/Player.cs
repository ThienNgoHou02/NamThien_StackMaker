using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    const float GAME_UNIT = 0.25f;
    const float PLAYER_HEIGHT = 0.3f;

    [SerializeField] Animator animator;

    Swipe swipe;

    [SerializeField]
    private Transform rayCastPosition;

    [SerializeField]
    private Transform bodyTransform;    //Body chứa gạch đã nhặt

    [SerializeField]
    private Transform playerTransform;  //Hình ảnh nhân vật

    [SerializeField]
    private Transform bridgeTransform;  //Cầu để đặt gạch


    [SerializeField]
    [Range(0f, 5f)]
    private float rayCastLenght;

    [SerializeField]
    private LayerMask wallLayerMask;    
    [SerializeField]
    private LayerMask brickLayerMask;    
    [SerializeField]
    private LayerMask bridgeLayerMask;    
    [SerializeField]
    private LayerMask pushLayerMask;    
    [SerializeField]
    private LayerMask finishLayerMask;    
    [SerializeField]
    private LayerMask chestLayerMask;    
    [SerializeField]
    private LayerMask starLayerMask;

    private Direction swipeInput;
    private Vector3 moveDirection;
    private Vector3 toWard;

    private int brickCount;

    private bool isMoving;
    private bool onStep;

    private int JUMP = Animator.StringToHash("Jump");
    private int VICTORY = Animator.StringToHash("Victory");

    private void Start()
    {
        swipe = GetComponent<Swipe>();
        brickCount = 1;
    }
    private void Update()
    {
       if (!isMoving)
       {
            GameObject push = ObjectTouched(new Vector3(rayCastPosition.position.x, rayCastPosition.position.y + 0.5f, rayCastPosition.position.z), Vector3.down, pushLayerMask);
            if (push)
            {
                moveDirection = push.GetComponent<Push>().direction;
                if (moveDirection != Vector3.zero)
                    isMoving = true;
            }
            else
            {
                swipeInput = swipe.GetSwipeInput();
                if (swipeInput != Direction.None)
                {
                    isMoving = true;
                    moveDirection = ConvertEnumToVector(swipeInput);
                }
            }
       }
       if (isMoving)
       {
            //Nhặt gạch
            GameObject brick = ObjectTouched(new Vector3(rayCastPosition.position.x, rayCastPosition.position.y + 0.5f, rayCastPosition.position.z), Vector3.down, brickLayerMask);
            if (brick && Vector3.Distance(transform.position, brick.transform.position) < 0.01f)
            {
                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (stateInfo.IsName("Idle"))
                {
                    SetAnimation(JUMP);
                }

                brick.GetComponent<BoxCollider>().enabled = false;
                Vector3 lastBrickPos = Vector3.zero;
                if (bodyTransform.childCount - 1 > 0)
                {
                    Transform lastBrick = bodyTransform.GetChild(bodyTransform.childCount - 1).transform;   
                    lastBrickPos = lastBrick.position;
                }
                brick.transform.SetParent(bodyTransform);
                brick.transform.localPosition = new Vector3(0f, lastBrickPos.y + GAME_UNIT, 0f);
                playerTransform.localPosition = new Vector3(0f, brick.transform.localPosition.y + PLAYER_HEIGHT, 0f);
                brickCount++;
            }

            //Xây cầu
            Vector3 position = transform.position - moveDirection * 0.5f;
            position.y += 0.5f;
            GameObject bridge = ObjectTouched(position, Vector3.down, bridgeLayerMask);
            if (bridge && brickCount > 0)
            {
                bridge.SetActive(false);
                Transform lastBrick = bodyTransform.GetChild(bodyTransform.childCount - 1).transform;
                lastBrick.transform.SetParent(bridgeTransform);
                lastBrick.transform.localPosition = new Vector3(bridge.transform.localPosition.x, bridge.transform.localPosition.y - 0.125f, bridge.transform.localPosition.z);
                brickCount--;

                playerTransform.localPosition = new Vector3(0f, bodyTransform.GetChild(bodyTransform.childCount - 1).transform.localPosition.y + PLAYER_HEIGHT, 0f);
            }

            //Về đích
            if (FinishLineTouched(moveDirection))
            {
                for (int i = 1; i < bodyTransform.childCount; i++)
                {
                    bodyTransform.GetChild(i).gameObject.SetActive(false);
                }
                playerTransform.localPosition = Vector3.zero;
                VFX.instance.ActiveVFX();
                GameManager.instance.CompletePanel();
            }

            //Chạm rương
            if (ChestTouched(moveDirection))
            {
                isMoving = false;
                Quaternion rotation = Quaternion.Euler(0f, 205f, 0f);
                playerTransform.localRotation = rotation;
                SetAnimation(VICTORY);
            }

            //Nhặt sao
            GameObject star = ObjectTouched(new Vector3(rayCastPosition.position.x, rayCastPosition.position.y + 0.5f, rayCastPosition.position.z), moveDirection, starLayerMask);
            if (star)
            {
                GameManager.instance.StarCollect();
                star.SetActive(false);
            }    

            if (WallTouched(moveDirection) || (ObjectTouched(rayCastPosition.position, moveDirection, bridgeLayerMask) && brickCount <= 0))
            {
                isMoving = false;
            }
            else
                Move();
       }
    }
    private void Move()
    {
        if (!onStep)
        {
            toWard = transform.position + moveDirection * 0.5f;
            onStep = true;
        }
        if (onStep)
        {
            transform.position = Vector3.MoveTowards(transform.position, toWard, 15f * Time.deltaTime);
            if (Vector3.Distance(transform.position, toWard) < 0.01f)
            {
                transform.position = toWard;
                onStep = false;
            }
        }
    }
    private void SetAnimation(int hash)
    {
        animator.SetTrigger(hash);
    }
    private Vector3 ConvertEnumToVector(Direction direction)
    {
        Vector3 vector3 = Vector3.zero;
        switch (direction)
        {
            case Direction.Forward:
                vector3 = Vector3.forward;
                break;
            case Direction.Back:
                vector3 = Vector3.back;
                break;
            case Direction.Left:
                vector3 = Vector3.left;
                break;
            case Direction.Right:
                vector3 = Vector3.right;
                break;
        }
        return vector3;
    }
    private bool WallTouched(Vector3 direction)
    {
        return Physics.Raycast(rayCastPosition.position, direction, rayCastLenght, wallLayerMask);
    }
    private bool FinishLineTouched(Vector3 direction)
    {
        return Physics.Raycast(rayCastPosition.position, direction, rayCastLenght, finishLayerMask);
    }
    private bool ChestTouched(Vector3 direction)
    {
        return Physics.Raycast(rayCastPosition.position, direction, rayCastLenght + 0.25f, chestLayerMask);
    }
    private GameObject ObjectTouched(Vector3 position, Vector3 direction, LayerMask layerMask)
    {
        GameObject touched = null;
        RaycastHit hit;
        if (Physics.Raycast(position, direction, out hit, rayCastLenght, layerMask))
        {
            touched = hit.collider.gameObject;
        }
        return touched;
    }
}
