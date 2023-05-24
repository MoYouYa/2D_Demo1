using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        if(player==null)
        {
            player = FindObjectOfType<Player>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 playerPosition = player.transform.position;
            if (Math.Abs(playerPosition.x - transform.position.x) > 1.5f)
            {
                transform.Translate(new Vector2(playerPosition.x - transform.position.x, 0) * Time.deltaTime);
            }
            if (Math.Abs(playerPosition.y - transform.position.y) > 0.6f && transform.position.y>=-0.5f)
            {
                transform.Translate(new Vector2(0, playerPosition.y - transform.position.y) * Time.deltaTime);
            }
        }
        else
        {
            player = FindObjectOfType<Player>();
        }
    }
}
