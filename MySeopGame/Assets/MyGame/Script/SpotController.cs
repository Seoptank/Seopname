using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotController : MonoBehaviour
{
    private Animator ani;
    private bool touchDown;

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        touchDown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            touchDown = true;
            ani.SetBool("IsFlag",true);
            Invoke("FlagAni", 2);
        }
    }

    void FlagAni()
    {
        if (touchDown == true)
            ani.SetBool("Flutter",true);
    }
}
