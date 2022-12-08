using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class interactionListener : MonoBehaviour
{
    [SerializeField] GameObject canInteract;
    [SerializeField] TMP_Text canInteractText;
    private const string defaultCanInteractText = "To interact";
    [SerializeField] GameObject cantInteract;
    [SerializeField] TMP_Text cantInteractText;
    private const string defaultCantInteractText = "Can't interact";
    [SerializeField] InputActionReference interactionAction;

    private GameObject lookingAt;
    private InteractionObject interactionObject;
    private void OnEnable() {
     //   interactionAction.action.started += OnInteraction;
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

    public void OnInteraction(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            if(interactionObject != null && interactionObject.canInteract == true)
            {
                interactionObject.interactionContainer.OnInteractionStart();
            //    interactionObject.canInteract = false;
                //   Interaction.Instance.setLastInteracted();
                Interaction.Instance.OnDeselect();
                //Interaction.Instance.interactionActivated = false;
            }
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
                    //    interactionObject.canInteract = false;
                     //   Interaction.Instance.setLastInteracted();
                        Interaction.Instance.OnDeselect();
                        //Interaction.Instance.interactionActivated = false;
                    }
                }
            }
        }
    }
    private void OnDisable() {
     //   interactionAction.action.started -= OnInteraction;
        if(canInteract.activeSelf == true)
        {
            canInteract.SetActive(false);
        }else
        {
            cantInteract.SetActive(false);
        }
    }       
}
