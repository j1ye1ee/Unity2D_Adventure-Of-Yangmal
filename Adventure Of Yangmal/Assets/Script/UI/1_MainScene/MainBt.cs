using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class MainBt : MonoBehaviour
{
   
    public void NewGame()
    {
        SceneManager.LoadScene(0);
    }

    public void Continue()
    { 
        // �̾��ϱ�
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
