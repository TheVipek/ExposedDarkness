using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class DisplayController : MonoBehaviour
{
    [Header("FullScreen checkbox settings")]
    public Toggle fullScreenCheckBox;
    [SerializeField] Image checkmark;
    [SerializeField] Image checkmarkBackground;
    [SerializeField] Color32 checkedBackgroundColor;
    [SerializeField] Color32 uncheckedBackgroundColor; 
    [Header("Resolution dropdown settings")]
    public TMP_Dropdown resolution;
    [SerializeField] Image resolutionBackground;
    [SerializeField] TMP_Text resolutionText;
    [SerializeField] float interactableResolutionAlpha;
    [SerializeField] float uninteractableResolutionAlpha;

    private void Awake() {
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
        //Debug.Log(toggle.isOn);

        //if isOn is true ,fullscreen is activated
        if(toggle.isOn == true)
        {
            ResolutionInteractable(false);
            //Set resolution to player screen current resolution
            int[] intParameters = {Screen.currentResolution.width,Screen.currentResolution.height};
            Screen.SetResolution(intParameters[0],intParameters[1],FullScreenMode.FullScreenWindow);
            //Save it to PlayerPrefs for next game launch
            DisplayManager.SetFullScreen(1);
            //Setting resolutionIdx to set it to default position (index 0 in dropdownOptions)
            DisplayManager.SetResolution(intParameters,0);


            //PlayerPrefs.SetInt("fullScreenMode",1);
            // PlayerPrefs.SetInt("currentResolutionX",Screen.currentResolution.width);
            // PlayerPrefs.SetInt("currentResolutionX",Screen.currentResolution.height);


        }else
        {
            ResolutionInteractable(true);
            ResolutionChangeHanlder(resolution);
        }
    }
    public void ResolutionInteractable(bool interactable)
    {
        //To ensure that value is right
        if(fullScreenCheckBox.isOn != !interactable) fullScreenCheckBox.isOn = !interactable;
        Debug.Log(interactable);
        resolution.interactable = interactable;
        // Checkmark always has opposite value to resolution
        checkmark.enabled = !interactable;
        if(interactable == true)
        {
            checkmarkBackground.color = uncheckedBackgroundColor;
        }
        else
        {
            checkmarkBackground.color = checkedBackgroundColor;
        }

    }
    void ResolutionChangeHanlder(TMP_Dropdown dropdown)
    {
        //dropdown.options.IndexOf(dropdown.options[dropdown.value]);
        
        string[] parameters = dropdown.options[dropdown.value].text.Split("x");
        int[] intParameters = {int.Parse(parameters[0]), int.Parse(parameters[1])}; 
        int resolutionIdx = dropdown.value;

        //Set resolution to current resolution parameters (selected in dropdown)
        Screen.SetResolution(intParameters[0],intParameters[1],FullScreenMode.Windowed);
        //Save it to PlayerPrefs for next game launch
        DisplayManager.SetFullScreen(3);
        DisplayManager.SetResolution(intParameters,resolutionIdx);

        
        // PlayerPrefs.SetInt("currentResolutionX",int.Parse(parameters[0]));
        // PlayerPrefs.SetInt("currentResolutionX",int.Parse(parameters[1]));
        // PlayerPrefs.SetInt("currentResolutionIdx",resolutionIdx);
    }
}
