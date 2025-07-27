using UnityEngine;

public class AutoDestroyAfterTime : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (ps != null && !ps.IsAlive()) Destroy(gameObject);
    }
}