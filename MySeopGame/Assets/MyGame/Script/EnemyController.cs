using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody2D rigid;
    private Animator ani;
    private SpriteRenderer renderer;

    public int nextMove;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();

        Invoke("Think", 3);
    }

    private void Update()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);


        Vector2 frontVec = new Vector2(rigid.position.x + nextMove, rigid.position.y);
        Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
        //** 빔에 대한 정보
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        //** Ray맞으면 
        if (rayHit.collider == null)
        {
            Debug.Log("경고!!");
        }

    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        //** 애니메이션 재생
        if (nextMove == 0)
            ani.SetBool("IsWalk", false);
        else
            ani.SetBool("IsWalk", true);


        //** 방향전환
        if (nextMove > 0)
            renderer.flipX = true;
        else
            renderer.flipX = false;

            Invoke("Think", 3); //**재귀함수
    }

}
