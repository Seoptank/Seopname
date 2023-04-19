using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    private Animator ani;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            ani.SetBool("IsJump",true);

        }
        else
        {
            ani.SetBool("IsJump", false);
        }
    }


}
