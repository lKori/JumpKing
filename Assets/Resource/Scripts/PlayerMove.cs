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
    float maxSpeed; // 좌우 이동 최고속도

    [SerializeField]
    float minJumpPower; // 최소 점프 높이
    float jumpPower;    // 점프 충전 게이지
    [SerializeField]
    float jumpSpeedX;   // 점프 중 좌우 이동 속도
    [SerializeField]
    float jumpTimeLimit;    // 점프 게이지 최대 시간

    bool isJumping = false; // 점프 상태

    float playerPrevY;

    public enum PlayerStatus
    {
        Idle,   // 기본
        WalkLeft,   // 왼쪽으로 걷기
        WalkRight,  // 오른쪽으로 걷기
        Jump,   // 점프 중
        Sit,    // 점프 게이지 충전
        Fall    // 떨어지는 중
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
        // 캐릭터 점프
        DoJump();

        // 캐릭터 정지
        MoveStop();

        // 떨어지는 중인지 체크
        CheckPlayerFall();
    }

    private void FixedUpdate()
    {
        // 캐릭터 이동
        DoMove();

        // 점프 충전
        JumpGauge();

        // 벽에 부딪혔을 때 튕기기
        BouncePlayer();

        // 땅 위에 있는지 판별
        GetLandingPlatform();

        Debug.Log($"Player Status: {playerStatus}");
    }

    private void DoMove()
    {
        // 점프 상태가 아니며 점프 키 입력 중이 아닐 때 이동 가능
        if (!Input.GetKey(KeyCode.Space) && !isJumping)
        {
            // 입력 방향이 왼쪽인지 오른쪽인지
            float h = Input.GetAxisRaw("Horizontal");

            // 플레이어 상태 변경
            if (h == 1)
            {
                playerStatus = PlayerStatus.WalkRight;
            }
            else if(h == -1)
            {
                playerStatus = PlayerStatus.WalkLeft;
            }

            // 입력 방향으로 이동
            rigid.velocity = new Vector2(maxSpeed * h, rigid.velocity.y);
        }
    }

    private void MoveStop()
    {
        // 점프 상태가 아니며 좌우 키가 입력이 되지 않을 때 정지
        if (!Input.GetButtonUp("Horizontal") && !isJumping)
        {
            // 플레이어 상태 변경
            playerStatus = PlayerStatus.Idle;

            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }
    }

    private void DoJump()
    {
        // 점프 상태가 아니며 점프 키를 뗐을 때 점프 실행
        if (Input.GetButtonUp("Jump") && !isJumping)
        {
            // 플레이어 상태 변경
            playerStatus = PlayerStatus.Jump;

            // 점프 높이가 최소 점프 높이보다 낮으면 최소 점프 높이로 점프 실행
            if (jumpPower < minJumpPower)
            {
                jumpPower = minJumpPower;
            }

            // 점프 실행
            isJumping = true;
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            rigid.velocity = new Vector2(jumpSpeedX * Input.GetAxisRaw("Horizontal"), rigid.velocity.y);

            // 점프 충전 게이지 초기화
            jumpPower = 0;
        }
    }

    private void JumpGauge()
    {
        // 점프 중이 아니며 점프 키를 계속 누르고 있을 경우 점프 게이지 충전
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            playerStatus = PlayerStatus.Sit;

            // 점프 게이지 충전 중에는 이동 중지
            rigid.velocity = new Vector2(0, 0);

            if (jumpTimeLimit > jumpPower)
            {
                jumpPower += 0.5f;
            }

            // 최대 점프 높이보다 낮을 경우 게이지 충전
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

        float rayCastLen = 0.33f;   // Raycast 길이
        float checkDistance = 0.33f;    // Raycast 감지 거리

        RaycastHit2D rayHitRight = Physics2D.Raycast(rigid.position, Vector3.right, rayCastLen, LayerMask.GetMask("Platform")); // 오른쪽 벽 충돌 감지
        RaycastHit2D rayHitLeft = Physics2D.Raycast(rigid.position, Vector3.left, rayCastLen, LayerMask.GetMask("Platform"));   // 왼쪽 벽 충돌 감지

        // 캐릭터의 왼쪽 또는 오른쪽에서 벽이 감지 되었을 경우
        if (rayHitRight.collider != null || rayHitLeft.collider != null)
        {
            // 캐릭터와 벽 충돌 시 반대 방향으로 튕기기
            if (rayHitRight.distance < checkDistance || rayHitLeft.distance < checkDistance)
            {
                rigid.velocity = new Vector2(-rigid.velocity.x, rigid.velocity.y);
            }
        }
    }

    private void GetLandingPlatform()
    {
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 0.52f, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));    // 캐릭터 아래쪽으로 바닥 감지

        if (rigid.velocity.y < 0)
        {
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.52f)
                {
                    // 플레이어 상태 변경
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
