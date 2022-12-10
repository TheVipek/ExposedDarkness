using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class completionMissionAction : MonoBehaviour,IAnyButton
{
    bool used = false;
    private void OnEnable() {
        GameManager.Instance.freezePlayerActions();
    }
    private void OnGUI() 
    {
        Event currentEvent = Event.current;
        if( (currentEvent.isKey || currentEvent.isMouse) && used == false)
        {
            Debug.Log("Mission complete clicked!");
            AnyButtonAction();
            used = true;
        }
    }
    private void OnDisable() {
        GameManager.Instance.freezePlayerActions();
    }
    public void AnyButtonAction()
    {
        SceneController.Instance.GoToScene("MainMenu");
    }
}
