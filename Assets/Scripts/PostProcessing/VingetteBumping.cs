using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class VingetteBumping : MonoBehaviour
{
    [SerializeField] PostProcessVolume volume;
    [SerializeField] AudioSource audioSource;
    Vignette vingette;
    public float entryValue = 0.1f;
    public float maxBloodValue = 0.25f;
    public float maxPoisonValue = 0.5f;
    public Color bloodColor,poisonColor;
    [SerializeField] float duration =2f;
    [SerializeField] float colorSwapDuration = 0.5f;
    Coroutine bloodBumpingCR = null; 

    private void Awake() {
        volume.profile.TryGetSettings(out vingette);
    }
    private void OnEnable() {
        WaveController.onWaveStartGlobal += disableVingeteEffect;
        WaveController.onWaveEndGlobal += enableVingetteEffect;
    }
    private void OnDisable() {
        WaveController.onWaveStartGlobal -= disableVingeteEffect;
        WaveController.onWaveEndGlobal -= enableVingetteEffect;

        
    }
    public void disableVingeteEffect()
    {
        vingette.active = false;
    }
    public void enableVingetteEffect()
    {
        vingette.active = true;
    }
    public void StartBloodBumping()
    {
        bloodBumpingCR = StartCoroutine(BloodBumping(maxBloodValue,false));
        Debug.Log("Blood bumping started!");
    }
    public void StopBloodBumping()
    {
        Debug.Log($"bloodBumpingCR:{bloodBumpingCR}");
        if(bloodBumpingCR != null) StopCoroutine(bloodBumpingCR);
        StartCoroutine(BloodBumping(0,true));

    }
    public IEnumerator BloodBumping(float toVal,bool immadiatelyExit = false)
    {
       float complete = 0f;
       float currentVal = vingette.intensity.value; 
       if(!audioSource.isPlaying)
       {
            audioSource.Play();
       }
       while(complete < duration)
       {
            
            vingette.intensity.value = Mathf.Lerp(currentVal,toVal,complete/duration);
            complete+=Time.deltaTime;
            yield return null;
       }
       vingette.intensity.value = toVal;
       if(immadiatelyExit)
       {
            if(toVal == 0) yield break;
            else yield return StartCoroutine(BloodBumping(toVal:0,immadiatelyExit:immadiatelyExit));
       }
       else 
       {
            bloodBumpingCR = StartCoroutine(BloodBumping(toVal:currentVal));
       }
    }
    public IEnumerator smoothColorChange(Color toColor)
    {
        float complete = 0f;
        Color currentColor = vingette.color.value;
        while(complete < colorSwapDuration)
        {
            vingette.color.value = Color.Lerp(currentColor,toColor,complete/colorSwapDuration);
            complete += Time.deltaTime;
            yield return null;
        }
        vingette.color.value = toColor;

    }
    public IEnumerator TriggerPoison(float toVal)
    {
       float complete = 0f;
       float currentVal = vingette.intensity.value;
       while(complete < duration)
       {
            
            vingette.intensity.value = Mathf.Lerp(currentVal,toVal,complete/duration);
            complete+=Time.deltaTime;
            yield return null;
       }
       vingette.intensity.value = toVal;
    }
    public void TriggerPoisoning()
    {
        StopAllCoroutines();
        StartCoroutine(smoothColorChange(poisonColor));
        StartCoroutine(TriggerPoison(maxPoisonValue));
    }

}


