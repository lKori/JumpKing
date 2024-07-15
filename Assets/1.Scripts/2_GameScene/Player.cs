using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float ChargeMaxPower, ChargeSpeed, PlayerSpeed;

    private enum PlayerState { Idle, MoveLeft, MoveRight, Charge, Jump, Fall };
    [SerializeField]
    private PlayerState playerState;
    private float chargeTimer;

    private void Start()
    {
        playerState = PlayerState.Idle;
    }

    private void Update()
    {
        CheckFalling();

        switch(playerState)
        {
            case PlayerState.Idle:
                ZeroVelocity();
                GetUserInput();
                break;
            case PlayerState.MoveLeft:
                ZeroVelocity();
                MoveLeft();
                GetUserInput();
                break;
            case PlayerState.MoveRight:
                ZeroVelocity();
                MoveRight();
                GetUserInput();
                break;
            case PlayerState.Charge:
                Charging();
                break;
            case PlayerState.Fall:
                CheckStop();
                break;
        }
    }

    private void ZeroVelocity()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    private void GetUserInput()
    {
        if (Input.anyKey == false)
        {
            playerState = PlayerState.Idle;
            return;
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerState = PlayerState.Charge;
            chargeTimer = 0;
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerState = PlayerState.MoveLeft;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerState = PlayerState.MoveRight;
        }
    }

    private void MoveLeft()
    {
        this.transform.position += new Vector3(-PlayerSpeed * Time.deltaTime, 0, 0);
    }

    private void MoveRight()
    {
        this.transform.position += new Vector3(PlayerSpeed * Time.deltaTime, 0, 0);
    }

    private void Charging()
    {
        chargeTimer += Time.deltaTime * ChargeSpeed;

        if (chargeTimer > 10f)
            chargeTimer = 10f;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * chargeTimer * ChargeMaxPower);

            if (Input.GetKey(KeyCode.LeftArrow))
                this.GetComponent<Rigidbody2D>().AddForce(Vector2.left * chargeTimer * ChargeMaxPower * 0.5f);
            if (Input.GetKey(KeyCode.RightArrow))
                this.GetComponent<Rigidbody2D>().AddForce(Vector2.right * chargeTimer * ChargeMaxPower * 0.5f);

            playerState = PlayerState.Jump;
        }
    }

    private void CheckFalling()
    {
        if(this.GetComponent<Rigidbody2D>().velocity.y < 0f)
        {
            playerState = PlayerState.Fall;
        }
    }

    private void CheckStop()
    {
        if (this.GetComponent<Rigidbody2D>().velocity.y == 0f)
        {
            playerState = PlayerState.Idle;
        }
    }
}
