using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerStats))]
public class PlayerHealth : MonoBehaviour
{
    private PlayerStats stats;

    private int currentHealth;

    [Header("Events")] public UnityEvent onDeath;
    public UnityEvent onDamageTaken;

    private void Awake()
    {
        stats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        currentHealth = Mathf.RoundToInt(stats.maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        onDamageTaken?.Invoke();

        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        FindObjectOfType<GameOverManager>()?.TriggerGameOver();
        gameObject.SetActive(false);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return Mathf.RoundToInt(stats.maxHealth);
    }

    // Optional: Add healing logic
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, Mathf.RoundToInt(stats.maxHealth));
    }

    // Optional: Reset full health (for respawn or upgrade)
    public void RestoreFullHealth()
    {
        currentHealth = Mathf.RoundToInt(stats.maxHealth);
    }
}