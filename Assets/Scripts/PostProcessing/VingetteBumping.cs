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
    public float currentBump;
    public Color bloodColor,poisonColor;
    //[SerializeField] float speed = 0.1f;
    [SerializeField] float duration =2f;
    [SerializeField] float colorSwapDuration = 0.5f;
    int deathPumps = 1;
    public Color currentColor;
    public static VingetteBumping instance;

    private void Awake() {
        if(instance!=this && instance!=null)
        {
            Destroy(this);
        }else
        {
            instance = this;
            volume.profile.TryGetSettings(out vingette);
        }
       
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
        Debug.Log(vingette.active);
        Debug.Log("Disabling vingette...");
        vingette.active = false;
        Debug.Log(vingette.active);

    }
    public void enableVingetteEffect()
    {
        vingette.active = true;
    }
    public IEnumerator BloodBumping(float toVal,float fromVal = 0)
    {
       float complete = 0f;

       if(!audioSource.isPlaying)
       {
            audioSource.Play();
       }
       while(complete < duration)
       {
            
            currentBump = Mathf.Lerp(currentBump,toVal,complete/duration);
            vingette.intensity.value = currentBump;
            complete+=Time.deltaTime;
            
            yield return null;
       }
       currentBump = toVal;
       vingette.intensity.value = currentBump;
       if(PlayerHealth.Instance.IsDead == false)
       {
            if(PlayerHealth.Instance.bloodOverFace == false && currentBump != 0)
            {
                    yield return StartCoroutine(BloodBumping(fromVal:toVal,toVal:0));
            }
            else if(PlayerHealth.Instance.bloodOverFace == false && currentBump == 0)
            {
                    yield break;
            }else
            {
                    yield return StartCoroutine(BloodBumping(fromVal:toVal,toVal:fromVal));
            }
       }
       else
       {
            if(deathPumps>=0)
            {
                deathPumps -=1;
                yield return StartCoroutine(BloodBumping(fromVal:toVal,toVal:fromVal));
            }
            else
            {
                Debug.Log("Player is dead - no heart beat");
                yield return null;
            }
       }

       
    }
    public IEnumerator smoothColorChange(Color toColor)
    {
        float complete = 0f;
        while(complete < colorSwapDuration)
        {
            currentColor = Color.Lerp(currentColor,toColor,complete/colorSwapDuration);
            vingette.color.value = currentColor;
            complete += Time.deltaTime;
            yield return null;
        }
        currentColor = toColor;
        vingette.color.value = currentColor;

    }
    public IEnumerator TriggerPoison(float toVal)
    {
       float complete = 0f;
       while(complete < duration)
       {
            
            currentBump = Mathf.Lerp(currentBump,toVal,complete/duration);
            vingette.intensity.value = currentBump;
            complete+=Time.deltaTime;
            yield return null;
       }
       currentBump = maxPoisonValue;
       vingette.intensity.value = currentBump;
    }
}
