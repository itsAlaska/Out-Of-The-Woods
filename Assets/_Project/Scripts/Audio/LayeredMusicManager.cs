using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LayeredMusicManager : MonoBehaviour
{
    [Header("Audio Clips")] [SerializeField]
    private AudioClip mainTheme;

    [SerializeField] private AudioClip corruptedTheme;
    [SerializeField] private AudioClip hospitalBeep;
    [SerializeField] private AudioClip flatline;

    [Header("Fade Settings")] [SerializeField]
    private float fadeOutSpeed = 0.5f;

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
        mainSource.volume = .5f;
        corruptedSource.volume = 0f;

        // Start music
        mainSource.Play();
        corruptedSource.Play();
    }

    // Gradually blend from main to corrupted based on corruption % (0 to 1)
    public void SetCorruptionProgress(float timeElapsed)
    {
        var mainVolume = 0f;
        var corruptedVolume = 0f;
        var beepVolume = 0f;

        if (timeElapsed < 90f)
        {
            mainVolume = .5f;
        }
        else if (timeElapsed < 270f)
        {
            var t = Mathf.InverseLerp(90f, 270f, timeElapsed);
            var fadeT = Mathf.Pow(t, 2.5f);

            mainVolume = Mathf.Lerp(0.5f, 0f, fadeT);
            corruptedVolume = Mathf.Lerp(0f, 1f, t);
        }
        else if (timeElapsed < 300f)
        {
            var t = Mathf.InverseLerp(270f, 300f, timeElapsed);
            corruptedVolume = Mathf.Lerp(1f, 0f, t);
            beepVolume = Mathf.Lerp(0f, 0.5f, t);
        }
        else
        {
            beepVolume = 0f;
        }

        mainSource.volume = mainVolume;
        corruptedSource.volume = corruptedVolume;
        sfxSource.volume = beepVolume;

        Debug.Log($"Main Volume Set To: {mainVolume} at timeElapsed: {timeElapsed}");
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

    public void SetHospitalBeepVolume(float t)
    {
        if (sfxSource.clip != hospitalBeep)
        {
            sfxSource.clip = hospitalBeep;
            sfxSource.loop = true;
            sfxSource.Play();
        }

        sfxSource.volume = Mathf.Lerp(0f, 1f, t);
    }

    private IEnumerator FadeOutSource(AudioSource source)
    {
        var startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= Time.unscaledDeltaTime * fadeOutSpeed;
            source.volume = Mathf.Max(source.volume, 0f);
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // Reset for future reuse if needed
    }
    
    public void FadeOutFlatline(float duration)
    {
        StartCoroutine(FadeOutSFXOverTime(duration));
    }

    private IEnumerator FadeOutSFXOverTime(float duration)
    {
        float startVolume = sfxSource.volume;
        float timer = 0f;

        while (timer < duration)
        {
            sfxSource.volume = Mathf.Lerp(startVolume, 0f, timer / duration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        sfxSource.volume = 0f;
        sfxSource.Stop(); // Optional
    }

}