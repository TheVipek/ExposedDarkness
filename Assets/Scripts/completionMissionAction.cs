using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class completionMissionAction : MonoBehaviour,IAnyButton
{
    bool used = false;
     private void OnGUI() {
        Event currentEvent = Event.current;
        if(currentEvent.isKey || currentEvent.isMouse && used == false)
        {
            AnyButtonAction();
            used = true;
        }
    }
    public void AnyButtonAction()
    {
        SceneController.Instance.GoToScene("MainMenu");
    }
}
