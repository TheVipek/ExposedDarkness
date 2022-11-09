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
    private void Start() {
        StartCoroutine(TriggerBlinking());
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
}
