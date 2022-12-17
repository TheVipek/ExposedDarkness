using UnityEngine;

[CreateAssetMenu]
public class PlayerHealthSettings : ScriptableObject
{
    [SerializeField] float maxHealth;
    public float MaxHealth {get{return maxHealth;}}
    private float currentHealth;
    public float CurrentHealth{get{return currentHealth;}}
    private PlayerHealth playerHealth;
    public PlayerHealth PlayerHealth{get{return playerHealth;}}
    public void AddHp(float value)
    {
        if(currentHealth == maxHealth) return;

        currentHealth += Mathf.Abs(value);
        if(currentHealth > maxHealth) currentHealth = maxHealth;
    }
    public void TakeHp(float value)
    {
        currentHealth -= Mathf.Abs(value);
        if(currentHealth <= 0) currentHealth = 0; 
    }
    public void SetPlayerHealth(PlayerHealth _playerHealth)
    {
        currentHealth = maxHealth;
        playerHealth = _playerHealth;
    }
    public void Restore() => currentHealth = maxHealth;
    private void OnDisable() {
        playerHealth = null;
    }
}
