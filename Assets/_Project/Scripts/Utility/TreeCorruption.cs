using System.Collections;
using UnityEngine;

public class TreeCorruption : MonoBehaviour
{
    [Header("Corruption Settings")] [SerializeField]
    private float minTime = 60f;

    [SerializeField] private float maxTime = 270f;
    [SerializeField] private float fadeDuration = 5f;

    private SpriteRenderer originalRenderer;
    private SpriteRenderer corruptedRenderer;

    private void Start()
    {
        originalRenderer = GetComponent<SpriteRenderer>();
        corruptedRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        // Ensure corrupted sprite starts transparent
        corruptedRenderer.color = new Color(1f, 1f, 1f, 0f);

        var delay = Random.Range(minTime, maxTime);
        StartCoroutine(CrossfadeSprites(delay));
    }

    private IEnumerator CrossfadeSprites(float delay)
    {
        yield return new WaitForSeconds(delay);

        var elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            var t = elapsed / fadeDuration;
            originalRenderer.color = new Color(1f, 1f, 1f, 1f - t);
            corruptedRenderer.color = new Color(1f, 1f, 1f, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final state
        originalRenderer.color = new Color(1f, 1f, 1f, 0f);
        corruptedRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}