using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//※ 문제점
//1. 벽에 꾹 누르고 있으면 벽에 계속 머무름
//2. Enemy와 닿을 시 플레이어 점프 안됨
public class PlayerController : MonoBehaviour
{
    public GameController gameController;

    public float speed;
    private float maxSpeed;

    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer playerRenderer;
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsul;

    // Bullet 오브젝트, 놓는 지점
    public GameObject bullet;
    public GameObject boomb;
    public Transform pos;

    public int bulletClip;
    public int maxBulletClip=10;
    public int minBulletClip=0;

    //** 점프를 위한 변수 
    private Vector2 boxCastSize = new Vector2(0.4f, 0.05f);
    private float boxCastMaxDistance = 0.7f;
    private float jumpPower;

    //** 오렌지 폭탄 변수
    public int orangeClip;
    public int maxOrangeClip;
    public int minOrangeClip;



    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerRenderer = GetComponent<SpriteRenderer>();

        rigid = GetComponent<Rigidbody2D>();

        capsul = GetComponent<CapsuleCollider2D>();


    }

    private void Start()
    {
        speed = 5.0f;
        maxSpeed = 5.0f;
        jumpPower = 15.0f;

        maxOrangeClip = 5;
        orangeClip = 0;
        minOrangeClip = 0;

        bulletClip = maxBulletClip;
    }

    private void Update()
    {
        PlayerMove();

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Jump();
        }


        if (rigid.velocity.y < 0)
        {
            //** Jump Ray디버그
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            //** 빔에 대한 정보
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            //** Ray맞으면 
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                {
                    //** "IsJump" false
                    animator.SetBool("IsJump", false);
                }
            }
        }

        //Bullet 발사
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if(bulletClip > minBulletClip)
            {
                bulletClip--;
                Instantiate(bullet, pos.position, transform.rotation);
                bullet.gameObject.SetActive(true);
            }
            
            if (bulletClip<= minBulletClip)
            {
                print("탄약 다씀");
                bulletClip = minBulletClip;
                bullet.gameObject.SetActive(false);
            }
        }

        //** Boomb놓기 
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            orangeClip--;
            Instantiate(boomb, pos.position, transform.rotation);
            
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
        float hor = Input.GetAxisRaw("Horizontal");

        transform.Translate(new Vector3(Mathf.Abs(hor) * speed * Time.deltaTime, 0, 0));
        if (hor > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            animator.SetBool("IsWalk", true);
        }
        else if (hor < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
            animator.SetBool("IsWalk", true);
        }
        else if (hor == 0)
            animator.SetBool("IsWalk", false);

    }

    


    private void OnCollisionEnter2D(Collision2D collision)
    {

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
            //Point
            //＃ Contains: 대상 문자열에 비교문이 있으면 true
            bool coinB = collision.gameObject.name.Contains("CoinB");
            bool coinS = collision.gameObject.name.Contains("CoinS");
            bool coinG = collision.gameObject.name.Contains("CoinG");
            bool temAppleI = collision.gameObject.name.Contains("Apple");
            bool temOrange = collision.gameObject.name.Contains("Orange");

            if (coinB)
            {
                gameController.coin++;
                gameController.point += 50;
                collision.gameObject.SetActive(false);

            }

            if (temAppleI)
            {
                if(gameController.hp<3)
                {
                    gameController.hp ++;
                    collision.gameObject.SetActive(false);


                }
                else if (gameController.hp >= 3)
                {
                    gameController.hp = 3;
                    collision.gameObject.SetActive(true);
                    
                }

            }

            if (temOrange)
            {
                orangeClip = maxOrangeClip;
                collision.gameObject.SetActive(false);

            }
        }

        if (collision.gameObject.tag == "Point")
        {
            gameController.VictoryUIStart();
           
        }

        
    }

    void OnAttack(Transform enemy)
    {
        //Point
        gameController.point += 100;

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

        gameController.LoseUIStart();

    }
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    //** 점프를 위한 함수들
    private bool IsOnGround()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Platform"));
        return (raycastHit.collider != null);
    }

    public void Jump()
    {
        if (IsOnGround())
        {
            this.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpPower,ForceMode2D.Impulse);
        }
    }

    void OnDrawGizmos()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(transform.position, boxCastSize, 0f, Vector2.down, boxCastMaxDistance, LayerMask.GetMask("Platform"));

        Gizmos.color = Color.red;

        if (raycastHit.collider != null)
        {
            Gizmos.DrawRay(transform.position, Vector2.down * raycastHit.distance);
            Gizmos.DrawWireCube(transform.position + Vector3.down * raycastHit.distance, boxCastSize);
        }
        else
        {
            Gizmos.DrawRay(transform.position, Vector2.down * boxCastMaxDistance);
        }
    }
    

}
