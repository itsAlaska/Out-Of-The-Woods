using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float projectileDamage = 10f;
    public float projectileSpeed = 10f;
    public float fireRate = 0.5f;
    public float fireRadius = 10f;
    public float pickupRange = 2f;

    public void ApplyStatModifier(string statName, float value)
    {
        switch (statName)
        {
            case "Health": maxHealth += value; break;
            case "MoveSpeed": moveSpeed += value; break;
            case "Damage": projectileDamage += value; break;
            case "FireRate": fireRate -= value; break; // lower = faster
            case "PickupRange": pickupRange += value; break;
            // Add more as needed
        }
    }
}