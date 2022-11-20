using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class optionsController : MonoBehaviour
{
    [SerializeField] Toggle fullScreenCheckBox;
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

    //Checks for fullscreen option and depending on it changes window-type
    void FullScreenChecker(Toggle toggle)
    {
        Debug.Log(toggle.isOn);

        //if isOn is true ,fullscreen is activated
        if(toggle.isOn == true)
        {
            resolution.interactable = false;

            Screen.SetResolution(Screen.currentResolution.width,Screen.currentResolution.height,FullScreenMode.FullScreenWindow,0);

            PlayerPrefs.SetInt("fullScreenMode",1);
            PlayerPrefs.SetInt("currentResolutionX",Screen.currentResolution.width);
            PlayerPrefs.SetInt("currentResolutionX",Screen.currentResolution.height);


        }else
        {
            resolution.interactable = true;
            ResolutionChangeHanlder(resolution);
        }
    } 
    void ResolutionChangeHanlder(TMP_Dropdown dropdown)
    {
        string[] parameters = dropdown.options[dropdown.value].text.Split("x");
        //dropdown.options.IndexOf(dropdown.options[dropdown.value]);
        int resolutionIdx = dropdown.value;


        Screen.SetResolution(int.Parse(parameters[0]),int.Parse(parameters[1]),FullScreenMode.Windowed,0);
        PlayerPrefs.SetInt("fullScreenMode",3);
        PlayerPrefs.SetInt("currentResolutionX",int.Parse(parameters[0]));
        PlayerPrefs.SetInt("currentResolutionX",int.Parse(parameters[1]));
        PlayerPrefs.SetInt("currentResolutionIdx",resolutionIdx);
    }
}
