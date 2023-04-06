using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{
    private Animator ani;
    private bool touchDown;

    public GameController game;

    void Awake()
    {
        ani = GetComponent<Animator>();
    }
    void Start()
    {
        touchDown = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            touchDown = true;
            ani.SetBool("IsFinish", true);
            game.mainBtn.gameObject.SetActive(true);
        }
    }
}
