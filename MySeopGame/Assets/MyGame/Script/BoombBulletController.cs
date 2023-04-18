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
    public LayerMask isLayer;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        Invoke("DestroyBoomb", 3);


    }


    void DestroyBoomb()
    {
        Destroy(this.gameObject);
        Instantiate(effect, effectPos.position, transform.rotation);
    }

}
