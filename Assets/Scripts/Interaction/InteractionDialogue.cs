using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[CreateAssetMenu(fileName = "InteractionDialogue", menuName = "Interaction/Dialogue", order = 0)]
public class InteractionDialogue : InteractionContainer {
    public DialogueText textToAppear;

    public override void OnInteractionStart()
    {
        DialogueController.Instance.DialogueStartPhase(textToAppear,true);
    }
}

