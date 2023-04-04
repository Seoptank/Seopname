using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//** 점수, 스테이지 관리
public class GameController : MonoBehaviour
{
    public int totalPoint;
    public int stageIndex;
    public int hp;
    public int coin;
    public int point;

    public static int maxHp = 3;

    public  PlayerController player;
    public GameObject[] stage;


    public Image[] hpUI;
    public Text pointUI;
    public Text stageUI;
    public GameObject restartBtn;

    //정보창UI
    [Header("정보창UI")]
    public Text infoStateUI;
    [SerializeField]
    public Text infoStageUI;
    public Text infoStagePointUI;
    public Text infoCoinUI;


    void Update()
    {
        //플레이어 coin값을 계속 받아옴
        coin = player.coin;
        point = player.point;

        pointUI.text = point.ToString();


        // 정보창UI
        infoStageUI.text = "Stage " + (stageIndex + 1).ToString();
        infoStagePointUI.text = "Stage Point : " + point.ToString();
        infoCoinUI.text = "Stage Coin : " + coin.ToString();

       
        //hpUI 상태
        if (hp == 0)
        {
            hpUI[0].color = new Color(1, 0, 0, 0.4f);
            hpUI[1].color = new Color(1, 0, 0, 0.4f);
            hpUI[2].color = new Color(1, 0, 0, 0.4f);
        }
        else if (hp == 1)
        {
            hpUI[0].color = new Color(1, 1, 1, 1);
            hpUI[1].color = new Color(1, 0, 0, 0.4f);
            hpUI[2].color = new Color(1, 0, 0, 0.4f);
        }
        else if (hp == 2)
        {
            hpUI[0].color = new Color(1, 1, 1, 1);
            hpUI[1].color = new Color(1, 1, 1, 1);
            hpUI[2].color = new Color(1, 0, 0, 0.4f);
        }
        else if (hp == 3)
        {
            hpUI[0].color = new Color(1, 1, 1, 1);
            hpUI[1].color = new Color(1, 1, 1, 1);
            hpUI[2].color = new Color(1, 1, 1, 1);
        }

       


    }

    //public void NextStage()
    //{
    //    //Change Stage
    //    if (stageIndex < stage.Length-1)
    //    {
    //        stage[stageIndex].SetActive(false);
    //        stageIndex++;
    //        stage[stageIndex].SetActive(true);
    //        PlayerReposition();

    //        stageUI.text = "STAGE " + (stageIndex + 1);
    //    }
    //    else
    //    {
    //        //Game Clear

    //        //Player Control Lock
    //        Time.timeScale = 0;

    //        //ReStart Button UI
    //        Text btnText = restartBtn.GetComponentInChildren<Text>();
    //        btnText.text = "Clear!";
    //        restartBtn.SetActive(true);
    //        ViewBtn();

    //    }

    //    //Calaulartion Point
    //    point = 0;
    //}

    public void HpDown()
    {
        if (hp > 1)
            hp--;
        else
        {
            //Player Die Effect
            player.OnDie();

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

        if (collision.gameObject.name == "Apple")
        {
            if (hp < maxHp)
            {
                hp++;
            }
            
            else if (hp >= maxHp)
            {
                collision.gameObject.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "Item")
        {
            bool coinB = collision.gameObject.name.Contains("CoinB");

            if (coinB)
            {
                print(coin);
            }
        }

        if(collision.gameObject.tag == "Point")
        {
            print("터치다운!!");
        }
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 0);//Player Start Position
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

    public void InfoUIStart()
    {

    }


}
