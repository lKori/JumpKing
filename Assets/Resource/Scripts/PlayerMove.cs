using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    bool isJumping = false; // ���� ����
    [SerializeField]
    bool onLanding = true;

    double playerPrevY;
    public float currentSpeedX;

    public enum PlayerStatus
    {
        Idle,   // �⺻
        WalkLeft,   // �������� �ȱ�
        WalkRight,  // ���������� �ȱ�
        Jump,   // ���� ��
        Sit,    // ���� ������ ����
        Fall    // �������� ��
    }

    [SerializeField]
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
        //BouncePlayer();

        // �� ���� �ִ��� �Ǻ�
        //GetLandingPlatform();

        //Debug.Log($"Player Status: {playerStatus}");
    }

    private void DoMove()
    {
        // ���� ���°� �ƴϸ� ���� Ű �Է� ���� �ƴ� �� �̵� ����
        if (onLanding && !Input.GetKey(KeyCode.Space) && !isJumping)
        {
            // �Է� ������ �������� ����������
            float h = Input.GetAxisRaw("Horizontal");

            // �÷��̾� ���� ����
            if (h == 1)
            {
                if (playerStatus != PlayerStatus.WalkRight)
                {
                    playerStatus = PlayerStatus.WalkRight;
                }
            }
            else if (h == -1)
            {
                if (playerStatus != PlayerStatus.WalkLeft)
                {
                    playerStatus = PlayerStatus.WalkLeft;
                }
            }

            // �Է� �������� �̵�
            rigid.velocity = new Vector2(maxSpeed * h, rigid.velocity.y);
        }
    }

    private void MoveStop()
    {
        // ���� ���°� �ƴϸ� �¿� Ű�� �Է��� ���� ���� �� ����
        //if (!Input.GetButtonUp("Horizontal") && !isJumping)
        if (onLanding && !Input.anyKey && !isJumping)
        {
            //�÷��̾� ���� ����
            if (playerStatus != PlayerStatus.Idle)
            {
                playerStatus = PlayerStatus.Idle;
            }

            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    private void DoJump()
    {
        // ���� ���°� �ƴϸ� ���� Ű�� ���� �� ���� ����
        if (onLanding && Input.GetButtonUp("Jump") && !isJumping)
        {
            // �÷��̾� ���� ����
            if (playerStatus != PlayerStatus.Jump)
            {
                playerStatus = PlayerStatus.Jump;
            }

            // ���� ���̰� �ּ� ���� ���̺��� ������ �ּ� ���� ���̷� ���� ����
            if (jumpPower < minJumpPower)
            {
                jumpPower = minJumpPower;
            }

            // ���� ����
            onLanding = false;
            isJumping = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            rigid.velocity = new Vector2(jumpSpeedX * Input.GetAxisRaw("Horizontal"), rigid.velocity.y);
            currentSpeedX = rigid.velocity.x;

            Debug.Log(jumpPower);

            // ���� ���� ������ �ʱ�ȭ
            jumpPower = 0;
        }
    }

    private void JumpGauge()
    {
        // ���� ���� �ƴϸ� ���� Ű�� ��� ������ ���� ��� ���� ������ ����
        if (onLanding && Input.GetKey(KeyCode.Space) && !isJumping)
        {
            if(playerStatus != PlayerStatus.Sit)
            {
                playerStatus = PlayerStatus.Sit;
            }

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

            //Debug.Log(jumpPower);
        }
    }

    private void BouncePlayer()
    {
        //Debug.DrawRay(rigid.position, Vector3.right, new Color(1, 0, 0));
        //Debug.DrawRay(rigid.position, Vector3.left, new Color(-1, 0, 0));

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
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 0.5f, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));    // ĳ���� �Ʒ������� �ٴ� ����

        if (rigid.velocity.y < 0)
        {
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.52f)
                {
                    isJumping = false;
                    rigid.velocity = new Vector2(0, 0);
                }
            }
        }
    }

    private void CheckPlayerFall()
    {
        double playerNowY = Math.Round(this.transform.position.y, 2);

        if (playerNowY < playerPrevY)
        {
            playerStatus = PlayerStatus.Fall;

            onLanding = false;
        }

        playerPrevY = playerNowY;
    }

    public PlayerStatus GetPlayerStatus()
    {
        return playerStatus;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"vector: {collision.contacts[0].normal}");

        if (collision.gameObject.tag == "Platform" && collision.contacts[0].normal.y > Math.Abs(collision.contacts[0].normal.x))    // ���� ����� ��
        {
            onLanding = true;
            isJumping = false;
            rigid.velocity = new Vector2(0, 0);
            currentSpeedX = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (rigid.velocity.y < 0)
        {
            if (collision.gameObject.tag == "Platform" && Math.Abs(collision.contacts[0].normal.y) > Math.Abs(collision.contacts[0].normal.x))
            {
                onLanding = true;
                isJumping = false;
                rigid.velocity = new Vector2(0, 0);
                currentSpeedX = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "EndPoint")
        {
            SceneManager.LoadScene("EndingTitle");
        }
    }
}
