using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeathHandler : MonoBehaviour
{
    Canvas gameOverCanvas;
    [SerializeField] TMP_Text deathText;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip deathUI;
    public static DeathHandler instance;
    private void OnEnable() {
        PlayerHealth.onDamageTaken+=HandleDeath;
    }
    private void OnDisable() {
        PlayerHealth.onDamageTaken-=HandleDeath;
    }
    private void Awake() {
        if(instance!= null && instance != this)
        {
            Destroy(this);
        }else
        {
            instance = this;
        }
        gameOverCanvas = GetComponent<Canvas>();
    }
    private void Start() {
        gameOverCanvas.enabled = false;
    }
    public void HandleDeath()
    {
        if(PlayerHealth.instance.CurrentHealth<=0)
        {
            PlayerHealth.instance.IsDead = true;
            PlayerHealth.instance.animator.enabled = true;
            PlayerMovement.instance.enabled = false;
            WeaponSwitcher.instance.gameObject.SetActive(false);
            gameOverCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // Time.timeScale=0;
        }
        
    }
    public void DeathUIanimation()
    {
        animator.SetTrigger("deathUI");
    }
    public void OutOfTime()
    {
        PlayerHealth.instance.IsDead = true;
        deathText.text = "YOU HAVE"+"\n"+"BEEN"+"\n"+"POISONED";
        PlayerHealth.instance.animator.enabled = true;
        PlayerMovement.instance.enabled = false;
        WeaponSwitcher.instance.gameObject.SetActive(false);
        gameOverCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
