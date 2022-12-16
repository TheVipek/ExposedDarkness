using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFinish : MonoBehaviour
{
    [SerializeField] WaveTrigger waveTrigger;
    [SerializeField] Canvas missionCompletionPanel;

    public void ShowMissionComplete() => missionCompletionPanel.gameObject.SetActive(true);
}
