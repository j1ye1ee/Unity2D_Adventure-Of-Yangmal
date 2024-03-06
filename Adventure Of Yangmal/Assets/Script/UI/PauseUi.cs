using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUi : MonoBehaviour
{
    public AudioSource _button;
    public Button _firstBt;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
           for(int i =0; i< this.gameObject.transform.childCount; i++)
            {
                this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            _firstBt.Select();
            Time.timeScale = 0;
        }
    }


    public void ReturnButton()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ButtonSound()
    {
        _button.PlayOneShot(_button.clip);
    }
}
