using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LayeredMusicManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainTheme;
    [SerializeField] private AudioClip corruptedTheme;
    [SerializeField] private AudioClip hospitalBeep;
    [SerializeField] private AudioClip flatline;

    [Header("Fade Settings")]
    [SerializeField] private float fadeOutSpeed = 0.5f;

    private AudioSource mainSource;
    private AudioSource corruptedSource;
    private AudioSource sfxSource;

    private void Awake()
    {
        // Create and configure audio sources
        mainSource = gameObject.AddComponent<AudioSource>();
        corruptedSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        // Loop background music
        mainSource.loop = true;
        corruptedSource.loop = true;

        // Assign clips
        mainSource.clip = mainTheme;
        corruptedSource.clip = corruptedTheme;

        // Initial volumes
        mainSource.volume = 1f;
        corruptedSource.volume = 0f;

        // Start music
        mainSource.Play();
        corruptedSource.Play();
    }

    // Gradually blend from main to corrupted based on corruption % (0 to 1)
    public void SetCorruptionProgress(float t)
    {
        mainSource.volume = 1f - t;
        corruptedSource.volume = t;
    }

    // When timer hits 5 minutes: fade out music, trigger flatline sfx
    public void TriggerFlatline()
    {
        FadeOutAll();
        sfxSource.loop = false;
        sfxSource.clip = flatline;
        sfxSource.PlayDelayed(0.5f); // Dramatic pause
    }

    // When player dies before timer ends: fade out everything silently
    public void FadeOutAll()
    {
        StartCoroutine(FadeOutSource(mainSource));
        StartCoroutine(FadeOutSource(corruptedSource));
    }

    private IEnumerator FadeOutSource(AudioSource source)
    {
        float startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= Time.unscaledDeltaTime * fadeOutSpeed;
            source.volume = Mathf.Max(source.volume, 0f);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset for future reuse if needed
    }
}
