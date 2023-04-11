using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CangeScene : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Loading()
    {
        SceneManager.LoadScene("ProgressBar");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
