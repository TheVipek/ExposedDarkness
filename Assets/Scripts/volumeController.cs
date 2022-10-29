using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class volumeController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string mixerValueToChange;
    [SerializeField] float multiplier = 30f;

    void Awake()
    {
        slider.onValueChanged.AddListener(delegate{
            sliderValueHandler(slider);
        });
    }
    void Start() {
        slider.value = PlayerPrefs.GetFloat(mixerValueToChange);
    }
    void sliderValueHandler(Slider slider)
    {
        audioMixer.SetFloat(mixerValueToChange,Mathf.Log10(slider.value) * multiplier);
    }
    private void OnDisable() {
        PlayerPrefs.SetFloat(mixerValueToChange,slider.value);
    }
}
