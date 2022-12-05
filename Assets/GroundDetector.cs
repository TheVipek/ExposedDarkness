using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start() {
        // if(playerMovement == null) Debug.LogWarning($"Player movement is null: {playerMovement} in {GetType()}.");
        playerMovement = PlayerMovement.Instance;
    }
     private void OnTriggerStay(Collider other) {
        int othLayer = 1<< other.gameObject.layer;
        // Debug.Log($"groundLayer:{playerMovement.groundLayer.value} | otherLayer:{othLayer}");
        if(playerMovement.IsGrounded == false && (playerMovement.groundLayer.value == othLayer))
        {
            Debug.Log("Grounded!");
            playerMovement.IsGrounded = true;
            if(playerMovement.jumping)
            {
                playerMovement.jumping = false;
                playerMovement.SetMoveBasedOnState();
            }
        }
    }
    
    private void OnTriggerExit(Collider other) {
        
      //  Debug.Log("Exit:"+other.gameObject.name);
        if(playerMovement.IsGrounded == true)
        {
            Debug.Log("Not grounded!");
            playerMovement.IsGrounded = false;
            playerMovement.SetMoveBasedOnState();
        }
    }
}
