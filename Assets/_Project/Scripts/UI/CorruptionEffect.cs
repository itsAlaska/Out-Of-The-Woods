using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CorruptionEffect : MonoBehaviour
{
    [Header("Post-Processing")]
    [SerializeField] private Volume corruptionVolume;

    [Header("Final Ending UI")]
    [SerializeField] private CanvasGroup endingCanvasGroup;
    [SerializeField] private TMPro.TextMeshProUGUI endingText;

    [Header("Corruption Timing")]
    [SerializeField] private float corruptionDuration = 300f;
    [SerializeField] private float corruptionStartTime = 90f; // 1:30 in seconds

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 5f;
    [SerializeField] private float textDelay = 1.5f;
    [SerializeField] private float textFadeDuration = 2f;
    
    [SerializeField] private GameObject finalFadeCanvasObject; 

    private float timer;
    private bool isLocked = false;
    private bool isEnding = false;

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

        // Ensure UI starts fully invisible
        if (endingCanvasGroup != null)
            endingCanvasGroup.alpha = 0f;

        if (endingText != null)
        {
            endingText.alpha = 0f;
            endingText.gameObject.SetActive(false);
        }
        
        if (finalFadeCanvasObject != null)
            finalFadeCanvasObject.SetActive(false); // Hide it at game start

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // T for Test
        {
            LockCorruptionToMax();
            musicManager?.TriggerFlatline();
            StartCoroutine(ShowFinalEnding());
            isEnding = true;
        }
        
        if (isLocked || corruptionVolume == null || GameOverManager.IsExternallyPaused)
            return;

        timer += Time.unscaledDeltaTime;

        if (timer < corruptionStartTime)
            return;

        float rawT = Mathf.Clamp01((timer - corruptionStartTime) / (corruptionDuration - corruptionStartTime));
        float easedT = Mathf.Pow(rawT, 1.75f);
        Debug.Log($"Corruption Progress: {easedT}");

        musicManager?.SetCorruptionProgress(timer);

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
        if (timer >= 270f)
        {
            float beepFadeT = Mathf.Clamp01((timer - 270f) / 30f);
            musicManager?.SetHospitalBeepVolume(beepFadeT);
        }

        // Trigger ending sequence at 5:00
        if (!isEnding && timer >= 300f)
        {
            LockCorruptionToMax(); // snap visuals
            musicManager?.TriggerFlatline();
            StartCoroutine(ShowFinalEnding());
            isEnding = true;
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

    [SerializeField] private CanvasGroup endingButtonsGroup; // Assign in Inspector

    private System.Collections.IEnumerator ShowFinalEnding()
    {
        if (finalFadeCanvasObject != null)
            finalFadeCanvasObject.SetActive(true); // Make it visible for the fade

        
        // Freeze the game immediately
        Time.timeScale = 0f;
        GameOverManager.SetExternalPause(true);

        float timer = 0f;

        // Fade in white screen
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            if (endingCanvasGroup != null)
                endingCanvasGroup.alpha = t;

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        if (endingCanvasGroup != null)
            endingCanvasGroup.alpha = 1f;

        // Wait before showing the text
        yield return new WaitForSecondsRealtime(textDelay);

        if (endingText != null)
        {
            endingText.text = "The world didn’t end. Just your place in it. Maybe that’s enough... To know you were here, even if only for a while...";
            endingText.gameObject.SetActive(true); // ← ADD THIS LINE

            float textT = 0f;
            while (textT < 1f)
            {
                endingText.alpha = textT;
                textT += Time.unscaledDeltaTime / textFadeDuration;
                yield return null;
            }
            endingText.alpha = 1f;
        }

        // Wait a moment before showing buttons
        yield return new WaitForSecondsRealtime(1f);

        // Fade in retry/quit buttons
        if (endingButtonsGroup != null)
        {
            float buttonT = 0f;
            while (buttonT < 1f)
            {
                endingButtonsGroup.alpha = buttonT;
                buttonT += Time.unscaledDeltaTime / 1.5f;
                yield return null;
            }
            endingButtonsGroup.alpha = 1f;
            endingButtonsGroup.interactable = true;
            endingButtonsGroup.blocksRaycasts = true;
        }
        
        musicManager?.FadeOutFlatline(5f);
        endingButtonsGroup.interactable = true;
        endingButtonsGroup.blocksRaycasts = true;

    }
}
