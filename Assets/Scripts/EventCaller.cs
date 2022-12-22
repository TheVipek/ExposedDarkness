using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class EventCaller : MonoBehaviour
{
    private List<UnityEvent> eventsToRaise;
    public void RaiseEvents()
    {
        if(eventsToRaise != null)
        {
            foreach(UnityEvent _event in eventsToRaise) _event.Invoke();
        }
    }
}
