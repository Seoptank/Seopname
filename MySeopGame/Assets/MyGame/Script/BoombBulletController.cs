using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoombBulletController : MonoBehaviour
{
    public GameObject effect;
    public Transform effectPos;

    private float speed = 0.01f;
    private float distance;

    private Rigidbody2D rigid;
    private SpriteRenderer bombRenderer;
    public LayerMask isLayer;

    // 색상 변경을 위한 변수
    private float maxTime = 5.0f;
    public Color startColor = Color.white;
    public Color endColor = Color.red;
    [Range(0, 10)]
    public float colorChangespeed = 1;


    //동그라미 레이
    float maxDistance = 1.09f;
    float mySize = 0.3f;
    


    private void Awake()
    {
        bombRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            bombRenderer.color = Color.Lerp(startColor, endColor, Mathf.PingPong(Time.time * colorChangespeed, 1));
        }
    }


    void DestroyBoomb()
    {
        Destroy(this.gameObject);
        Instantiate(effect, effectPos.position, transform.rotation);
    }

    private void OnDrawGizmos()
    {
        Vector3 myPosition = transform.position;
        RaycastHit2D rayHit = Physics2D.CircleCast(myPosition, mySize, Vector2.up, maxDistance, LayerMask.GetMask("Enemy"));



        if (rayHit.collider != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * rayHit.distance, mySize);
            Invoke("DestroyBoomb", maxTime);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position + Vector3.down * rayHit.distance, mySize);
        }
    }
}

