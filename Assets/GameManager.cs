using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get;private set;}
    [SerializeField] InputActionAsset playerInputActions;
    private Weapon currentWeapon;
    private void Awake() {
        if(Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }
    public void freezeTime()
    {
        AudioListener.pause = true;
        freezePlayerActions();
        Time.timeScale = 0;
    }
    public void unfreezeTime()
    {
        AudioListener.pause = false;
        unfreezePlayerActions();
        Time.timeScale = 1;

    }
    public void freezePlayerActions() => playerInputActions.FindActionMap("Player",true).Disable();
    public void unfreezePlayerActions() => playerInputActions.FindActionMap("Player",true).Enable();
    public void activeCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    } 
    public void deactivateCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void OnEnable() {
        DialogueController.OnGlobalDialogueStarted += freezeTime;

        DialogueController.OnGlobalDialogueEnded += unfreezeTime;
    }
    private void OnDisable() {
        DialogueController.OnGlobalDialogueStarted -= freezeTime;

        DialogueController.OnGlobalDialogueEnded -= unfreezeTime;
    }
}
