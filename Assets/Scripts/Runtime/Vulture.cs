using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
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
    //private Vector2 flyDir;
    private float warningRadius;

    private Animator animator;
    // Start is called before the first frame update

    void Start()
    {
        vultureState = VultureState.Idle;
        faceDir = FaceDir.Left; //it needs reset localRotation,beacause its sprite was originally facing to the right
        transform.localRotation= new Quaternion(transform.localRotation.x, 180.0f, transform.localRotation.z, transform.localRotation.w);
        flySpeed = 1.0f;
        //flyDir=Vector2.zero;
        targetPos = new Vector2(float.MinValue, float.MinValue);
        warningRadius = 1.0f;

        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetTargetPlayerPos().ToString());
        StateAndAnimationTransition();
    }

    private void StateAndAnimationTransition()
    {
        switch (vultureState)
        {
            case VultureState.Idle:
                //targetPos = GetTargetPlayerPos();
                //Vector2 transform.position = transform.position;
                if (DiscoverPlayer())
                {
                    //Debug.Log("discover player");
                    targetPos =(Vector2)transform.position+ Vector2.up/4;
                    //flyDir = Vector2.up;
                    vultureState = VultureState.Fly;
                    warningRadius *= 3.0f;
                    //animator.SetBool("Fly", true);
                }
                break;
            case VultureState.Fly:
                //Debug.Log("fly");
                if (Math.Abs(targetPos.x-transform.position.x)>=1e-5 ||  Math.Abs(targetPos.y - transform.position.y) >= 1e-5)
                {
                    //Debug.Log($"take off dify={targetPos.y - transform.position.y} and difx={targetPos.x - transform.position.x}");
                    transform.Translate(new Vector2(targetPos.x-transform.position.x,targetPos.y-transform.position.y).normalized * Time.deltaTime*flySpeed,Space.World);
                }
                else
                {
                    //Debug.Log("fly over");
                    if(DiscoverPlayer())
                    {
                        targetPos = GetTargetPlayerPos();
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
                        //flyDir=new Vector2(targetPos.x-transform.position.x,targetPos.y-transform.position.y).normalized;
                        //Debug.Log($"fly discover player and this.pos={transform.position} player.pos={targetPos}");
                    }
                }
                break;
            case VultureState.Hurt:
                break;
            case VultureState.Death:
                break;

        }

        //if(faceDir)
        animator.SetInteger("vultureState", (int)vultureState);
    }

    private bool DiscoverPlayer()
    {
        Vector2 playerPos= GetTargetPlayerPos();
        //Debug.Log((playerPos.x - transform.position.x) * (playerPos.x - transform.position.x) + (playerPos.y - transform.position.y) * (playerPos.y - transform.position.y));
        //Debug.Log(warningRadius*warningRadius);
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
