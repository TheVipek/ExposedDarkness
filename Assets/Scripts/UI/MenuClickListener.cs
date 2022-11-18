using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MenuClickListener : MonoBehaviour,IAnyButton
{
    [SerializeField] Canvas menuCanvas;
    bool used = false;
    private void OnGUI() {
        if(Event.current.isKey)
        {
            
            Event currentEvent = Event.current;
            if(currentEvent.keyCode == KeyCode.Escape)
            {
                if (currentEvent.type == EventType.KeyDown && used == false)
                {
                    AnyButtonAction();
                    used = true;
                }
                else if(currentEvent.type == EventType.KeyUp && used == true)
                {
                    used = false;
                }
            }
        }
    }
    public void AnyButtonAction()
    {
        menuCanvas.enabled = !menuCanvas.enabled;   
        if(menuCanvas.enabled == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            setCursor(true);
            GameManager.Instance.freezeTime();
        }else
        {
            setCursor(false);
            GameManager.Instance.unfreezeTime();
        }
    }
    public void setCursor(bool _visible)
    {
        Cursor.visible = _visible;
    }
}
