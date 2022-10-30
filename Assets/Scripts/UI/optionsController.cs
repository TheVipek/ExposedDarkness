using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class optionsController : MonoBehaviour
{
    [SerializeField] Toggle fullScreenCheckBox;
    [SerializeField] Image preventingResolutionPanel;
    [SerializeField] TMP_Dropdown resolution;
    void Awake()
    {
        fullScreenCheckBox.onValueChanged.AddListener(delegate
        {
            FullScreenChecker(fullScreenCheckBox);
        });
        resolution.onValueChanged.AddListener(delegate{
            ResolutionChangeHanlder(resolution);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Checks for fullscreen option and depending on it changes window-type
    void FullScreenChecker(Toggle toggle)
    {
        Debug.Log(toggle.isOn);
        if(toggle.isOn == true)
        {
            preventingResolutionPanel.enabled = true;
            Screen.SetResolution(Screen.width,Screen.height,FullScreenMode.FullScreenWindow,0);
        }else
        {
            preventingResolutionPanel.enabled = false;
            Screen.SetResolution(Screen.width,Screen.height,FullScreenMode.Windowed,0);

        }
    } 
    void ResolutionChangeHanlder(TMP_Dropdown dropdown)
    {
        string[] parameters = dropdown.options[dropdown.value].text.Split("x");
        Screen.SetResolution(int.Parse(parameters[0]),int.Parse(parameters[1]),FullScreenMode.Windowed,0);
    }
}
