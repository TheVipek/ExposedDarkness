using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    Canvas impactCanvas;
    [SerializeField] float impactTime = 2f;
    Coroutine showingBloodCoroutine; 
    
    private void OnEnable() {
        PlayerHealth.onDamageTaken += DamageEffects;
    }
    private void OnDisable() {
        PlayerHealth.onDamageTaken -= DamageEffects;

    }
    private void Awake() {
        impactCanvas = GetComponent<Canvas>();
    }
    void Start() {

        impactCanvas.enabled = false;    
    }
    void DamageEffects()
    {
        if(showingBloodCoroutine != null)
        {
            StopCoroutine(showingBloodCoroutine);
            showingBloodCoroutine = StartCoroutine(startBleeding());
            
        }
        else
        {
            showingBloodCoroutine = StartCoroutine(startBleeding());
        }
        
    }
    IEnumerator startBleeding()
    {
        impactCanvas.enabled = true;
        yield return new WaitForSeconds(impactTime);
        impactCanvas.enabled = false;
    }
}
