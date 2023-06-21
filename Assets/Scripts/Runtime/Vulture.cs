using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VultureState
{
    Idle, Fly,Hurt, Death
}
public class Vulture : Enermy
{
    private VultureState vultureState;
    private FaceDir faceDir;
    private float flySpeed;
    private Vector2 targetPos;// player's position

    //private float debugTime = -1.0f;

    private float warningRadius;

    private Animator animator;
    // Start is called before the first frame update

    void Start()
    {
        vultureState = VultureState.Idle;
        faceDir = FaceDir.Left; //it needs reset localRotation,beacause its sprite was originally facing to the right
        transform.localRotation= new Quaternion(transform.localRotation.x, 180.0f, transform.localRotation.z, transform.localRotation.w);
        flySpeed = 1.0f;
        targetPos = new Vector2(float.MinValue, float.MinValue);
        warningRadius = 1.0f;

        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StateAndAnimationTransition();
    }

    private void StateAndAnimationTransition()
    {
        switch (vultureState)
        {
            case VultureState.Idle:
                if (DiscoverPlayer())
                {
                    targetPos =(Vector2)transform.position+ Vector2.up/4;
                    vultureState = VultureState.Fly;
                    warningRadius *= 3.0f;
                }
                break;
            case VultureState.Fly:
                if (Math.Abs(targetPos.x-transform.position.x)>= CustomData.FLOAT_PERCISION ||  Math.Abs(targetPos.y - transform.position.y) >= CustomData.FLOAT_PERCISION)
                {
                    //if (Time.time - debugTime > 1.0f)
                    //{
                    //    debugTime= Time.time;
                    //    Debug.Log($"now position={transform.position} and targetPos={targetPos}");
                    //}
                    transform.Translate(new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized * Time.deltaTime*flySpeed,Space.World);
                }
                else
                {
                    if(DiscoverPlayer())
                    {
                        targetPos = GetTargetPlayerPos();
                        //Debug.Log($"update targetPos={targetPos}");
                        if (targetPos.x - transform.position.x > 0 && faceDir==FaceDir.Left)
                        {
                            faceDir = FaceDir.Right;
                            transform.localRotation = new Quaternion(transform.localRotation.x, 0.0f, transform.localRotation.z, transform.localRotation.w);
                        }
                        else if(targetPos.x - transform.position.x <= 0 && faceDir == FaceDir.Right)
                        {
                            faceDir = FaceDir.Left;
                            transform.localRotation = new Quaternion(transform.localRotation.x, 180.0f, transform.localRotation.z, transform.localRotation.w);
                        }
                    }
                }
                break;
            case VultureState.Hurt:
                break;
            case VultureState.Death:
                break;

        }

        animator.SetInteger("vultureState", (int)vultureState);
    }

    private bool DiscoverPlayer()
    {
        Vector2 playerPos= GetTargetPlayerPos();
        return (playerPos.x - transform.position.x) * (playerPos.x - transform.position.x) + (playerPos.y - transform.position.y) * (playerPos.y - transform.position.y) <= warningRadius * warningRadius;
    }

    private Vector2 GetTargetPlayerPos()
    {
        Player player= GameManager.Instance.GetPlayer();
        if(player != null )
        {
            return player.transform.position;
        }
        else
        {
            return new Vector2(float.MinValue,float.MinValue);
        }
    }
}
