using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioHolder : MonoBehaviour
{
    List<AudioSource> audioSources;
    public void muteAll(bool isMuted)
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            audioSources[i].mute = isMuted;
        }
    }
}
