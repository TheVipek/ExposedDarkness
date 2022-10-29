using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance{get;private set;}
    public float loadingProgress{get; private set;} 
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
    public void Quit()
    {
        Application.Quit();
    }
    public void GoToScene(string sceneName)
    {
        Debug.Log("GoToScene called!");
        string previousScene = SceneManager.GetActiveScene().name;
        
        StartCoroutine(goToLoadingScene(previousScene,sceneName));

       // SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);
    }
    IEnumerator goToLoadingScene(string previousScene,string wantGoSceneName)
    {
        Debug.Log("Start Loading...");
        WaitForSeconds isCompletedChecker = new WaitForSeconds(0.1f);
        WaitForSeconds swappingTime = new WaitForSeconds(2f);
        AsyncOperation sceneLoading = SceneManager.LoadSceneAsync("loadingScene",LoadSceneMode.Additive);
        //actionsOnScenes.Add(SceneManager.LoadSceneAsync(wantGoSceneName,LoadSceneMode.Additive));

        Debug.Log("Entering Loading Loop...");
        while(!sceneLoading.isDone)
        {
            yield return null;
        }
        AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(wantGoSceneName,LoadSceneMode.Additive);
        sceneToLoad.allowSceneActivation = false;
        while(sceneToLoad.progress < 0.9f)
        {
            loadingProgress = sceneToLoad.progress;
            yield return isCompletedChecker;
        }
        loadingProgress = 1;
        yield return swappingTime;
        sceneToLoad.allowSceneActivation = true;
        while(!sceneToLoad.isDone)
        {
            yield return null;
        }
        SceneManager.UnloadSceneAsync(previousScene);
        SceneManager.UnloadSceneAsync("loadingScene");
        // AsyncOperation loadingScene = SceneManager.LoadSceneAsync("loadingScene",LoadSceneMode.Additive);
        // while(!loadingScene.isDone)
        // {
        //     yield return isCompletedChecker;
        // }
        // Debug.Log("LoadingScene loaded");
        // AsyncOperation sceneToUnload = SceneManager.UnloadSceneAsync(previousScene);
        // AsyncOperation sceneToLoad = SceneManager.LoadSceneAsync(wantGoSceneName,LoadSceneMode.Additive);
        // Debug.Log("Starting to unload "+previousScene+" ,"+"and load "+sceneToLoad );
        // sceneToLoad.allowSceneActivation = false;
        
        

        // while(!sceneToUnload.isDone)
        // {
        //     yield return isCompletedChecker;
        // }
        // sceneToLoad.allowSceneActivation = true;

        // while(!sceneToLoad.isDone)
        // {
        //     yield return isCompletedChecker;
        // }
        // SceneManager.UnloadSceneAsync("loadingScene");
        // Cursor.lockState = CursorLockMode.Confined;
        
        
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
        Debug.Log(scene.name + " loaded!");
        SceneManager.SetActiveScene(scene);
        if(SceneManager.GetActiveScene().name == "GameScene")
        {
            Debug.Log("Entry GameScene!");
            DialogueController dialogueController = DialogueController.Instance;
            dialogueController.DialogueStartPhase(dialogueController.entryDialogueText,false);
        }
    }
}
