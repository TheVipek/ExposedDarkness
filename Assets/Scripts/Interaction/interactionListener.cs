using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class interactionListener : MonoBehaviour
{
    [SerializeField] GameObject canInteract;
    [SerializeField] TMP_Text canInteractText;
    private const string defaultCanInteractText = "To interact";
    [SerializeField] GameObject cantInteract;
    [SerializeField] TMP_Text cantInteractText;

    private const string defaultCantInteractText = "Can't interact";


    private GameObject lookingAt;
    private InteractionObject interactionObject;
    private void OnEnable() {
        lookingAt = Interaction.Instance.lookingAt;
        interactionObject = lookingAt.GetComponent<InteractionObject>();
        if(interactionObject.canInteract == true)
        {
            canInteract.SetActive(true);
            if(interactionObject.canInteractDescription != string.Empty) 
                    canInteractText.text = interactionObject.canInteractDescription;
        }else
        {
            cantInteract.SetActive(true);
            if(interactionObject.cantInteractDescription != string.Empty) 
                    cantInteractText.text = interactionObject.cantInteractDescription;
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
                        Interaction.Instance.setLastInteracted();
                        Interaction.Instance.OnDeselect();
                        //Interaction.Instance.interactionActivated = false;
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
