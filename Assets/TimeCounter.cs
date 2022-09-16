using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TimeCounter : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] float startingTime;
    [SerializeField] float remainingTime;
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
