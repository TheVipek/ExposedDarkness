using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    Camera mCamera;
    public static PlayerCamera instance;
    [SerializeField] float shakeDuration = .5f;
    [SerializeField] float shakePower = 1f;


    private void Awake() {
        if(instance!=this && instance != null)
        {
            Destroy(this);
        }else{
            instance = this;
        }
        mCamera = GetComponent<Camera>();
    }

           
    public IEnumerator shakeCamera()
    {
        Vector3 originalPos = transform.localPosition;
        float timeElapsed = shakeDuration;

        while(timeElapsed > 0)
        {
            float x = Random.Range(-1f,1f) * shakePower;
            float y = Random.Range(-1f,1f) * shakePower;
            transform.localPosition = Vector3.Lerp(originalPos,new Vector3(originalPos.x+x,originalPos.y+y,originalPos.z),.1f);
            timeElapsed-=Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
        
    }
}
