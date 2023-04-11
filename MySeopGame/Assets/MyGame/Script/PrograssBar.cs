using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrograssBar : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public Text text;
    public Text massage;

    IEnumerator Start()
    {
        EditorApplication.isPaused = true;
        asyncOperation = SceneManager.LoadSceneAsync("GameStart");
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {

            yield return null;



            if (asyncOperation.progress > 0.7f)
            {
                yield return new WaitForSeconds(2.5f); 
            }
            else
            {
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                text.text = (progress * 100f).ToString();
                Debug.Log((progress * 100f).ToString());

            }
        }

    }


}
