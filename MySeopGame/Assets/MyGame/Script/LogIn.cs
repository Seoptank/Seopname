using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LogIn : MonoBehaviour
{
    void OnClickLoginButton()
    {
        WWWForm form = new WWWForm();
        form.AddField("order", "login");
        form.AddField("id", "TestId");
        form.AddField("password", "1111122");

        StartCoroutine(Post(form));
    }

    private IEnumerator Post(WWWForm form)
    {
        string URL = "";

        using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
        {
            yield return www.SendWebRequest();

            if (www.isDone)
                print(www.downloadHandler.text);
            else
                print("Error");
        }

    }
}
