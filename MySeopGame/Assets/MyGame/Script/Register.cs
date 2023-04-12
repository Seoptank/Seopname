using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Register : MonoBehaviour
{
    public void OnClickRegisterButton()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("id", "TestId");
        form.AddField("password", "1111122");

        StartCoroutine(Post(form));
    }
    

    private IEnumerator Post(WWWForm form)
    {
        string URL = "https://script.google.com/macros/s/AKfycbyYlTXgWjUi_cMiuBiInv8BP8QddsCmdC-tkV9SOGh03MhO__drDGr_qIp5yb3sV_mDKQ/exec";

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
            {
                print(www.downloadHandler.text);
            }
            else
                print("Error");
        }
    }
}
