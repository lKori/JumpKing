using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMove;

public class PlayerAnimation : MonoBehaviour
{
    private const float ANIM_FRAME = 0.5f;

    [SerializeField]
    public Sprite[] IdleSprite, WalkLeftSprite, WalkRightSprite, JumpSprite, SitSprite, FallSprite;

    private PlayerMove playerMove;
    private PlayerStatus playerStatus;
    private SpriteRenderer spriteRenderer;

    private int animIndex;
    private float timer;

    void Start()
    {
        playerMove = this.GetComponent<PlayerMove>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        PlayerStatusUpdate();

        if (timer >= ANIM_FRAME)
        {
            timer = 0;
            PlayerAnimationChange();
        }
    }

    private void PlayerAnimationChange()
    {
        switch (playerStatus)
        {
            case PlayerStatus.Idle:
                AnimIdle();
                break;
            case PlayerStatus.WalkLeft:
                AnimWalkLeft();
                break;
            case PlayerStatus.WalkRight:
                AnimWalkRight();
                break;
            case PlayerStatus.Jump:
                AnimJump();
                break;
            case PlayerStatus.Sit:
                AnimSit();
                break;
            case PlayerStatus.Fall:
                AnimFall();
                break;
        }
    }

    private void PlayerStatusUpdate()
    {
        PlayerStatus newPlayerStatus = playerMove.GetPlayerStatus();

        if (playerStatus == newPlayerStatus)
            return;

        playerStatus = newPlayerStatus;
        animIndex = 0;
        PlayerAnimationChange();
    }

    private void AnimIdle()
    {
        spriteRenderer.sprite = IdleSprite[animIndex];
        animIndex++;
        if (animIndex >= IdleSprite.Length)
            animIndex = 0;

        Debug.Log("Animation State: Idle");
    }
    private void AnimWalkLeft()
    {
        spriteRenderer.sprite = WalkLeftSprite[animIndex];
        animIndex++;
        if (animIndex >= WalkLeftSprite.Length)
            animIndex = 0;

        Debug.Log("Animation State: Walking left");
    }
    private void AnimWalkRight()
    {
        spriteRenderer.sprite = WalkRightSprite[animIndex];
        animIndex++;
        if (animIndex >= WalkRightSprite.Length)
            animIndex = 0;

        Debug.Log("Animation State: Walking right");
    }
    private void AnimJump()
    {
        spriteRenderer.sprite = JumpSprite[animIndex];
        animIndex++;
        if (animIndex >= JumpSprite.Length)
            animIndex = 0;

        Debug.Log("Animation State: Jump");
    }
    private void AnimSit()
    {
        spriteRenderer.sprite = SitSprite[animIndex];
        animIndex++;
        if (animIndex >= SitSprite.Length)
            animIndex = 0;

        Debug.Log("Animation State: Sit");
    }
    private void AnimFall()
    {
        spriteRenderer.sprite = FallSprite[animIndex];
        animIndex++;
        if (animIndex >= FallSprite.Length)
            animIndex = 0;

        Debug.Log("Animation State: Fall");
    }
}
