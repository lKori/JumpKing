using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallContact : MonoBehaviour
{
    GameObject player;
    Rigidbody2D playerRigidbody;

    PlayerMove playerMove;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        playerMove = player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 벽이 x 방향으로 충돌했을 대
        if (Math.Abs(collision.contacts[0].normal.y) < Math.Abs(collision.contacts[0].normal.x))
        {
            playerRigidbody.velocity = new Vector2(-playerMove.currentSpeedX, playerRigidbody.velocity.y);
        }
    }
}
