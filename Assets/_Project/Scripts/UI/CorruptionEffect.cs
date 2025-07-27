using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CorruptionEffect : MonoBehaviour
{
    [Header("Post-Processing")]
    [SerializeField] private Volume corruptionVolume;
    [SerializeField] private float corruptionDuration = 300f;
    [SerializeField] private float corruptionStartTime = 90f; // 1:30 in seconds

    private float timer;
    private bool isLocked = false;
    private LayeredMusicManager musicManager;

    private ColorAdjustments colorAdjustments;
    private Vignette vignette;

    private void Start()
    {
        musicManager = FindObjectOfType<LayeredMusicManager>();
        if (corruptionVolume != null)
        {
            corruptionVolume.profile.TryGet(out colorAdjustments);
            corruptionVolume.profile.TryGet(out vignette);
        }
    }

    private void Update()
    {
        if (isLocked || corruptionVolume == null || GameOverManager.IsExternallyPaused)
            return;

        timer += Time.unscaledDeltaTime;

        if (timer < corruptionStartTime)
            return;

        float rawT = Mathf.Clamp01((timer - corruptionStartTime) / (corruptionDuration - corruptionStartTime));
        float easedT = Mathf.SmoothStep(0f, 1f, rawT);
        Debug.Log($"Corruption Progress: {easedT}");

        musicManager?.SetCorruptionProgress(easedT);

        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = Mathf.Lerp(0f, -80f, easedT);
            colorAdjustments.contrast.value = Mathf.Lerp(0f, 30f, easedT);
        }

        if (vignette != null)
        {
            vignette.intensity.value = Mathf.Lerp(0f, 1f, easedT);
            vignette.smoothness.value = Mathf.Lerp(0.4f, 1f, easedT);
            vignette.rounded.value = false;
        }

        // Fade in hospital beep starting at 4:30 (270s)
        if (timer >= 270f && musicManager != null)
        {
            float beepFadeT = Mathf.Clamp01((timer - 270f) / 30f);
            musicManager.SetHospitalBeepVolume(beepFadeT);
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