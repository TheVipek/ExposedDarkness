using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class sceneSwapper : MonoBehaviour
{   
    public void swapScene(string scene)
    {
        Debug.Log("scene swap called");
        SceneController.Instance.GoToScene(scene);

    }
}
