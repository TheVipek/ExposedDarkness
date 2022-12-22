using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class barUpdater : MonoBehaviour
{
    [SerializeField] Image loadingImage; 
    float t;
    
    private void Start() {
        StartCoroutine(smoothLoading());
    }
    IEnumerator smoothLoading()
    {
        t = 0;
        while(SceneController.Instance.loadingProgress == 0)
        {
            yield return null;
        }
        while(t < 1f)
        {
            loadingImage.fillAmount = Mathf.Lerp(loadingImage.fillAmount,SceneController.Instance.loadingProgress,t);
            t += 0.1f*Time.deltaTime;
            yield return null;
        }

    }
}
