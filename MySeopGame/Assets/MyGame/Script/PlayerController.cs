using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�� ������
//1. ���� �� ������ ������ ���� ��� �ӹ���
//2. Enemy�� ���� �� �÷��̾� ���� �ȵ�
public class PlayerController : MonoBehaviour
{
    public GameController gameController;

    private float speed;
    private float maxSpeed;
    private float jumpPower;
    
    private bool isJump;
    public bool haveBanana;

    

    private Vector2 movement;

    private Animator animator;
    private SpriteRenderer playerRenderer;
    private Rigidbody2D rigid;
    private CapsuleCollider2D capsul;

    // Bullet ������Ʈ, ���� ����
    public GameObject bullet;
    public Transform pos;

    public int bulletClip;
    public int maxBulletClip=10;
    public int minBulletClip=0;


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
        jumpPower = 13.0f;

        bulletClip = maxBulletClip;

        isJump = false;
        haveBanana = false;
    }

    private void Update()
    {
        PlayerMove();
        Jump();

       
        if (rigid.velocity.y < 0)
        {
            //** Jump Ray�����
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            //** ���� ���� ����
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            //** Ray������ 
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1.5f)
                {
                    //** "IsJump" false
                    animator.SetBool("IsJump", false);
                }
            }
        }

        //Bullet �߻�
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
                print("ź�� �پ�");
                bulletClip = minBulletClip;
                bullet.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        //** maxSpeed������ �ӵ� ����
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

        //** �÷��̾� �¿��̵�
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    rigid.AddForce(new Vector2(speed, 0.0f), ForceMode2D.Force);
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    rigid.AddForce(new Vector2((-1.0f) *speed, 0.0f), ForceMode2D.Force);
        //}

        
        //** ���⶧ �ӵ�
        //if (Input.GetButtonUp("Horizontal"))
        //{
        //    rigid.velocity = new Vector2(
        //        rigid.velocity.normalized.x * 0.5f,
        //        rigid.velocity.y);
        //}
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            if (isJump == false)
            {
                animator.SetBool("IsJump", true);
                isJump = true;
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            }
            else
                return;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            isJump = false;
            animator.SetBool("IsJump", false);

        }


        if (collision.gameObject.tag == "Enemy")
        {
            //** ����
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                OnAttack(collision.transform);

            //** ������
            else    
                OnDamaged(collision.transform.position);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item")
        {
            collision.gameObject.SetActive(false);
            //Point
            //�� Contains: ��� ���ڿ��� �񱳹��� ������ true
            bool coinB = collision.gameObject.name.Contains("CoinB");
            bool coinS = collision.gameObject.name.Contains("CoinS");
            bool coinG = collision.gameObject.name.Contains("CoinG");
            bool temAppleI = collision.gameObject.name.Contains("Apple");

            if (coinB)
            {
                gameController.coin++;
                gameController.point += 50;
            }
            
            if (temAppleI)
            {
                if(gameController.hp<3)
                {
                    gameController.hp ++;
                    
                }
                else if (gameController.hp >= 3)
                {
                    gameController.hp = 3;
                    collision.gameObject.SetActive(true);
                }

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

        //** �÷��̾� ���̾ 7������ �ٲ�
        gameObject.layer = 7;

        //** �÷��̾� ���� �ٲ�
        playerRenderer.color = new Color(1, 1, 1, 0.4f);

        //** �´� ������ ƨ����
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1)*7 , ForceMode2D.Impulse);

        //** �ִϸ��̼�
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

}
