using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    private void OnTriggerEnter(Collider other) {
        int othLayer = 1<<other.gameObject.layer;
   //     Debug.Log($"CollisionEnter {othLayer}");
        if(playerMovement.IsGrounded == false && playerMovement.groundLayer.value == othLayer)
        {
        //    Debug.Log("Grounded in Enter");

            playerMovement.IsGrounded = true;
            if(playerMovement.Jumping)
            {
                playerMovement.Jumping = false;
            }
            playerMovement.SetMoveBasedOnState();
        }
    }
     private void OnTriggerStay(Collider other) {

        int othLayer = 1<< other.gameObject.layer;
//        Debug.Log($"CollisionStay {othLayer}");
        
        // Debug.Log($"groundLayer:{playerMovement.groundLayer.value} | otherLayer:{othLayer}");
        if(playerMovement.IsGrounded == false && playerMovement.groundLayer.value == othLayer)
        {
        //    Debug.Log("Grounded in Stay");
            playerMovement.IsGrounded = true;

        }
    }
    
    private void OnTriggerExit(Collider other) {
        
   //     Debug.Log("Exit:"+other.gameObject.name);
        if(playerMovement.IsGrounded == true)
        {
         //   Debug.Log("Not grounded in Exit");
            playerMovement.IsGrounded = false;
        }
    }
}
