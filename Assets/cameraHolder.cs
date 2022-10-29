using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class cameraHolder : MonoBehaviour
{
    [SerializeField] List<Camera> cameras;
    Scene myScene;
    private void Awake() {
        myScene = SceneManager.GetActiveScene();
    }
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;        
        
    }
    public void setListeners(bool activate)
    {
        for (int i = 0; i < cameras.Count; i++){
            if(cameras[i].TryGetComponent<AudioListener>(out AudioListener audioListener) == true)
            {
             //  audioListener.volume
                audioListener.enabled = activate;
                //cameras[i].enabled = activate;
            }
            
        }
    }
    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if(scene == myScene)
        {
            setListeners(true);
        }else
        {
            setListeners(false);
        }
    }
    public void setSound(bool activate)
    {
        if(myScene == SceneManager.GetActiveScene())
        {
            
        }
    }

}