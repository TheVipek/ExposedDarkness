using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class completionMissionAction : MonoBehaviour,IAnyButton
{
    private bool canBeUsed = false;
    public bool CanBeUsed{get{return canBeUsed;} set{canBeUsed = value;}}
    public UnityEvent completeMissionEvent;
    InputAction clickAction;
    [SerializeField] AnimationClip showingClip;
    private void Awake() {
        clickAction = new InputAction(binding:"/Keyboard/anyKey");
        clickAction.AddBinding("/Mouse/leftButton");
        clickAction.AddBinding("/Mouse/rightButton");
        clickAction.Enable();

       showingClip.AddEvent(new AnimationEvent(){
        time = showingClip.length,
        functionName = "TriggerUsage"
       });
    }
    private void OnEnable() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        completeMissionEvent.Invoke();
        clickAction.started += OnClicked;
    }

    private void OnDisable() {
        clickAction.started -= OnClicked;
        clickAction.Disable();
        
    }
    public void OnClicked(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            
            
                Debug.Log("Pressed this frame.");
                if(canBeUsed == true) 
                {
                    canBeUsed = false;
                    Debug.Log("Clicked.");
                    AnyButtonAction();
                }
                

            
        }
    }
    
    public void AnyButtonAction()
    {
            SceneController.Instance.GoToScene("MainMenu");
    }
    public void TriggerUsage() => canBeUsed = true;
    //public void enableInput() => clickAction.Enable();
}
