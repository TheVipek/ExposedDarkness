using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeInitializer : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] volumeController[] volControllers;
    private void Start() {
        foreach (var volController in volControllers)
        {
            volController.AudioMixerSetValue(PlayerPrefs.GetFloat(volController.MixerValueToChange));
        }
    }
}
