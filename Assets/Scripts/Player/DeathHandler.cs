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
        if(PlayerHealth.Instance.CurrentHealth<=0)
        {
            PlayerMovement.Instance.PlayerMapActivate(false);
            PlayerMovement.Instance.enabled = false;
            PlayerCamera.Instance.enabled = false;
            WeaponsManager.Instance.weaponSwitcher.gameObject.SetActive(false);
            


            PlayerHealth.Instance.IsDead = true;
            PlayerHealth.Instance.animator.enabled = true;
            gameOverCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.Confined;
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
        PlayerMovement.Instance.PlayerMapActivate(false);
        PlayerMovement.Instance.enabled = false;
        PlayerCamera.Instance.enabled = false;
        WeaponsManager.Instance.weaponSwitcher.gameObject.SetActive(false);
        


        PlayerHealth.Instance.IsDead = true;
        deathText.text = "YOU HAVE"+"\n"+"BEEN"+"\n"+"POISONED";
        PlayerHealth.Instance.animator.enabled = true;
        gameOverCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
