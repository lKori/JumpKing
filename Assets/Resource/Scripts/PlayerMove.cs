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
    float maxSpeed; // �¿� �̵� �ְ�ӵ�

    [SerializeField]
    float minJumpPower; // �ּ� ���� ����
    float jumpPower;    // ���� ���� ������
    [SerializeField]
    float jumpSpeedX;   // ���� �� �¿� �̵� �ӵ�
    [SerializeField]
    float jumpTimeLimit;    // ���� ������ �ִ� �ð�

    bool isJumping = false; // ���� ����

    float playerPrevY;

    public enum PlayerStatus
    {
        Idle,   // �⺻
        WalkLeft,   // �������� �ȱ�
        WalkRight,  // ���������� �ȱ�
        Jump,   // ���� ��
        Sit,    // ���� ������ ����
        Fall    // �������� ��
    }

    PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        playerStatus = PlayerStatus.Idle;
        playerPrevY = this.transform.position.y;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ĳ���� ����
        DoJump();

        // ĳ���� ����
        MoveStop();

        // �������� ������ üũ
        CheckPlayerFall();
    }

    private void FixedUpdate()
    {
        // ĳ���� �̵�
        DoMove();

        // ���� ����
        JumpGauge();

        // ���� �ε����� �� ƨ���
        BouncePlayer();

        // �� ���� �ִ��� �Ǻ�
        GetLandingPlatform();

        Debug.Log($"Player Status: {playerStatus}");
    }

    private void DoMove()
    {
        // ���� ���°� �ƴϸ� ���� Ű �Է� ���� �ƴ� �� �̵� ����
        if (!Input.GetKey(KeyCode.Space) && !isJumping)
        {
            // �Է� ������ �������� ����������
            float h = Input.GetAxisRaw("Horizontal");

            // �÷��̾� ���� ����
            if (h == 1)
            {
                playerStatus = PlayerStatus.WalkRight;
            }
            else if(h == -1)
            {
                playerStatus = PlayerStatus.WalkLeft;
            }

            // �Է� �������� �̵�
            rigid.velocity = new Vector2(maxSpeed * h, rigid.velocity.y);
        }
    }

    private void MoveStop()
    {
        // ���� ���°� �ƴϸ� �¿� Ű�� �Է��� ���� ���� �� ����
        if (!Input.GetButtonUp("Horizontal") && !isJumping)
        {
            // �÷��̾� ���� ����
            playerStatus = PlayerStatus.Idle;

            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    private void DoJump()
    {
        // ���� ���°� �ƴϸ� ���� Ű�� ���� �� ���� ����
        if (Input.GetButtonUp("Jump") && !isJumping)
        {
            // �÷��̾� ���� ����
            playerStatus = PlayerStatus.Jump;

            // ���� ���̰� �ּ� ���� ���̺��� ������ �ּ� ���� ���̷� ���� ����
            if (jumpPower < minJumpPower)
            {
                jumpPower = minJumpPower;
            }

            // ���� ����
            isJumping = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            rigid.velocity = new Vector2(jumpSpeedX * Input.GetAxisRaw("Horizontal"), rigid.velocity.y);

            // ���� ���� ������ �ʱ�ȭ
            jumpPower = 0;
        }
    }

    private void JumpGauge()
    {
        // ���� ���� �ƴϸ� ���� Ű�� ��� ������ ���� ��� ���� ������ ����
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            playerStatus = PlayerStatus.Sit;

            // ���� ������ ���� �߿��� �̵� ����
            rigid.velocity = new Vector2(0, 0);

            if (jumpTimeLimit > jumpPower)
            {
                jumpPower += 0.5f;
            }

            // �ִ� ���� ���̺��� ���� ��� ������ ����
            if (jumpTimeLimit < jumpPower)
            {
                jumpPower = jumpTimeLimit;
            }

            Debug.Log(jumpPower);
        }
    }

    private void BouncePlayer()
    {
        Debug.DrawRay(rigid.position, Vector3.right, new Color(1, 0, 0));
        Debug.DrawRay(rigid.position, Vector3.left, new Color(-1, 0, 0));

        float rayCastLen = 0.33f;   // Raycast ����
        float checkDistance = 0.33f;    // Raycast ���� �Ÿ�

        RaycastHit2D rayHitRight = Physics2D.Raycast(rigid.position, Vector3.right, rayCastLen, LayerMask.GetMask("Platform")); // ������ �� �浹 ����
        RaycastHit2D rayHitLeft = Physics2D.Raycast(rigid.position, Vector3.left, rayCastLen, LayerMask.GetMask("Platform"));   // ���� �� �浹 ����

        // ĳ������ ���� �Ǵ� �����ʿ��� ���� ���� �Ǿ��� ���
        if (rayHitRight.collider != null || rayHitLeft.collider != null)
        {
            // ĳ���Ϳ� �� �浹 �� �ݴ� �������� ƨ���
            if (rayHitRight.distance < checkDistance || rayHitLeft.distance < checkDistance)
            {
                rigid.velocity = new Vector2(-rigid.velocity.x, rigid.velocity.y);
            }
        }
    }

    private void GetLandingPlatform()
    {
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 0.52f, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));    // ĳ���� �Ʒ������� �ٴ� ����

        if (rigid.velocity.y < 0)
        {
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.52f)
                {
                    // �÷��̾� ���� ����
                    playerStatus = PlayerStatus.Idle;

                    isJumping = false;
                    rigid.velocity = new Vector2(0, 0);
                }
            }
        }
    }

    private void CheckPlayerFall()
    {
        float playerNowY = this.transform.position.y;

        if (playerNowY < playerPrevY)
        {
            playerStatus = PlayerStatus.Fall;
        }

        playerPrevY = playerNowY;
    }

    public PlayerStatus GetPlayerStatus()
    {
        return playerStatus;
    }
}
