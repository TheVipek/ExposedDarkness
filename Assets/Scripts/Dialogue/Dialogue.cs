using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[Serializable]
public class Dialogue
{
    public DialogueText dialogueText;
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
    public sentEvents[] sentenceEvents;

    public void checkForSentenceEvent(int index)
    {
        for(int i=0;i<sentenceEvents.Length;i++)
        {
            if(sentenceEvents[i].sentenceIndex == index && sentenceEvents[i].sentenceEvent != null)
            {
                Debug.Log(sentenceEvents[i].sentenceIndex +" = "+index +" and has: "+sentenceEvents[i].sentenceEvent);
                callEvent(sentenceEvents[i].sentenceEvent);
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
