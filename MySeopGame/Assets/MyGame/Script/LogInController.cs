using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;//특정 함수를 쓰기 위해 불러옴
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LogInController : MonoBehaviour
{
    string URL = "";

    public InputField emailIn; // 아이디 인풋 필드
    public InputField passwardIn;// 비번 인풋 필드
    private string emailPattern = @"^[\w-.]+@([\w-]+.)+[\w-]{2,4}$";//이메일 패턴이 맞는지 확인하는 코드

    public Text massage;// 현재 상태를 띄우기 위한 텍스트 변수

    void Start()
    {
        massage.text = ""; // 상태 메세지 초반에 아무 것ㄷ 없음
    }

    void LoginCheck()
    {
        string email = emailIn.text;

        //이메일 패턴에 맞는지?
        if (Regex.IsMatch(email, emailPattern))
        {
            // true
            string passward = Security(passwardIn.text);

            print(passward);

            // 로그인
            // "쌤 같으면 쓰레드(코루틴이랑 비슷하지만 다름) 만들어서 할 것 같다." 라고 함. 하지만 지금은 쓰지 않는 것을 권장.
            // Manager은 싱글톤(어디서도쓸 수 있는)으로 만들어야한다.

        }
        else
        {
            //false
            massage.text = "email형식을 다시 확인 하세요";
        }
    }

    string Security(string password)
    {
        // 비번 비어있다면?
        if (string.IsNullOrEmpty(password))
        {
            // true
            massage.text = "passward를 입력하세요";
            return "null";
        }
        // 비어있지 않다면?
        else
        {
            // ※암호화, 복호화 검색
            // false 패스워드도 들어온 상태
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();

            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }

            return stringBuilder.ToString();
        }
    }
}
