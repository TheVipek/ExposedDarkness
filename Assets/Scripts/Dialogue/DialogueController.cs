using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;


public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance{get;private set;}
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject dialogueEndPossibility;
    [SerializeField] Animator dialogueAnimator;
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField] float timePerCharacter;
    [SerializeField] float timePerSentence;

    public Dialogue currentDialogue;
    public static event Action OnGlobalDialogueStarted;
    public static event Action OnGlobalDialogueEnded;
    private void Awake() {

        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }
    
    public void DialogueStartPhase(Dialogue _dialogue, bool transitionInside = true)
    {
        currentDialogue = _dialogue;
        OnGlobalDialogueStarted();
        currentDialogue.callStartEvents();
        if(transitionInside == true) dialogueAnimator.SetTrigger("appear");
        dialoguePanel.SetActive(true);
        StartCoroutine(textTransition(currentDialogue));
    }
    public void DialogueShowEnd()
    {
        if(currentDialogue.forceExitDialogue == true)
        {  
            DialogueEndPhase();
        }else
        {
            dialogueEndPossibility.SetActive(true);
        }
    }
    public void DialogueEndPhase()
    {
        dialogueAnimator.SetTrigger("disappear");
        OnGlobalDialogueEnded();
        currentDialogue.callEndEvents();
    }
    IEnumerator textTransition(Dialogue _dialogue)
    {
        string[] text = _dialogue.dialogueText.textToDisplayAtOnce;
        for(int i=0;i<text.Length;i++)
        {
          //  Debug.Log(text[i].Length);
            int currentIndex =0;
            bool tagStarted = false;
            string tagStorage = string.Empty;
            tMP_Text.text = string.Empty;
            while(currentIndex<text[i].Length)
            {
                currentDialogue.checkForSentenceEvent(i);
                if(text[i][currentIndex] == '<')
                {
                    tagStarted = true;
                }

                if(tagStarted == true)
                {
                    if(text[i][currentIndex] == '>')
                    {
                        tagStarted = false;
                    }
                    tagStorage += text[i][currentIndex];
                }

                if(tagStarted == false)
                {
                    if(tagStorage != string.Empty)
                    {
                        tMP_Text.text += tagStorage;
                        tagStorage = string.Empty;
                    }
                    else
                    {
                        tMP_Text.text += text[i][currentIndex];
                    }
                }

                currentIndex+=1;
                yield return new WaitForSeconds(timePerCharacter);
            }
            yield return new WaitForSeconds(timePerSentence);
        }
        DialogueShowEnd();
        yield return null;
    }
}
