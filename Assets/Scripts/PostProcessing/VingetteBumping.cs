using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class VingetteBumping : MonoBehaviour
{
    [SerializeField] PostProcessVolume volume;
    Vignette vingette;
    public float entryValue = 0f;
    public float maxBloodValue = 0.25f;
    public float maxPoisonValue = 0.5f;
    public float currentBump;
    public Color bloodColor,poisonColor;
    //[SerializeField] float speed = 0.1f;
    [SerializeField] float duration =2f;
    [SerializeField] float colorSwapDuration = 0.5f;

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
    public IEnumerator BloodBumping(float toVal,float fromVal = 0)
    {
       float complete = 0f;
       while(complete < duration)
       {
            
            currentBump = Mathf.Lerp(currentBump,toVal,complete/duration);
            vingette.intensity.value = currentBump;
            complete+=Time.deltaTime;
            
            yield return null;
       }
       currentBump = toVal;
       vingette.intensity.value = currentBump;

        if(PlayerHealth.instance.bloodOverFace == false && currentBump == entryValue)
        {
            yield break;
        }
        yield return StartCoroutine(BloodBumping(fromVal:toVal,toVal:fromVal));

       
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
