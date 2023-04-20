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

    // 색상 변경을 위한 변수's
    private float curTime;
    private float maxTime = 3f;
    private float r = 255f;
    private float g = 255f;
    private float b = 255f;
    private float colorCurve = -0.3f ;

    //동그라미 레이
    float maxDistance = 1.09f;
    float mySize = 0.16f;
    


    private void Awake()
    {
        bombRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        
        bombRenderer.color = new Color(r,g,b); 
    }

    private void Update()
    {
        
    }

    void DestroyBoomb()
    {
        Destroy(this.gameObject);
        Instantiate(effect, effectPos.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Invoke("DestroyBoomb", 3);

            for(curTime = 0f; curTime <= maxTime; curTime += Time.deltaTime )
            {

            }

            if (curTime >= 1)
            {
                g += curTime * colorCurve;
                b += curTime * colorCurve;
                if (g <= 120)
                {
                    colorCurve = -0.1f;
                    r += (curTime * -0.08f);
                }
            }
        }
    }

    void BombColorCange()
    {
        
        //curTime += Time.deltaTime;
        
        //for (float i = 0; i <= maxTime; i += 0.5f)
        //{

        //    bombRenderer.color = new Color()
        //}
    }

    void OnDrawGizmos()
    {
        Vector3 myPosition = transform.position;
        RaycastHit2D rayHit = Physics2D.CircleCast(myPosition, mySize, Vector2.up, maxDistance, LayerMask.GetMask("Enemy"));


        Gizmos.color = Color.red;

        if (rayHit.collider != null)
        {
            print("안부딧힘");
        }
        else
        {
            print("부딧힘");
        }
    }

    private bool IsTouchEnemy()
    {
        Vector3 myPosition = transform.position;
        RaycastHit2D rayHit = Physics2D.CircleCast(myPosition, mySize, Vector2.up, maxDistance, LayerMask.GetMask("Enemy"));
        return (rayHit.collider != null);
    }

}