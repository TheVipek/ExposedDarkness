using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeCounter : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] float startingTime;
    [SerializeField] float remainingTime;
    bool startedPoisoning = false;
    public float RemainingTime{get{return remainingTime;}}
    public static TimeCounter instance;
    private void Awake() {
        instance = this;
    }
    private void OnEnable() {
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        remainingTime = startingTime;
        while(remainingTime>0)
        {
            remainingTime-=1;
            displayTimer();
            if(remainingTime <= 60 && startedPoisoning == false)
            {
                VingetteBumping.instance.StopAllCoroutines();
                PlayerHealth.instance.poisonTriggered = true;
                StartCoroutine(VingetteBumping.instance.smoothColorChange(VingetteBumping.instance.poisonColor));
                StartCoroutine(VingetteBumping.instance.TriggerPoison(VingetteBumping.instance.maxPoisonValue));
                ChromaticAberrationTrigger.instance.enabled = true;
            }
            yield return new WaitForSeconds(1);
        }
        DeathHandler.instance.OutOfTime();
    }
    void displayTimer()
    {
        float minutes = Mathf.FloorToInt(remainingTime / 60);
        float seconds = Mathf.FloorToInt(remainingTime%60);
        if(seconds<10)
        {
            timerText.GetComponent<TMP_Text>().text = minutes+":0"+seconds;

        }
        else
        {
            timerText.GetComponent<TMP_Text>().text = minutes+":"+seconds;
        }
    }
}
