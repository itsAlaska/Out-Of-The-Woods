using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("Stats")] [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private int xpReward = 1;
    [SerializeField] private GameObject xpPickupPrefab;
    [SerializeField] private Transform spriteChild;

    private Transform player;
    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private Coroutine flashRoutine;
    private AudioSource audioSource;


    private void Start()
    {
        currentHealth = maxHealth;

        if (spriteChild != null)
        {
            spriteRenderer = spriteChild.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                originalColor = spriteRenderer.color;
            else
                Debug.LogWarning("SpriteRenderer not found on spriteChild!");
        }
        else
        {
            Debug.LogWarning("spriteChild is not assigned on " + gameObject.name);
        }

        audioSource = GetComponent<AudioSource>();

        var playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }


    private void FixedUpdate()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        transform.position += (Vector3)(direction * moveSpeed * Time.fixedDeltaTime);

        // Flip sprite based on horizontal direction
        if (spriteChild != null)
        {
            if (direction.x > 0.01f)
                spriteChild.localScale = new Vector3(-1f, 1f, 1f); // Facing right
            else if (direction.x < -0.01f)
                spriteChild.localScale = new Vector3(1f, 1f, 1f); // Facing left
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enemy collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            var playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                Debug.Log("Damaging player!");
                playerHealth.TakeDamage(1);
            }
        }
    }


    public void TakeDamage(int damage)
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashDamage());

        currentHealth -= damage;
        if (currentHealth <= 0) Die();
    }


    private void Die()
    {
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        if (XPManager.Instance != null)
            XPManager.Instance.AddXP(xpReward);

        if (xpPickupPrefab != null)
            Instantiate(xpPickupPrefab, transform.position, Quaternion.identity);

        // Disable visuals & collisions right away
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;

        StartCoroutine(DestroyAfterSound());
    }

    private IEnumerator DestroyAfterSound()
    {
        yield return new WaitForSeconds(deathSound.length);
        Destroy(gameObject);
    }

    private IEnumerator FlashDamage()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = originalColor;
        }
    }
}