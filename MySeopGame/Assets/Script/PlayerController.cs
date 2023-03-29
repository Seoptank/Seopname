using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed;
    private float maxSpeed;
    private float jumpPower;

    private bool isJump;

    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer playerRenderer;
    private Rigidbody2D rigid;


    

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerRenderer = GetComponent<SpriteRenderer>();

        rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        speed = 3.0f;
        maxSpeed = 3.0f;
        jumpPower = 10.0f;

        isJump = false;
    }

    private void Update()
    {
        PlayerMove();
        Jump();

        if (rigid.velocity.y < 0)
        {
            //** Jump Ray디버그
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            //** 빔에 대한 정보
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            //** Ray맞으면 
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.5f)
                {
                    //** "IsJump" false
                    animator.SetBool("IsJump", false);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        //** maxSpeed변수로 속도 조절
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1.0f))
            rigid.velocity = new Vector2(maxSpeed * (-1.0f), rigid.velocity.y);


    }



    void PlayerMove()
    {
        //** 플레이어 좌우이동
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigid.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
            playerRenderer.flipX = false;
            animator.SetBool("IsWalk", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigid.AddForce(new Vector2((-1.0f) *speed, 0.0f), ForceMode2D.Force);
            playerRenderer.flipX = true;
            animator.SetBool("IsWalk", true);
        }
        
        //** "IsWalk"Ani 멈춤
        if(Mathf.Abs(rigid.velocity.x) < 1.0f)
                animator.SetBool("IsWalk", false);
        
        //** 멈출때 속도
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(
                rigid.velocity.normalized.x * 0.5f,
                rigid.velocity.y);
        }
    }

    void Jump()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("IsJump",true);

            if (!isJump)
            {
                isJump = true;
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
            else
                return;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Floor"))
            isJump = false;
    }



}
