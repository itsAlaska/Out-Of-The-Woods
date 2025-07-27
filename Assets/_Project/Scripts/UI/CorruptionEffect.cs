using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CorruptionEffect : MonoBehaviour
{
    [Header("Post-Processing")]
    [SerializeField] private Volume corruptionVolume;
    [SerializeField] private float corruptionDuration = 300f;

    private float timer;
    private bool isLocked = false;

    private ColorAdjustments colorAdjustments;
    private Vignette vignette;

    private void Start()
    {
        if (corruptionVolume != null)
        {
            corruptionVolume.profile.TryGet(out colorAdjustments);
            corruptionVolume.profile.TryGet(out vignette);
        }
    }

    private void Update()
    {
        if (isLocked || corruptionVolume == null) return;

        timer += Time.unscaledDeltaTime;
        float t = Mathf.Clamp01(timer / corruptionDuration);
        Debug.Log($"Corruption Progress: {t}");

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(0f, -80f, t);
            colorAdjustments.contrast.value = Mathf.Lerp(0f, 30f, t);
        }

        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(0f, 1f, t);        // Full screen blackout
            vignette.smoothness.value = Mathf.Lerp(0.4f, 1f, t);     // Sharper edges over time
            vignette.rounded.value = false;                          // More cinematic (optional)
        }
    }

    public void LockCorruptionToMax()
    {
        isLocked = true;

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = -80f;
            colorAdjustments.contrast.value = 30f;
        }

        if (vignette != null)
        {
            vignette.intensity.value = 1f;
            vignette.smoothness.value = 1f;
            vignette.rounded.value = false;
        }
    }
}