using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;//Ư�� �Լ��� ���� ���� �ҷ���
using System.Text;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class LogInController : MonoBehaviour
{
    string URL = "";

    public InputField emailIn; // ���̵� ��ǲ �ʵ�
    public InputField passwardIn;// ��� ��ǲ �ʵ�
    private string emailPattern = @"^[\w-.]+@([\w-]+.)+[\w-]{2,4}$";//�̸��� ������ �´��� Ȯ���ϴ� �ڵ�

    public Text massage;// ���� ���¸� ���� ���� �ؽ�Ʈ ����

    void Start()
    {
        massage.text = ""; // ���� �޼��� �ʹݿ� �ƹ� �ͤ� ����
    }

    void LoginCheck()
    {
        string email = emailIn.text;

        //�̸��� ���Ͽ� �´���?
        if (Regex.IsMatch(email, emailPattern))
        {
            // true
            string passward = Security(passwardIn.text);

            print(passward);

            // �α���
            // "�� ������ ������(�ڷ�ƾ�̶� ��������� �ٸ�) ���� �� �� ����." ��� ��. ������ ������ ���� �ʴ� ���� ����.
            // Manager�� �̱���(��𼭵��� �� �ִ�)���� �������Ѵ�.

        }
        else
        {
            //false
            massage.text = "email������ �ٽ� Ȯ�� �ϼ���";
        }
    }

    string Security(string password)
    {
        // ��� ����ִٸ�?
        if (string.IsNullOrEmpty(password))
        {
            // true
            massage.text = "passward�� �Է��ϼ���";
            return "null";
        }
        // ������� �ʴٸ�?
        else
        {
            // �ؾ�ȣȭ, ��ȣȭ �˻�
            // false �н����嵵 ���� ����
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
