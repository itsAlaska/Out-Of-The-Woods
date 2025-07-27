using UnityEngine;

public class PickupCollector : MonoBehaviour
{
    [SerializeField] private LayerMask pickupLayer;
    [SerializeField] private float moveSpeed = 10f;

    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        float range = playerStats != null ? playerStats.pickupRange : 2f;

        Collider2D[] pickups = Physics2D.OverlapCircleAll(transform.position, range, pickupLayer);

        foreach (Collider2D pickup in pickups)
        {
            if (pickup.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 direction = (transform.position - pickup.transform.position).normalized;
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                pickup.transform.position = Vector2.MoveTowards(
                    pickup.transform.position,
                    transform.position,
                    moveSpeed * Time.deltaTime
                );
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        float range = playerStats != null ? playerStats.pickupRange : 2f;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}