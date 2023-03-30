using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�� ������
//1. ���� �� ������ ������ ���� ��� �ӹ���
//2. Enemy�� ���� �� �÷��̾� ���� �ȵ�
public class PlayerController : MonoBehaviour
{
    public GameManager gameManager;

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
        jumpPower = 13.0f;

        isJump = false;
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
        //** �÷��̾� �¿��̵�
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
        
        //** "IsWalk"Ani ����
        if(Mathf.Abs(rigid.velocity.x) < 1.0f)
                animator.SetBool("IsWalk", false);
        
        //** ���⶧ �ӵ�
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
            //Point

            //Deactive Item
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Finish")
        {
            //NextStage
        }
    }

    void OnDamaged(Vector2 targetPos)
    {
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

    void OnAttack(Transform enemy)
    {
        //Point

        //Reaction Force
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Enemy Die
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDamaged();
    }

    

}