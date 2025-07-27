using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [SerializeField] private float maxTime = 300f;

    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private LayeredMusicManager musicManager;

    public float ElapsedTime { get; private set; } = 0f;
    public bool IsFinished => ElapsedTime >= maxTime;

    private bool hasTriggeredEnd = false;

    private void Update()
    {
        if (hasTriggeredEnd) return;

        ElapsedTime += Time.deltaTime;
        ElapsedTime = Mathf.Min(ElapsedTime, maxTime);

        int minutes = Mathf.FloorToInt(ElapsedTime / 60f);
        int seconds = Mathf.FloorToInt(ElapsedTime % 60f);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";

        // Check for timer-based ending
        if (ElapsedTime >= maxTime)
        {
            hasTriggeredEnd = true;

            if (gameOverManager != null)
                gameOverManager.TriggerGameOver();

            if (musicManager != null)
                musicManager.TriggerFlatline();
        }
    }
}