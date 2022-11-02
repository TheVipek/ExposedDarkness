using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionListener : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnGUI() {
        if(Event.current.isKey)
        {
            
            Event currentEvent = Event.current;
            if(currentEvent.keyCode == KeyCode.E)
            {
                if (currentEvent.type == EventType.KeyDown)
                {
                    GameObject lookingAt = Interaction.Instance.lookingAt;
                    InteractionObject interactionObject = lookingAt.GetComponent<InteractionObject>();
                    if(interactionObject != null && interactionObject.canInteract == true) interactionObject.interactionContainer.OnInteractionStart();
                }
            }
        }
    }       
}
