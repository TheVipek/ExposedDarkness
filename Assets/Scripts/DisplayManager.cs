using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour
{
    public int FullScreen
    {
        get{return PlayerPrefs.GetInt("fullScreenMode",-1);}
    }
    public int ResolutionX
    {
        get{return PlayerPrefs.GetInt("currentResolutionX",-1); }
    }
    public int ResolutionY
    {
        get{return PlayerPrefs.GetInt("currentResolutionY",-1); }
    }
    public int ResolutionIdx
    {
        get{return PlayerPrefs.GetInt("currentResolutionIdx",-1); }
    }
    [SerializeField] DisplayController displayController;
    private void Awake() {
        int fullScreenMode = FullScreen;
        if(fullScreenMode != -1)
        {
            if(fullScreenMode == 1)
            {
               Debug.Log("fullScreen On");
                displayController.ResolutionInteractable(false);
                // displayController.fullScreenCheckBox.isOn = true;
                // displayController.resolution.interactable = false;
            }
            else
            {
                Debug.Log("fullScreen Off");
                displayController.ResolutionInteractable(true);
                // displayController.fullScreenCheckBox.isOn = false;
                // displayController.resolution.interactable = true;

            }

            int[] currentResolution = { ResolutionX,ResolutionY };
            Debug.Log(currentResolution[0] + ":" + currentResolution[1]);

            if(currentResolution[0] != -1 && currentResolution[1] != -1)
            {
                Screen.SetResolution(currentResolution[0],currentResolution[1],fullscreenMode:(FullScreenMode)fullScreenMode);
                int currentResolutionIdx = ResolutionIdx;
                //selected index 
                //int currIndex = resolution.value;
                //All options
                //List<TMP_Dropdown.OptionData> list = resolution.options;
                //Change selected index
                
                //Debug.Log(resolution.value);
                Debug.Log(currentResolutionIdx);
                displayController.resolution.value = currentResolutionIdx;
                Debug.Log(displayController.resolution.value);
                
            }
        }
    }
    public static void SetFullScreen(int fullScreenValue)
    {
        PlayerPrefs.SetInt("fullScreenMode",fullScreenValue);

    }
    public static void SetResolution(int[] resolution, int resolutionIdx)
    {
        PlayerPrefs.SetInt("currentResolutionX",resolution[0]);
        PlayerPrefs.SetInt("currentResolutionY",resolution[1]);
        PlayerPrefs.SetInt("currentResolutionIdx",resolutionIdx);
    }
}
