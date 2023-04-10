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

// 회원가입
// 로그인 

public class ExampleManager : MonoBehaviour
{
    string URL = "https://script.google.com/macros/s/AKfycbwKrRJETzMP0av6NkuZM6I7SWwThQ8nsEFRS5OEYUphfQWSjQ6E6uX99Y-gIDYUcfnQ/exec";

    IEnumerator Start()
    {
        //** 요청을 하기위한 작엄
        MemberForm member = new MemberForm("변사또", 45);

        WWWForm form = new WWWForm();

        form.AddField("Name", member.Name);
        form.AddField("Age", member.Age);

        //MemberForm
        //string userName="변사또";
        //int age = 45;


        //UnityWebRequest request = UnityWebRequest.Get(URL);
        using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
        {

            yield return request.SendWebRequest();


            //** 응답에 대한 작업
            print(request.downloadHandler.text);
            //print(request.error);
        }
    }
}
