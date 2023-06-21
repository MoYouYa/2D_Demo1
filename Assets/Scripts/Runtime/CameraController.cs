using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Border
{
    public float left,right,top,bottom;
    public Border(float l, float r, float t, float b)
    {
        left = l;
        right = r;
        top = t;
        bottom = b;
    }
}
public class CameraController : MonoBehaviour
{
    private Player player;
    private Border border;

    // Start is called before the first frame update
    void Start()
    {
        if(player==null)
        {
            player = FindObjectOfType<Player>();
        }
        border=new Border(float.MinValue,float.MaxValue,2.0f,-1.0f);
    }

    // Update is called once per frame

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            if(playerPosition.x >= border.left && playerPosition.x<=border.right 
                && playerPosition.y>=border.bottom && playerPosition.y<=border.top)
            {
                if (Math.Abs(playerPosition.x - transform.position.x) > 1.5f)
                {
                    transform.Translate(new Vector2(playerPosition.x - transform.position.x, 0).normalized * Time.deltaTime);
                }
                if (Math.Abs(playerPosition.y - transform.position.y) > 0.6f )
                {
                    transform.Translate(new Vector2(0, playerPosition.y - transform.position.y).normalized * Time.deltaTime);
                }

            }
        }
        else
        {
            player = FindObjectOfType<Player>();
        }
    }
}
