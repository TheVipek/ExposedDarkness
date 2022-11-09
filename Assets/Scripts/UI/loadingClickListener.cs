using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loadingClickListener : MonoBehaviour,IAnyButton
{
    bool used = false;
    private void OnGUI() {
        Event currentEvent = Event.current;
        if(currentEvent.isKey && used == false)
        {
            AnyButtonAction();
            used = true;
        }
    }
    public void AnyButtonAction()
    {
        StartCoroutine(SceneController.Instance.loadingPreparedAction());
    }
}
