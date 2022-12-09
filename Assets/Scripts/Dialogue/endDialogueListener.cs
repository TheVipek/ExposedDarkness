using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
public class endDialogueListener : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] TMP_Text tMP_Text;
    private bool pressed = false;


    private void OnGUI() {
        Event e = Event.current;
        if((e.isKey || e.isMouse) && pressed == false)
        {
            pressed = true;
            Debug.Log("keyboard event has occured!");
            animator.SetBool("clicked",true);
            StartCoroutine(DialogueController.Instance.DialogueEndPhase());
        }
    }
   
   private void OnDisable() {
        tMP_Text.color = new Color32(255,255,255,0);
   }
}
