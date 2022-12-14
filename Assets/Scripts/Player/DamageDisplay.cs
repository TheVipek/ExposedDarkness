using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DamageDisplay : MonoBehaviour
{
    [SerializeField] Canvas impactCanvas;
    [SerializeField] Image bloodImage;
    private float currentAlpha;
    [SerializeField] float impactTime = 2f;
    private float elapsedTime;
    Coroutine showingBloodCoroutine; 
    
    private void OnEnable() {
        PlayerHealth.onDamageTaken += DamageEffects;
    }
    private void OnDisable() {
        PlayerHealth.onDamageTaken -= DamageEffects;

    }
    void Start() {

        impactCanvas.enabled = false;    
    }
    void DamageEffects()
    {
        if(showingBloodCoroutine != null)
        {
            StopCoroutine(showingBloodCoroutine);
            showingBloodCoroutine = StartCoroutine(Bleeding(0,1));
            
        }
        else
        {
            showingBloodCoroutine = StartCoroutine(Bleeding(0,1));
        }
        
    }
    IEnumerator Bleeding(float fromValue,float toValue)
    {
        impactCanvas.enabled = true;
        currentAlpha = bloodImage.color.a;

        if(currentAlpha == toValue)
        {
            yield return new WaitForSeconds(impactTime);
        }
        elapsedTime = 0;
        while(elapsedTime < impactTime)
        {
            currentAlpha = Mathf.Lerp(currentAlpha,toValue,elapsedTime/impactTime);
            bloodImage.color = new Color(bloodImage.color.r,bloodImage.color.g,bloodImage.color.b,currentAlpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        currentAlpha = toValue;
        bloodImage.color = new Color(bloodImage.color.r,bloodImage.color.g,bloodImage.color.b,currentAlpha);
        yield return showingBloodCoroutine = StartCoroutine(Bleeding(1,0));
        
        
    }
    
}
