using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private LayerMask targetLayers;

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
        transform.Translate(direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayers) != 0)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("Damaging enemy.");
                enemy.TakeDamage(1);
            }
            ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
            ps.transform.parent = null;
            ps.Stop();
            Destroy(gameObject);
        }
    }

}