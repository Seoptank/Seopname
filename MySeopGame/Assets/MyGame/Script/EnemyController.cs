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
        //** ���� ���� ����
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Platform"));

        //** Ray������ 
        if (rayHit.collider == null)
        {
            Debug.Log("���!!");
        }

    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        //** �ִϸ��̼� ���
        if (nextMove == 0)
            ani.SetBool("IsWalk", false);
        else
            ani.SetBool("IsWalk", true);


        //** ������ȯ
        if (nextMove > 0)
            renderer.flipX = true;
        else
            renderer.flipX = false;

            Invoke("Think", 3); //**����Լ�
    }

}
