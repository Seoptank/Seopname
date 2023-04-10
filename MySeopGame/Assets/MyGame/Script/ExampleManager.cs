using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MemberForm
{
    public string Name;
    public int Age;

    public MemberForm(string name, int age)
    {
        this.Name = name;
        this.Age = age;
    }
}

// ȸ������
// �α��� 

public class ExampleManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbwKrRJETzMP0av6NkuZM6I7SWwThQ8nsEFRS5OEYUphfQWSjQ6E6uX99Y-gIDYUcfnQ/exec";

    IEnumerator Start()
    {
        //** ��û�� �ϱ����� �۾�
        MemberForm member = new MemberForm("�����", 45);

        WWWForm form = new WWWForm();

        form.AddField("Name", member.Name);
        form.AddField("Age", member.Age);

        //MemberForm
        //string userName="�����";
        //int age = 45;


        //UnityWebRequest request = UnityWebRequest.Get(URL);
        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {

            yield return request.SendWebRequest();


            //** ���信 ���� �۾�
            print(request.downloadHandler.text);
            //print(request.error);
        }
    }
}
