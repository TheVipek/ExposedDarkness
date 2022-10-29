using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;
public enum DialogueState
{
    DEFAULT,
    STARTED,
    FINISHED
}

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance{get;private set;}
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject dialogueEndPossibility;
    [SerializeField] Animator dialogueAnimator;
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField] float timePerCharacter;
    [SerializeField] float timePerSentence;
    public DialogueState dialogueState = DialogueState.DEFAULT;

    public DialogueText entryDialogueText;
    public static event Action OnDialogueStarted;
    public static event Action OnDialogueEnded;
    private void Awake() {
        if(Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }
    
    public void DialogueStartPhase(DialogueText dialogueText, bool transitionInside = true)
    {
        OnDialogueStarted();
        dialogueState = DialogueState.STARTED;
        if(transitionInside == true) dialogueAnimator.SetTrigger("appear");
        dialoguePanel.SetActive(true);
        StartCoroutine(textTransition(dialogueText));
    }
    public void DialogueShowEnd()
    {
        dialogueState = DialogueState.FINISHED;
        dialogueEndPossibility.SetActive(true);
    }
    public void DialogueEndPhase()
    {
        dialogueAnimator.SetTrigger("disappear");
        OnDialogueEnded();
    }
    IEnumerator textTransition(DialogueText dialogueText)
    {
        string[] text = dialogueText.textToDisplayAtOnce;
        for(int i=0;i<text.Length;i++)
        {
          //  Debug.Log(text[i].Length);
            int currentIndex =0;
            bool tagStarted = false;
            string tagStorage = string.Empty;
            tMP_Text.text = string.Empty;
            while(currentIndex<text[i].Length)
            {
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
                        Debug.Log(tagStorage);
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
