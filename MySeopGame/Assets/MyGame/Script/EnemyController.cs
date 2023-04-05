using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator ani;
    private SpriteRenderer renderer;
    private BoxCollider2D boxCollider;

    public int nextMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();

        Invoke("Think", 3);
    }

    private void FixedUpdate()
    {
        //** 이동
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        
        //** Platform끝 확인
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f , rigid.position.y);// 방향
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));
        if (rayHit.collider == null)
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 3);
        }
        //** 몬스터 방향 전환 
        if (nextMove > 0)
            renderer.flipX = true;
        else
            renderer.flipX = false;

    }

    void Think()
    {
        //** 다음 활동 설정
        nextMove = Random.Range(-1, 2);


        //** 애니메이션 재생
        if (nextMove == 0)
            ani.SetBool("IsWalk",false);
        else
            ani.SetBool("IsWalk",true);

        Invoke("Think", 3); //**재귀함수
    }

    public void OnDamaged()
    {
        //Sprite Alpha
        renderer.color = new Color(1, 1, 1, 0.4f);

        //Sprite FlipY
        renderer.flipY = true;

        //Collider Disable
        boxCollider.enabled = false;

        //Die Effect Jump
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Destroy
        Invoke("DeActive", 5);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            OnDamaged();
        }
    }

}
