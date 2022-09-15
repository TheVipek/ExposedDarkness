using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] Canvas gameOverCanvas;
    public static DeathHandler instance;
    private void Awake() {

        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    private void OnEnable() {
        PlayerHealth.onDamageTaken+=HandleDeath;
    }
    private void OnDisable() {
        PlayerHealth.onDamageTaken-=HandleDeath;
    }
    private void Start() {
        gameOverCanvas.enabled = false;
    }
    public void HandleDeath()
    {
        if(PlayerHealth.instance.CurrentHealth<=0)
        {
            gameOverCanvas.enabled = true;
            Time.timeScale=0;
            FindObjectOfType<WeaponSwitcher>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
}
