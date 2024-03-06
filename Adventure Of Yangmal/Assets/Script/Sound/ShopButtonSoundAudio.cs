using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopButtonSoundAudio : MonoBehaviour
{
    public AudioSource _buttonSound;
    public AudioSource _buySound;

    public void SelectButton()
    {
        _buttonSound.PlayOneShot(_buttonSound.clip);
    }

    public void Buy()
    {
        _buttonSound.PlayOneShot(_buySound.clip);
    }
}
