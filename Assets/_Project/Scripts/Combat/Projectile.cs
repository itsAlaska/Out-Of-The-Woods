using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private LayerMask targetLayers;

    public float Speed { get; set; }
    public int Damage { get; set; }

    private PlayerStats playerStats;
    private Vector2 direction;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void FixedUpdate()
    {
        transform.Translate(direction * Speed * Time.fixedDeltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayers) != 0)
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Damaging enemy.");
                enemy.TakeDamage(Damage);
            }

            var ps = GetComponentInChildren<ParticleSystem>();
            ps.transform.parent = null;
            ps.Stop();
            Destroy(gameObject);
        }
    }
}