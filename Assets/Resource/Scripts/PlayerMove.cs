using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    Rigidbody2D rigid;

    [SerializeReference]
    float maxSpeed;

    [SerializeField]
    float minJumpPower;
    float jumpPower;
    [SerializeField]
    float jumpTimeLimit;

    bool isJumping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        DoJump();

        MoveStop();
    }

    private void FixedUpdate()
    {
        DoMove();

        JumpGauge();

        GetLandingPlatform();
    }

    private void DoMove()
    {
        // Move
        if (!Input.GetKey(KeyCode.Space) && !isJumping)
        {
            float h = Input.GetAxisRaw("Horizontal");

            rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

            // Max Speed
            if (rigid.velocity.x > maxSpeed)
            {
                rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
            }
            else if (rigid.velocity.x < maxSpeed * (-1))
            {
                rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);
            }
        }
    }

    private void DoJump()
    {
        // Jump
        if (Input.GetButtonUp("Jump") && !isJumping)
        {
            if(jumpPower < minJumpPower)
            {
                jumpPower = minJumpPower;
            }

            isJumping = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            rigid.velocity = new Vector2(maxSpeed * Input.GetAxisRaw("Horizontal"), rigid.velocity.y);

            jumpPower = 0;
        }
    }

    private void JumpGauge()
    {
        if(Input.GetKey(KeyCode.Space) && !isJumping)
        {
            rigid.velocity = new Vector2(0, 0);

            if(jumpTimeLimit > jumpPower)
            {
                jumpPower += 0.3f;
            }

            if(jumpTimeLimit < jumpPower)
            {
                jumpPower = jumpTimeLimit;
            }

            Debug.Log(jumpPower);
        }
    }

    private void MoveStop()
    {
        // Move Stop
        if (Input.GetButtonUp("Horizontal") && !isJumping)
        {
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    private void GetLandingPlatform()
    {
        // Landing Platform
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

        if (rigid.velocity.y < 0)
        {
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.85f)
                {
                    isJumping = false;
                    rigid.velocity = new Vector2(0, 0);
                }
            }
        }
    }
}
