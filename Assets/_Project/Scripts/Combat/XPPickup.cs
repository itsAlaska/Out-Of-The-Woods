using UnityEngine;

public class XPPickup : MonoBehaviour
{
    [SerializeField] private int xpAmount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            XPManager.Instance?.AddXP(xpAmount);
            Destroy(gameObject);
        }
    }
}