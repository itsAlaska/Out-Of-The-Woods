using UnityEngine;

public class AutoFire : MonoBehaviour
{
    [Header("Firing")] [SerializeField] private GameObject projectilePrefab;
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
        var hits = Physics2D.OverlapCircleAll(transform.position, playerStats.fireRadius, enemyLayers);

        Transform closestEnemy = null;
        var closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            var dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestEnemy = hit.transform;
            }
        }

        if (closestEnemy == null) return;

        Vector2 direction = (closestEnemy.position - firePoint.position).normalized;

        var projectileGO = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        var projectile = projectileGO.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.SetDirection(direction);
            projectile.Damage = Mathf.RoundToInt(playerStats.projectileDamage);
            projectile.Speed = playerStats.projectileSpeed;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerStats.fireRadius);
    }
}