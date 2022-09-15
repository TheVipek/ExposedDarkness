using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class VingetteBumping : MonoBehaviour
{
    [SerializeField] PostProcessVolume volume;
    Vignette vingette;
    [SerializeField] float entryBump = 0f;
    [SerializeField] float maxBump = 0.25f;
    [SerializeField] float speed = 0.1f;

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
    public IEnumerator StartBloodBumping()
    {
       float complete = 0f;
       while(complete < 1)
       {
            vingette.intensity.value = Mathf.Lerp(entryBump,maxBump,complete);
            complete+=Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
       }
       yield return null;
    }
    public IEnumerator StopBloodBumping()
    {
       float complete = 0f;
       while(complete < 1)
       {
            vingette.intensity.value = Mathf.Lerp(maxBump,entryBump,complete);
            complete+=Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
       }
       yield return null;
    }
}
