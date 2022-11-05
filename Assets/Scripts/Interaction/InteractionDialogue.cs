using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;


public class InteractionDialogue : InteractionContainer {
    [SerializeField] dialogueConversation dialogueConversation;
    [SerializeField] bool forceDialogue ;
    public override void OnInteractionStart()
    {
        
        StartCoroutine(DialogueController.Instance.DialogueStartPhase(dialogueConversation.dialogue,forceDialogue));
    }
}

