using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private PlayerMovement playerMovement;
    // private RaycastHit hit;
    // [SerializeField] LayerMask layerToIgnore;
    private void Start() {
        // if(playerMovement == null) Debug.LogWarning($"Player movement is null: {playerMovement} in {GetType()}.");
        playerMovement = PlayerMovement.Instance;
    }
    // private void Update() {
    //     if(Physics.Raycast(transform.position,-transform.up,out RaycastHit hit ,.2f,~layerToIgnore))
    //     {
    //        //// Debug.DrawRay(transform.position,hit.normal*.25f,Color.green,15);
    //       ////  Debug.Log(hit.transform.name);
    //       //  Debug.Log(transform.position);
    //         if(playerMovement.IsGrounded == false && playerMovement.movementActions == MovementActions.JUMPING)
    //         {
    //             playerMovement.movementActions = MovementActions.DEFAULT;
    //             playerMovement.IsGrounded = true;
    //         }
    //         else
    //         {
    //             playerMovement.IsGrounded = true;
    //             if(playerMovement.Jumping)
    //             {
    //                 playerMovement.Jumping = false;
    //                 playerMovement.SetMoveBasedOnState();
    //             }
    //         }
    //     }
    //     else
    //     {
    //         if(playerMovement.IsGrounded == true)
    //         {
    //             playerMovement.IsGrounded = false;
    //             playerMovement.SetMoveBasedOnState();
    //         }
    //     }
    // }
    private void OnTriggerEnter(Collider other) {
        int othLayer = 1<<other.gameObject.layer;
   //     Debug.Log($"CollisionEnter {othLayer}");
        
        if(playerMovement.IsGrounded == false && playerMovement.groundLayer.value == othLayer)
        {
            Debug.Log("Grounded in Enter");

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
            Debug.Log("Grounded in Stay");
            playerMovement.IsGrounded = true;

        }
    }
    
    private void OnTriggerExit(Collider other) {
        
   //     Debug.Log("Exit:"+other.gameObject.name);
        if(playerMovement.IsGrounded == true)
        {
           Debug.Log("Not grounded in Exit");
            playerMovement.IsGrounded = false;
        }
    }
}
