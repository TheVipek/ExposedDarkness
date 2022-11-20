using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class SceneController : MonoBehaviour
{
    public static SceneController Instance{get;private set;}
    public float loadingProgress{get; private set;} 
    public static Action onLoadingPrepared;
    public AsyncOperation sceneToLoad;
    
    private string goingToScene; 
    public string GoingToScene {get{return goingToScene;}}
    private void Awake() {
        Debug.Log("Initializing SceneController...");
        if(Instance!=this && Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;    
            DontDestroyOnLoad(gameObject);
        }
    }
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void GoToScene(string sceneName)
    {
        //Debug.Log("GoToScene called!");
        string previousScene = SceneManager.GetActiveScene().name;
        loadingProgress = 0;
        StartCoroutine(goToLoadingScene(previousScene,sceneName));

       // SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
    }
    IEnumerator goToLoadingScene(string previousScene,string wantGoSceneName)
    {
        goingToScene = wantGoSceneName;
        //Debug.Log("Start Loading...");
        WaitForSeconds isCompletedChecker = new WaitForSeconds(0.1f);
        WaitForSeconds swappingTime = new WaitForSeconds(3f);
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("loadingScene",LoadSceneMode.Additive);
        //actionsOnScenes.Add(SceneManager.LoadSceneAsync(wantGoSceneName,LoadSceneMode.Additive));

        //Debug.Log("Entering Loading Loop...");
        while(!sceneLoading.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(previousScene);
        yield return swappingTime;
        sceneToLoad = SceneManager.LoadSceneAsync(wantGoSceneName,LoadSceneMode.Additive);
        sceneToLoad.allowSceneActivation = false;
        while(sceneToLoad.progress < 0.9f)
        {
            loadingProgress = sceneToLoad.progress;
            yield return null;
        }
        loadingProgress = 1;
        yield return swappingTime;
        onLoadingPrepared();
    }
    public void loadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

    }
    public void unloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
    //    SceneManager.SetActiveScene(scene);
    }
    public IEnumerator loadingPreparedAction()
    {
        sceneToLoad.allowSceneActivation = true;
        goingToScene = String.Empty;
        while(!sceneToLoad.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync("loadingScene");

    }
}
