using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip transitionIn;
    public float TransitionInLength { get{return transitionIn.length;;} } 
    [SerializeField] AnimationClip transitionOut;
    public float TransitionOutLength { get{return transitionOut.length;;} } 



    public void TransitionIn()
    {
        
        EnableTransition("TransitionIn");
    }
    public void TransitionOut()
    {
        EnableTransition("TransitionOut");
    }
    public void TransitionBoth()
    {
        EnableTransition("TransitionIn");
        EnableTransition("TransitionOut");
    }
    public void DisableTransition(string transition) => animator.SetBool(transition,false);
    public void EnableTransition(string transition) => animator.SetBool(transition,true);
    private void OnDisable() => animator.Rebind(); 
}
