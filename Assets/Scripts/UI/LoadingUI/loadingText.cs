using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class loadingText : MonoBehaviour
{
    [SerializeField] TMP_Text loadingTMP;
    [SerializeField] TMP_Text dotsTMP;
    [SerializeField] float forwardSpeed = 0.4f;
    [SerializeField] float backwardSpeed = 0.15f;
    int dots = 3;
    loadingClickListener loadingClickListener;

    private void Awake() {
        loadingClickListener = GetComponent<loadingClickListener>();
    }
    private void OnEnable() {
        SceneController.onLoadingPrepared += preparedSceneText;
        
    }
    private void OnDisable() {
        SceneController.onLoadingPrepared -= preparedSceneText;
        
    }
    void Start()
    {
        StartCoroutine(smoothText());
    }

    IEnumerator smoothText()
    {
        WaitForSeconds wForward = new WaitForSeconds(forwardSpeed);
        WaitForSeconds wBackward = new WaitForSeconds(backwardSpeed);
        
        while(true)
        {
            while(dots > 0)
            {
                dotsTMP.text+=".";
                dots -= 1;
                yield return wForward;
            }
            while(dots < 3)
            {
                dotsTMP.text = dotsTMP.text.Remove(dotsTMP.text.Length -1); 
                dots += 1;
                yield return wBackward;
            }
            yield return null;
        }
        
    }
    public void preparedSceneText()
    {
        if(loadingClickListener != null) loadingClickListener.enabled = true;
        if(SceneController.Instance.GoingToScene != "MainMenu")
        {
            loadingTMP.text = "Click Any Key To Continue";
        }
        else
        {
            loadingClickListener.AnyButtonAction();
        }
    }
}
