using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLamps : MonoBehaviour
{
    [SerializeField] List<GameObject> lamps;
    [Tooltip("Random time blink occurs beetwen 0 to selected value")]
    [Range(0,2)] 
    [SerializeField] float blinkingDelayRange;
    [SerializeField] int timeToOverheat;
    void Start()
    {
        StartCoroutine(TriggerBlinking());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator TriggerBlinking()
    {
        while(timeToOverheat > 0)
        {
            float randomTime = Random.Range(0,blinkingDelayRange);
            foreach (var item in lamps)
            {
                
                Light[] _light = item.GetComponentsInChildren<Light>();
                foreach (var _item in _light)
                {
                    _item.enabled = false;
                }
                
            }
            yield return new WaitForSeconds(randomTime);
            randomTime = Random.Range(0,blinkingDelayRange);
            foreach (var item in lamps)
            {
                 Light[] _light = item.GetComponentsInChildren<Light>();
                foreach (var _item in _light)
                {
                    _item.enabled = true;
                }
            }
            yield return new WaitForSeconds(randomTime);
            timeToOverheat-=1;
        }
    }
}
