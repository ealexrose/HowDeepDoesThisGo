using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{

    public string buttonActivationSound = "ButtonClick";

    public void PlayClick()
    {
        AudioManager.instance.Play(buttonActivationSound);
    }
}
