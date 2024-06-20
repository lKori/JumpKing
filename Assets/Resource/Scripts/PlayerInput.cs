using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    PlayerController mPlayerController;
    Rigidbody2D rigid;

    public float jumpPower;
    public float moveSpeed;
    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        mPlayerController = GetComponent<PlayerController>();
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        PCInput();
    }

    private void PCInput()
    {
        if(Input.GetButtonDown("Jump"))
        {
            DoJump();
        }

        if(Input.GetButtonDown("Horizontal"))
        {
            Debug.Log("move");

            float h = Input.GetAxisRaw("Horizontal");
            DoMove(h);
        }

    }

    private void DoJump()
    {
        rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
    }

    private void DoMove(float h)
    {
        rigid.AddForce(Vector2.right * moveSpeed * h, ForceMode2D.Impulse);

        if(rigid.velocity.x > maxSpeed)
        {
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        }
        else if(rigid.velocity.x < maxSpeed * (-1))
        {
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
        }
    }

    private void DoStop()
    {
    }
}
