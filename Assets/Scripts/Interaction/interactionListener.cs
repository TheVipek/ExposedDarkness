using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class interactionListener : MonoBehaviour
{
    [SerializeField] GameObject canInteract;
    [SerializeField] GameObject cantInteract;

    private GameObject lookingAt;
    private InteractionObject interactionObject;
    private void OnEnable() {
        lookingAt = Interaction.Instance.lookingAt;
        interactionObject = lookingAt.GetComponent<InteractionObject>();
        if(interactionObject.canInteract == true)
        {
            canInteract.SetActive(true);
        }else
        {
            cantInteract.SetActive(true);
        }
    }
    private void OnGUI() {
        if(Event.current.isKey)
        {
            
            Event currentEvent = Event.current;
            if(currentEvent.keyCode == KeyCode.E)
            {
                if (currentEvent.type == EventType.KeyDown)
                {
                    if(interactionObject != null && interactionObject.canInteract == true)
                    {
                        interactionObject.interactionContainer.OnInteractionStart();
                        Interaction.Instance.OnDeselect();
                    }
                }
            }
        }
    }
    private void OnDisable() {
        if(canInteract.activeSelf == true)
        {
            canInteract.SetActive(false);
        }else
        {
            cantInteract.SetActive(false);
        }
    }       
}
