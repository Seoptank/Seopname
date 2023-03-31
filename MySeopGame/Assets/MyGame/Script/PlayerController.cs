using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//※ 문제점
//1. 벽에 꾹 누르고 있으면 벽에 계속 머무름
//2. Enemy와 닿을 시 플레이어 점프 안됨
public class PlayerController : MonoBehaviour
{
    public GameController gameController;

    private float speed;
    private float maxSpeed;
    private float jumpPower;
    private bool isJump;

    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer playerRenderer;
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsul;
    


    

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerRenderer = GetComponent<SpriteRenderer>();

        rigid = GetComponent<Rigidbody2D>();

        capsul = GetComponent<CapsuleCollider2D>();


    }

    private void Start()
    {
        speed = 3.0f;
        maxSpeed = 3.0f;
        jumpPower = 13.0f;

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Floor")
            isJump = false;

        if (collision.gameObject.tag == "Enemy")
        {
            //** 어택
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                OnAttack(collision.transform);

            //** 데미지
            else    
                OnDamaged(collision.transform.position);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            print("충돌");
            //Point
            //＃ Contains: 대상 문자열에 비교문이 있으면 true
            bool coinB = collision.gameObject.name.Contains("CoinB");
            bool coinS = collision.gameObject.name.Contains("CoinS");
            bool coinG = collision.gameObject.name.Contains("CoinG");
            bool jumpBooster = collision.gameObject.name.Contains("JumpBooster");

            if (coinB)
            {
                gameController.stagePoint += 50;
                print("코인 먹음!");
            }
            else if (coinS)
                gameController.stagePoint += 100;
            else if (coinG)
                gameController.stagePoint += 300;
            else if (jumpBooster)
                jumpPower = 20.0f;

            //Deactive Item
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //NextStage
            gameController.NextStage();
            animator.SetTrigger("IsFinish");
        }
    }

    void OnAttack(Transform enemy)
    {
        //Point
        gameController.stagePoint += 100;

        //Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Enemy Die
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDamaged();
    }


    void OnDamaged(Vector2 targetPos)
    {
        //HP Down
        gameController.HpDown();

        //** 플레이어 레이어를 7번으로 바꿈
        gameObject.layer = 7;

        //** 플레이어 색깔 바꿈
        playerRenderer.color = new Color(1, 1, 1, 0.4f);

        //** 맞는 방향대로 튕겨짐
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7 , ForceMode2D.Impulse);

        //** 애니메이션
        animator.SetTrigger("Damaged");

        Invoke("OffDamage", 2);
    }

    void OffDamage()
    {
        gameObject.layer = 3;
        
        playerRenderer.color = new Color(1, 1, 1, 1);

    }

    public void OnDie()
    {
        //Sprite Alpha
        playerRenderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite FlipY
        playerRenderer.flipY = true;

        //Collider Disable
        capsul.enabled = false;

        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }



}
