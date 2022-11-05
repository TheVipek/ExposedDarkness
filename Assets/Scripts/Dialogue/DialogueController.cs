using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;


public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] GameObject dialogueEndPossibility;
    [SerializeField] Animator dialogueAnimator;
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField] float timePerCharacter;
    [SerializeField] float timePerSentence;

    public Dialogue currentDialogue;
    public static event Action OnGlobalDialogueStarted;
    public static event Action OnGlobalDialogueEnded;
    private void Awake()
    {

        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public IEnumerator DialogueStartPhase(Dialogue _dialogue, bool transitionInside = true)
    {
        // dialoguePanel.SetActive(true);
        if (transitionInside == true)
        {
            dialogueAnimator.SetTrigger("appear");
            //yield return null;
            float animLength = dialogueAnimator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animLength);
        }

        OnGlobalDialogueStarted();
        currentDialogue.callStartEvents();
        currentDialogue = _dialogue;
        yield return StartCoroutine(textTransition(currentDialogue));
    }
    public void StartShowingText()
    {
        StartCoroutine(textTransition(currentDialogue));
    }
    public void DialogueShowEnd()
    {
        if (currentDialogue.forceExitDialogue == true)
        {
            StartCoroutine(DialogueEndPhase());
        }
        else
        {
            dialogueEndPossibility.SetActive(true);
        }
    }
    public IEnumerator DialogueEndPhase()
    {
        dialogueAnimator.SetTrigger("disappear");
        float animLength = dialogueAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animLength);
        OnGlobalDialogueEnded();
        currentDialogue.callEndEvents();
    }
    IEnumerator textTransition(Dialogue _dialogue)
    {
        string[] text = _dialogue.dialogueText.textToDisplayAtOnce;
        for (int i = 0; i < text.Length; i++)
        {
            
            int currentIndex = 0;
            bool tagStarted = false;
            string tagStorage = string.Empty;
            tMP_Text.text = string.Empty;
            currentDialogue.checkForSentenceEvent(i);
            while (currentIndex < text[i].Length)
            {
                if (text[i][currentIndex] == '<')
                {
                    tagStarted = true;
                }

                if (tagStarted == true)
                {
                    if (text[i][currentIndex] == '>')
                    {
                        tagStarted = false;
                    }
                    tagStorage += text[i][currentIndex];
                }

                if (tagStarted == false)
                {
                    if (tagStorage != string.Empty)
                    {
                        tMP_Text.text += tagStorage;
                        tagStorage = string.Empty;
                    }
                    else
                    {
                        tMP_Text.text += text[i][currentIndex];
                    }
                }

                currentIndex += 1;
                yield return new WaitForSeconds(timePerCharacter);
            }
            yield return new WaitForSeconds(timePerSentence);
        }
        DialogueShowEnd();
        yield return null;
    }
}
