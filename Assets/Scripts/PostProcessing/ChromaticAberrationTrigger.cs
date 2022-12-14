using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class ChromaticAberrationTrigger : MonoBehaviour
{
    [SerializeField] PostProcessVolume volume;
    ChromaticAberration chromaticAberration;
    public float maxVal = 0.2f;
    public float minVal = 0;
    [SerializeField] float currentVal;
    [SerializeField] float duration = 5f;
     private void Awake() {
        volume.profile.TryGetSettings(out chromaticAberration);
    }
    private void OnEnable() {
        StartCoroutine(chromaticAberrationBump(minVal,maxVal));
    }
    IEnumerator chromaticAberrationBump(float fromVal,float toVal)
    {
        float complete = 0f;
        while(complete < duration)
        {
            currentVal = Mathf.Lerp(fromVal, toVal, complete/duration);
            chromaticAberration.intensity.value = currentVal;
            complete += Time.deltaTime;
            yield return null;
        }
        currentVal = toVal;
        chromaticAberration.intensity.value = currentVal;
    }
}
