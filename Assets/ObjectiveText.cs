using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ObjectiveText : MonoBehaviour
{
    public TMP_Text objectiveText;
    public Toggle objectiveCheckbox;

    public void SetObjectiveCheckBox(bool value)
    {
        objectiveCheckbox.isOn = value;
    }
}
