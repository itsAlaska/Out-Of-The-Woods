using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null) 
        {
            Debug.LogWarning("No PlayerHealth found in scene.");
            enabled = false;
            return;
        }

        UpdateHealth(); // Initialize
    }

    private void Update()
    {
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        healthSlider.maxValue = playerHealth.GetMaxHealth();
        healthSlider.value = playerHealth.GetCurrentHealth();
    }
}