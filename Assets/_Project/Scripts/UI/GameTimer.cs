using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [SerializeField] private float maxTime = 300f;

    public float ElapsedTime { get; private set; } = 0f;
    public bool IsFinished => ElapsedTime >= maxTime;

    private void Update()
    {
        ElapsedTime += Time.deltaTime;
        ElapsedTime = Mathf.Min(ElapsedTime, maxTime);

        int minutes = Mathf.FloorToInt(ElapsedTime / 60f);
        int seconds = Mathf.FloorToInt(ElapsedTime % 60f);

        if (timerText != null)
            timerText.text = $"{minutes:00}:{seconds:00}";
    }
}