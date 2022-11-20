using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get;private set;}
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerWeapons;
    [SerializeField] Camera mainSceneCamera;
    private void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }
    public void freezeTime()
    {
       // Debug.Log("freezedMovement");
        Time.timeScale = 0;
        freezePlayerActions();
    }
    public void unfreezeTime()
    {
      //  Debug.Log("unfreezedMovement");
        Time.timeScale = 1;
        unfreezePlayerActions();

    }
    public void freezePlayerActions()
    {

        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerHealth>().enabled = false;
        playerWeapons.SetActive(false);
    }
    public void unfreezePlayerActions()
    {

        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerHealth>().enabled = true;
        playerWeapons.SetActive(true);
    }
    private void OnEnable() {
        DialogueController.OnGlobalDialogueStarted += freezeTime;
        //DialogueController.OnGlobalDialogueStarted += freezePlayerActions;

        DialogueController.OnGlobalDialogueEnded += unfreezeTime;
        //DialogueController.OnGlobalDialogueEnded += unfreezePlayerActions;
    }
    private void OnDisable() {
        DialogueController.OnGlobalDialogueStarted -= freezeTime;
        //DialogueController.OnGlobalDialogueStarted -= freezePlayerActions;

        DialogueController.OnGlobalDialogueEnded -= unfreezeTime;
       // DialogueController.OnGlobalDialogueEnded -= unfreezePlayerActions;
    }
}
