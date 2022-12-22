using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLamp : MonoBehaviour
{
    [SerializeField] List<Light> lights;
    [Tooltip("Random time blink occurs beetwen 0 to selected value")]
    [Range(0,10)] 
    [SerializeField] float blinkingDelayRange;
    [SerializeField] int timeToOverheat;
    //[SerializeField] AudioClip buzzingSound;
    [SerializeField] AudioSource audioSource;
    private float powerOutTime = 1f;
    private void Start() {
        //StartCoroutine(TriggerBlinking());
    }
    public IEnumerator TriggerBlinking()
    {
        while(true)
        {
            float randomTime = Random.Range(0,blinkingDelayRange);
            foreach (var item in lights)
            {
                
                item.enabled = false;
                audioSource.Stop();
                
            }
            yield return new WaitForSeconds(0.2f);
            //randomTime = Random.Range(0,blinkingDelayRange);
            foreach (var item in lights)
            {
                
                item.enabled = true;
                audioSource.Play();
                
            }
            yield return new WaitForSeconds(randomTime);
            
        }
    }
    public void PowerOut()
    {
        StartCoroutine(StartPowerOut());
    }
    private IEnumerator StartPowerOut()
    {
        float currentPowerOut = powerOutTime;
        List<float> startingIntensities = new List<float>();
        lights.ForEach( x => startingIntensities.Add(x.intensity));
        while(true)
        {
            currentPowerOut -= Time.deltaTime;
            for(int i=0 ; i<lights.Count ; i++)
            {
                lights[i].intensity = startingIntensities[i] * (currentPowerOut / powerOutTime);
            }
            audioSource.volume = audioSource.volume * (currentPowerOut / powerOutTime);
            if(currentPowerOut <= 0)
            {

                yield break;
            }
            yield return null;
        }
    }
}
