using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum PlayerState
{
    Idle, Run, Jump, Fall, Hurt, Death
}

public enum FaceDir
{
    Left, Right
}

public class Player : MonoBehaviour
{

    private PlayerState playerState;
    private FaceDir faceDir;
    private int jumpCount;
    private float runSpeed;
    private float jumpSpeed;

    private Animator animator;
    

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Update()
    {
        GetKeyboardInput();
        AnimationAndStateTransition();
    }

    private void Init()
    {
        playerState = PlayerState.Idle;
        faceDir = FaceDir.Right;
        jumpCount = 0;
        runSpeed = 1.0f;
        jumpSpeed = 3.4f;
        animator = GetComponent<Animator>();
    }

    private void GetKeyboardInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-runSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(runSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, GetComponent<Rigidbody2D>().velocity.y);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            if((playerState==PlayerState.Idle || playerState==PlayerState.Run || playerState==PlayerState.Fall)&&jumpCount<2)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,jumpSpeed);
                jumpCount++;
            }
            Debug.Log("jumpCount=" + jumpCount);
        }
    }

    private void AnimationAndStateTransition()
    {
        float FLOAT_PERCISION = 1e-5f;
        Vector2 newSpeed=GetComponent<Rigidbody2D>().velocity;
        switch (playerState)
        {
            case PlayerState.Idle:
                if (newSpeed.x >= FLOAT_PERCISION || newSpeed.x<=-FLOAT_PERCISION)
                {
                    //Debug.Log("newSpeed.x=" + newSpeed.x);
                    playerState = PlayerState.Run;
                }
                if (newSpeed.y >= jumpSpeed- FLOAT_PERCISION)
                {
                    playerState= PlayerState.Jump;
                }
                break;
            case PlayerState.Run:
                if(newSpeed.x>=-FLOAT_PERCISION && newSpeed.x <= FLOAT_PERCISION)
                {
                    playerState= PlayerState.Idle;
                }
                if (newSpeed.y >= jumpSpeed- FLOAT_PERCISION)
                {
                    playerState = PlayerState.Jump;
                    //Debug.Log("newSpeed.y=" + newSpeed.y.ToString());
                }
                //else if (newSpeed.y <= -FLOAT_PERCISION*5.0f /*-(jumpSpeed - FLOAT_PERCISION) /4.0f*/)
                //{
                //    playerState = PlayerState.Fall;
                //    Debug.Log("newSpeed.y=" + newSpeed.y.ToString());
                //}
                break;
            case PlayerState.Jump:
                if (newSpeed.y <= -FLOAT_PERCISION)
                {
                    playerState= PlayerState.Fall;
                }
                break;
            case PlayerState.Fall:
                if (newSpeed.y >= jumpSpeed-FLOAT_PERCISION)
                {
                    playerState= PlayerState.Jump;
                }
                else if(newSpeed.y>=-FLOAT_PERCISION /*&& newSpeed.y <= FLOAT_PERCISION*/)
                {
                    jumpCount = 0;
                    playerState= PlayerState.Idle;
                }
                break;
            case PlayerState.Hurt:
                break;
        }

        //�������ǵ�����
        if (newSpeed.x >= 1e-5f)
        {
            if (faceDir != FaceDir.Right)
            {
                faceDir = FaceDir.Right;
                transform.localRotation = new Quaternion(transform.localRotation.x, 0.0f, transform.localRotation.z, transform.localRotation.w);
            }
        }
        else if (newSpeed.x <= -1e-5f)
        {
            if (faceDir != FaceDir.Left)
            {
                faceDir = FaceDir.Left;
                transform.localRotation = new Quaternion(transform.localRotation.x, 180.0f, transform.localRotation.z, transform.localRotation.w);
            }
        }

        //���Ķ�����״̬
        animator.SetInteger("playerState", (int)playerState);
    }


}