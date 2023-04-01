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
    public GameObject restartBtn;

    void Update()
    {
        pointUI.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        //Change Stage
        if (stageIndex < stage.Length-1)
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

            //Player Control Lock
            Time.timeScale = 0;

            //ReStart Button UI
            Text btnText = restartBtn.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            restartBtn.SetActive(true);
            ViewBtn();

        }

        //Calaulartion Point
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HpDown()
    {
        if (hp > 1)
        {
            hp--;
            hpUI[hp].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            //All Health UI off
            hpUI[0].color = new Color(1, 0, 0, 0.4f);

            //Player Die Effect
            player.OnDie();


            //Result UI

            //Retry Button UI
            ViewBtn();
        }
        
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
        player.transform.position = new Vector3(-20, 1, 0);//Player Start Position
        player.VelocityZero();
    }
    void ViewBtn()
    {
        restartBtn.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

}
