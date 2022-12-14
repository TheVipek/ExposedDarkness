using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameEventListener : MonoBehaviour
{
    public GameEvent eventListenTo;
    public UnityEvent response;

    private void OnEnable() {
        eventListenTo.RegisterListener(this);
    }
    private void OnDisable() {
        eventListenTo.UnregisterListener(this);
    }
    public void OnEventRaised()
    {
        response.Invoke();
    }
}
