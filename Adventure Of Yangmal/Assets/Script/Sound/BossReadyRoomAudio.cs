using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossReadyRoomAudio : MonoBehaviour
{
    public AudioSource _buttonSound;

    public void SeclectButton()
    {
        _buttonSound.PlayOneShot(_buttonSound.clip);
    }

}
