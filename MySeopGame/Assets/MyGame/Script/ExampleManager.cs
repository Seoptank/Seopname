using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

[System.Serializable]
public class MemberForm
{
    public int index;
    public string name;
    public int age;
    public int gender;

   
    public MemberForm(int index, string name, int age, int gender)
    {
        this.index = index;
        this.name = name;
        this.age = age;
        this.gender = gender;
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
        //MemberForm member = new MemberForm();

        //WWWForm form = new WWWForm();

        //form.AddField("Name", member.name);
        //form.AddField("Age", member.age);

        //MemberForm
        //string userName="�����";
        //int age = 45;


        //UnityWebRequest request = UnityWebRequest.Get(URL);
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {

            yield return request.SendWebRequest();

            MemberForm json = JsonUtility.FromJson<MemberForm>(request.downloadHandler.text);

            //** ���信 ���� �۾�
            print(json.index);
            print(json.name);
            print(json.age);
            print(json.gender);
            //print(request.error);
        }
    }

    public void NextSene()
    {
        SceneManager.LoadScene("progressScene");
    }

}
