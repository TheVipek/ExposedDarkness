using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class loadingText : MonoBehaviour
{
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField] float forwardSpeed = 0.4f;
    [SerializeField] float backwardSpeed = 0.15f;
    int dots = 3;
    void Start()
    {
        StartCoroutine(smoothText());
    }

    IEnumerator smoothText()
    {
        WaitForSeconds wForward = new WaitForSeconds(forwardSpeed);
        WaitForSeconds wBackward = new WaitForSeconds(backwardSpeed);
        
        while(true)
        {
            while(dots > 0)
            {
                tMP_Text.text+=".";
                dots -= 1;
                yield return wForward;
            }
            while(dots < 3)
            {
                tMP_Text.text = tMP_Text.text.Remove(tMP_Text.text.Length -1); 
                dots += 1;
                yield return wBackward;
            }
            yield return null;
        }
        
    }
}
