using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    [SerializeField] TMP_Text deathText;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathUI;
    private void Start() {
        gameOverCanvas.enabled = false;
    }
}
