using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class InteractionDialogue : InteractionContainer {
    [SerializeField] dialogueConversation dialogueConversation;
    public override void OnInteractionStart()
    {
        
        DialogueController.Instance.DialogueStartPhase(dialogueConversation.dialogue,true);
    }
}

