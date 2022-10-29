using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class sceneSwapper : MonoBehaviour
{
    [SerializeField] Animator animator;
    
    [SerializeField] Image panel;
    private void OnEnable() {
        animator.SetTrigger("sceneChange");
        
    }
    private void OnDisable() {
        panel.color = new Color32(255,255,255,0);
    }
    
    
    public void createAnimatorWithSceneParameter(string scene)
    {
        gameObject.SetActive(true);
        AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
        if(clip.events.Length > 0) return;
        AnimationEvent animationEvent = new AnimationEvent();
        animationEvent.functionName = "swapScene";
        animationEvent.stringParameter = scene;
        animationEvent.time = clip.length;
        clip.AddEvent(animationEvent);
        
    }
    
    public void swapScene(string scene)
    {
        Debug.Log("scene swap called");
        SceneController.Instance.GoToScene(scene);

    }
}
