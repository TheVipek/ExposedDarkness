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
    [SerializeField] Image dialoguePanel;
    
    [SerializeField] GameObject dialogueEndPossibility;
    [SerializeField] Animator dialogueAnimator;
    [SerializeField] TMP_Text dialogueTMP;
    [SerializeField] float timePerCharacter;
    [SerializeField] float timePerSentence;
    private WaitForSecondsRealtime waitForTimePerCharacter;
    private WaitForSecondsRealtime waitForTimePerSentence;

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
    private void OnEnable() {
        waitForTimePerCharacter = new WaitForSecondsRealtime(timePerCharacter);
        waitForTimePerSentence = new WaitForSecondsRealtime(timePerSentence);

    }
    public IEnumerator DialogueStartPhase(Dialogue _dialogue, bool transitionInside = true)
    {
        OnGlobalDialogueStarted();
        currentDialogue.callStartEvents();
        currentDialogue = _dialogue;
        // dialoguePanel.SetActive(true);
        if (transitionInside == true)
        {
            dialogueAnimator.enabled = true;
            dialogueAnimator.SetTrigger("appear");
            //yield return null;
            float animLength = dialogueAnimator.GetCurrentAnimatorStateInfo(0).length;
            WaitForSecondsRealtime _animLength = new WaitForSecondsRealtime(animLength);
            yield return _animLength;
        }
        else
        {
            dialogueAnimator.enabled = false;
            Color dialogueImageColor = dialoguePanel.color;
            Color dialogueTextColor = dialogueTMP.color;
            dialogueTMP.color = new Color(dialogueTextColor.r,dialogueTextColor.g,dialogueTextColor.b,255);
            dialoguePanel.color = new Color(dialogueImageColor.r,dialogueImageColor.g,dialogueImageColor.b,255);
        }

       
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
        OnGlobalDialogueEnded();
        currentDialogue.callEndEvents();
        if(dialogueAnimator.enabled == false) dialogueAnimator.enabled = true;
        
        dialogueAnimator.SetTrigger("disappear");
        float animLength = dialogueAnimator.GetCurrentAnimatorStateInfo(0).length;
        WaitForSecondsRealtime _animLength = new WaitForSecondsRealtime(animLength);

        yield return _animLength;
        dialogueAnimator.enabled = false;

    }

    IEnumerator textTransition(Dialogue _dialogue)
    {
        string[] text = _dialogue.dialogueText.textToDisplayAtOnce;
        for (int i = 0; i < text.Length; i++)
        {
            
            int currentIndex = 0;
            bool tagStarted = false;
            string tagStorage = string.Empty;
            dialogueTMP.text = string.Empty;
            currentDialogue.checkForSentenceEvent(i);
            while (currentIndex < text[i].Length)
            {
                #region tagDetecting
                if (text[i][currentIndex] == '<')
                {
                    tagStarted = true;
                }

                if (tagStarted == true)
                {
                    tagStorage += text[i][currentIndex];
                 //   Debug.Log(text[i][currentIndex]);
                    if (text[i][currentIndex] == '>')
                    {
                        tagStarted = false;
                        dialogueTMP.text += tagStorage;
                        tagStorage = string.Empty;
                    }
                    
                }
                #endregion
                else
                {
                    dialogueTMP.text += text[i][currentIndex];
                }
                #region endOfSentenceDetecting
               // if(text[i][currentIndex] == '.' || text[i][currentIndex] == '!' || text[i][currentIndex] == '?') yield return waitForTimePerSentence.waitTime/2;
                #endregion
                currentIndex += 1;
                yield return waitForTimePerCharacter;
            }
            yield return waitForTimePerSentence;
        }
        DialogueShowEnd();
        yield return null;
    }
}
