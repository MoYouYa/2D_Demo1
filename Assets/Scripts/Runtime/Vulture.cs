using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VultureState
{
    Idle, Fly, Death
}
public class Vulture : Enermy
{
    private VultureState vultureState;
    private FaceDir faceDir;
    private float flySpeed;
    private Player targetPlayer;// player's position
    private float warningRadius;

    private Animator animator;
    // Start is called before the first frame update
    public Vulture():base()
    {
        enermyType = "Vulture";
    }

    void Start()
    {
        vultureState = VultureState.Idle;
        faceDir = FaceDir.Left; //it needs reset localRotation,beacause its sprite was originally facing to the right
        transform.localRotation= new Quaternion(transform.localRotation.x, 180.0f, transform.localRotation.z, transform.localRotation.w);
        flySpeed = 0.5f;
        warningRadius = 3.0f;

        animator=GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(GetTargetPlayerPos().ToString());
        switch (vultureState)
        {
            case VultureState.Idle:Idle();break;
            case VultureState.Fly:Fly();break;
        }
    }

    private void Idle()
    {
        Vector2 targetPos = GetTargetPlayerPos();
        Vector2 ownPos = transform.position;
        if( (targetPos.x-ownPos.x)* (targetPos.x - ownPos.x)+ (targetPos.x - ownPos.x)* (targetPos.x - ownPos.x) <= warningRadius* warningRadius)
        {
            vultureState = VultureState.Fly;
            animator.SetBool("Fly",true);
        }
    }

    private void Fly()
    {

    }

    public void SetTargetPlayer(Player player)
    {
        targetPlayer = player;
    }

    private Vector2 GetTargetPlayerPos()
    {
        return targetPlayer.transform.position;
    }
}
