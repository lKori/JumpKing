using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMove;

public class PlayerAnimation : MonoBehaviour
{
    public Sprite[] PlayerSprite;
    PlayerMove playerMove;
    PlayerStatus playerStatus;
    SpriteRenderer spriteRenderer;
    float delayTime = 0.5f;
    float coolTime;
    int i;

    void Start()
    {
        coolTime = 0;
    }

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        playerStatus = playerMove.GetPlayerStatus();
        PlayerAnimationChange();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayerAnimationChange()
    {
        switch (playerStatus)
        {
            case PlayerStatus.Idle:
                i = 0;
                spriteRenderer.sprite = PlayerSprite[0];
                Debug.Log("정지");
                //i = 0;
                //spriteRenderer.sprite = PlayerSprite[i];
                //while (playerStatus == PlayerStatus.Idle)
                //{
                //    coolTime += Time.deltaTime;
                //    if(coolTime > delayTime)
                //    {
                //        i++;
                //        spriteRenderer.sprite = PlayerSprite[i];
                //        if(i > 1)
                //        {
                //            i = 0;
                //        }
                //    }
                //}
                break;
            case PlayerStatus.WalkLeft:
                i = 1;
                spriteRenderer.sprite = PlayerSprite[1];
                Debug.Log("왼쪽걸음");

                break;
            case PlayerStatus.WalkRight:
                i = 4;
                spriteRenderer.sprite = PlayerSprite[4];
                Debug.Log("오른쪽걸음");

                break;
            case PlayerStatus.Jump:

                break;
            case PlayerStatus.Sit:

                break;
            case PlayerStatus.Fall:

                break;
        }
    }
}
