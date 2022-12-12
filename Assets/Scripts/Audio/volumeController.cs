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
    (float,float) volumeValues = (-80f,0.0f);
    public string MixerValueToChange {get{return mixerValueToChange;}}

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
        AudioMixerSetValue(slider.value);
    }
    public void AudioMixerSetValue(float value)
    {
        if(value == 0) audioMixer.SetFloat(mixerValueToChange,volumeValues.Item1);
        else audioMixer.SetFloat(mixerValueToChange,Mathf.Log10(value) * multiplier);
    }
    private void OnDisable() {
        PlayerPrefs.SetFloat(mixerValueToChange,slider.value);
    }
}
