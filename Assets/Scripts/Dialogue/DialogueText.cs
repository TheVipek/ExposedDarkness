using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogueText", menuName = "Dialogue/DialogueText", order = 0)]
public class DialogueText : ScriptableObject {
    public string[] textToDisplayAtOnce;
}

