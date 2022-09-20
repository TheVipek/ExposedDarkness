using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeathHandler : MonoBehaviour
{
    Canvas gameOverCanvas;
    [SerializeField] TMP_Text deathText;

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
            PlayerHealth.instance.animator.enabled = true;
            
            PlayerHealth.instance.IsDead = true;
            PlayerMovement.instance.enabled = false;
            WeaponSwitcher.instance.gameObject.SetActive(false);
            
            // gameOverCanvas.enabled = true;
            // Time.timeScale=0;
            // Cursor.lockState = CursorLockMode.None;
            // Cursor.visible = true;
            
        }
        
    }
    public void OutOfTime()
    {
        deathText.text = "YOU HAVE"+"\n"+"BEEN"+"\n"+"POISONED";
        gameOverCanvas.enabled = true;
        Time.timeScale=0;
        WeaponSwitcher.instance.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
