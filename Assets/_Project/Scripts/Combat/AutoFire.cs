using UnityEngine;

public class AutoFire : MonoBehaviour
{
    [Header("Firing")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private LayerMask enemyLayers;

    private float fireTimer;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }
    
    private void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= playerStats.fireRate)
        {
            FireAtNearestEnemy();
            fireTimer = 0f;
        }
    }

    private void FireAtNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, playerStats.fireRadius, enemyLayers);


        Transform closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = hit.transform;
            }
        }

        if (closestEnemy == null) return; // ✅ No enemy? Don't fire.

        Vector2 direction = (closestEnemy.position - firePoint.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * 10f; // Adjust speed as needed
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerStats.fireRadius);
    }
}