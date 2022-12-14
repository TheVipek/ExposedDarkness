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
    public GameEvent onInteracted;
    private void OnEnable() {
        interactionAction.action.started += OnInteraction;

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
                onInteracted.Raise();
            }
        }
    }
    private void OnDisable()
    {
        interactionAction.action.started -= OnInteraction;

        if(canInteract.activeSelf == true)
        {
            canInteract.SetActive(false);
        }
        else
        {
            cantInteract.SetActive(false);
        }
    }
    public void SetLookingAt(GameObject _lookingAt) => lookingAt = _lookingAt;
}
