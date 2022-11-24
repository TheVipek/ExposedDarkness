using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(dialogueConversation))]
public class dialogueTrigger : MonoBehaviour
{
    [SerializeField] dialogueConversation conversation;
    [SerializeField] bool transitionInside;

    private void Start() {
        StartCoroutine(DialogueController.Instance.DialogueStartPhase(conversation.dialogue,transitionInside));
    }
}
