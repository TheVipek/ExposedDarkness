using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class renderTexture : MonoBehaviour
{
    Camera _camera;

    public List<GameObject> sceneObjects;
    [SerializeField] string path;
    [SerializeField] string prefix;

    [ContextMenu("Screenshot")]
    private void ProcessScreenshots()
    {
        
    }
    private IEnumerator ScreenShot()
    {
        for (int i = 0; i < sceneObjects.Count; i++)
        {
            GameObject obj = sceneObjects[i];
            obj.gameObject.SetActive(true);
            yield return null;
            takeScreenshot($"{Application.dataPath}/{path}/{i}_Icon.png");
            yield return null;
            obj.gameObject.SetActive(false);
            Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>($"Assets/{path}/{i}_Icon.png");
            yield return null;
        }
    }
    
    void takeScreenshot(string fullPath)
    {
        if(_camera == null)
        {
            _camera = GetComponent<Camera>();
        }
        RenderTexture rt = new RenderTexture(256,256,24);
        _camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(256,256,TextureFormat.RGBA32,false);
        _camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0,0,256,256),0,0);
        _camera.targetTexture = null;
        RenderTexture.active = null;
        if(Application.isEditor)
        {
            DestroyImmediate(rt);
        }else
        {
            Destroy(rt);
        }
        byte[] bytes = screenShot.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullPath,bytes);
    #if UNITY_EDITOR
        AssetDatabase.Refresh();
    #endif
    }
}

