using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[Serializable]
public class Dialogue
{
    public DialogueText dialogueText;
    public bool forceExitDialogue;
    [NonReorderable]
    public List <UnityEvent> OnLocalDialogueStarted;
    [NonReorderable]
    public List <UnityEvent> OnLocalDialogueEnded;
    [Serializable]
    public struct sentEvents
    {
        public int sentenceIndex;
        public UnityEvent sentenceEvent; 
    }
    [NonReorderable]
    public sentEvents[] OnSentence;

    public void checkForSentenceEvent(int index)
    {
        for(int i=0;i<OnSentence.Length;i++)
        {
            if(OnSentence[i].sentenceIndex == index && OnSentence[i].sentenceEvent != null)
            {
                Debug.Log(OnSentence[i].sentenceIndex +" = "+index +" and has: "+OnSentence[i].sentenceEvent);
                callEvent(OnSentence[i].sentenceEvent);
                break;
            }
        }
    }
    public Dialogue(DialogueText _dialogueText)
    {
        dialogueText = _dialogueText;
    }
    public void callEvent(UnityEvent _unityEvent)
    {
        _unityEvent.Invoke();
    }
    public void callStartEvents()
    {
        if(OnLocalDialogueStarted.Count == 0) return;
        for(int i=0;i<OnLocalDialogueStarted.Count;i++)
        {
            callEvent(OnLocalDialogueStarted[i]);
        }
    }
    public void callEndEvents()
    {
        if(OnLocalDialogueEnded.Count == 0) return;
        for(int i=0;i<OnLocalDialogueEnded.Count;i++)
        {
            callEvent(OnLocalDialogueEnded[i]);
        }
    }

}
