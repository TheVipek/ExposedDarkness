using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get;private set;}
    [SerializeField] GameObject player;
    [SerializeField] GameObject mainWeaponsContainer;
    // [SerializeField] MonoBehaviour[] playerWeaponsScripts;
    private Weapon currentWeapon;
    // private MonoBehaviour[] currentWeaponComponents;
    private void Awake() {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }
    public void freezeTime()
    {
        AudioListener.pause = true;
        
        Time.timeScale = 0;
        freezePlayerActions();
    }
    public void unfreezeTime()
    {
        AudioListener.pause = false;

        unfreezePlayerActions();
        Time.timeScale = 1;

    }
    public void freezePlayerActions()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerHealth>().enabled = false;

        mainWeaponsContainer.gameObject.SetActive(false);
        // foreach(MonoBehaviour _comp in playerWeaponsScripts) _comp.enabled = false;
  
        // currentWeapon = playerWeaponsMain.GetComponent<WeaponSwitcher>().CurrentWeapon;
        // currentWeaponComponents = currentWeapon.GetComponents<MonoBehaviour>();

        // foreach(MonoBehaviour _comp in currentWeaponComponents)
        // {
        //     if(_comp.GetType() == typeof(WeaponAnimation)) continue; 
        //     _comp.enabled = false;
        // }
      
    }
    public void unfreezePlayerActions()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerHealth>().enabled = true;
        mainWeaponsContainer.gameObject.SetActive(true);

        // foreach(MonoBehaviour _comp in playerWeaponsScripts) _comp.enabled = true;

        // foreach(MonoBehaviour _comp in currentWeaponComponents) _comp.enabled = true;
    }
    private void OnEnable() {
        DialogueController.OnGlobalDialogueStarted += freezeTime;

        DialogueController.OnGlobalDialogueEnded += unfreezeTime;
    }
    private void OnDisable() {
        DialogueController.OnGlobalDialogueStarted -= freezeTime;

        DialogueController.OnGlobalDialogueEnded -= unfreezeTime;
    }
}
