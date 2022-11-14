using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waiter : MonoBehaviour
{
    public IEnumerator Wait(float waitTime)
    {
        WaitForSeconds _waitTime = new WaitForSeconds(waitTime);
        yield return _waitTime;
        
    }
    public static void callWaiter(float waitTime)
    {
        
    }
}
