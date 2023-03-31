using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//** 점수, 스테이지 관리
public class GameController : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int hp;

    public  PlayerController player;
    public GameObject[] stage;

    public Image[] hpUI;
    public Text pointUI;
    public Text stageUI;
    public GameObject restatBtn;

    void Update()
    {
        pointUI.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        if (stageIndex < stage.Length)
        {
            stage[stageIndex].SetActive(false);
            stageIndex++;
            stage[stageIndex].SetActive(true);
            PlayerReposition();

            stageUI.text = "STAGE " + (stageIndex + 1);
        }
        else
        {
            //Game Clear

            //Player Contol Lock
            Time.timeScale = 0;

            //Result UI
            print("게임 클리어");

            //ReStart Button UI

        }

        //Calaulartion Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HpDown()
    {
        if (hp > 0)
        {
            hp--;
            hpUI[hp].color = new Color(1, 0, 0, 0.4f);
        }
        else if (hp < 1)
        {
            //Player Die Effect
            player.OnDie();

            //Result UI

            //Retry Button UI
            Text btnText = restatBtn.GetComponentInChildren<Text>();
            btnText.text = "Game Clear!";
            restatBtn.SetActive(true);

        }
        else
            return;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Player Reposition
            if (hp > 1)
                PlayerReposition();

            // HP Down
            HpDown();
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(-20, 1, 0);
        player.VelocityZero();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
