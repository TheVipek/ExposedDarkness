using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicTransition : MonoBehaviour
{


    private void Start() {   
        triggerTransition(false);
    }
    public void triggerTransition(bool mute)
    {
        AudioManager.Instance.musicSoundTransition(GetComponent<AudioSource>(),mute:mute);

    }
}
