using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//** 점수, 스테이지 관리
public class GameController : MonoBehaviour
{
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
    public Text clipUI;
    public GameObject endUI;

    //정보창UI
    [Header("정보창UI")]
    public Text infoStateUI;
    [SerializeField]
    public Text infoStagePointUI;
    public Text infoCoinUI;

    public GameObject retryBtn;
    public GameObject nextBtn;
    public GameObject mainBtn;

    private void Start()
    {
        endUI.SetActive(false);
    }

    void Update()
    {
        clipUI.text = "x" + player.bulletClip;

        pointUI.text = point.ToString();

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

        //스테이지 변환시 


    }

    //public void endbutton()
    //{
    //    //change stage
        

//    else
//    {
//        //game clear

//        //player control lock
//        Time.timeScale = 0;

//        //restart button ui


//    }

//    //calaulartion point
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
            //ViewBtn();
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
    }

    void PlayerReposition()
    {
        player.transform.position = new Vector3(0, 0, 0);//Player Start Position
        player.VelocityZero();
    }
    
    
    //**버튼
    //----------------------------------------------------------------------------
    public void RetryButton()
    {
        //스테이지 원위치로 이동
        Time.timeScale = 1;
        SceneManager.LoadScene(0);

        //포인트 초기화
        point = 0;
        endUI.SetActive(false);

    }
    public void NextButton()
    {
        //다음 스테이지 원위치로 이동
        if (stageIndex < stage.Length - 1)
        {
            stage[stageIndex].SetActive(false);
            stageIndex++;
            stage[stageIndex].SetActive(true);
            PlayerReposition();

            stageUI.text = "stage " + (stageIndex + 1);
            point = 0;
            hp = maxHp;
            coin = 0;
            player.bulletClip = 10;
            endUI.SetActive(false);
        }


    }
    public void MainButton()
    {
        //메인으로 이동
        SceneManager.LoadScene("GameStart");
    }
    //------------------------------------------------------------------------------

    //InfoUI띄우기
    //------------------------------------------------------------------------------
    public void LoseUIStart()
    {
        //UI띄우기
        endUI.gameObject.SetActive(true);

        //정보UI
        infoStateUI.text = "Failuer";
        infoStagePointUI.text = "Stage Point : " + point.ToString();
        infoCoinUI.text = "Stage Coin : " + coin.ToString();
        retryBtn.SetActive(true);
    }

    //승리시 InfoUI띄우는 함수
    public void VictoryUIStart()
    {
        //UI띄우기
        endUI.gameObject.SetActive(true);

        //정보UI
        infoStateUI.text = "Success";
        infoStagePointUI.text = "Stage Point : " + point.ToString();
        infoCoinUI.text = "Stage Coin : " + coin.ToString();
        nextBtn.SetActive(true);
    }
    //------------------------------------------------------------------------------

   




}
