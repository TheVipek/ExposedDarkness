using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class barUpdater : MonoBehaviour
{
    [SerializeField] Image loadingImage; 
    float t = 0;
    private void Start() {
        StartCoroutine(smoothLoading());
    }
    IEnumerator smoothLoading()
    {
        while(t < 1f)
        {
            loadingImage.fillAmount = Mathf.Lerp(loadingImage.fillAmount,SceneController.Instance.loadingProgress,t);
            t += 0.1f * Time.deltaTime;
            yield return null;
        }

    }
}
