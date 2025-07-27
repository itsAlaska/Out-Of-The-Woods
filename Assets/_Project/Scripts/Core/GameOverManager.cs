using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverScreen;

    [Header("External References")]
    [SerializeField] private CorruptionEffect corruptionEffect;
    [SerializeField] private LayeredMusicManager musicManager;
    [SerializeField] private GameTimer gameTimer;

    private bool isGameOver = false;
    public static bool IsExternallyPaused { get; private set; } = false;

    private void Awake()
    {
        // Optional: fallback auto-assignment if left empty in Inspector
        if (gameTimer == null)
            gameTimer = FindObjectOfType<GameTimer>();

        if (musicManager == null)
            musicManager = FindObjectOfType<LayeredMusicManager>();

        if (corruptionEffect == null)
            corruptionEffect = FindObjectOfType<CorruptionEffect>();
    }

    public void TriggerGameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("Game Over triggered");

        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);
        else
            Debug.LogWarning("GameOverScreen is not assigned!");

        Time.timeScale = 0f;

        // Lock corruption visual effect
        if (corruptionEffect != null)
            corruptionEffect.LockCorruptionToMax();
        else
            Debug.LogWarning("CorruptionEffect is not assigned!");

        // Handle music transition
        if (musicManager != null)
        {
            if (gameTimer != null && gameTimer.IsFinished)
            {
                musicManager.TriggerFlatline();
            }
            else
            {
                musicManager.FadeOutAll();
            }
        }
        else
        {
            Debug.LogWarning("MusicManager is not assigned!");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public static void SetExternalPause(bool isPaused)
    {
        IsExternallyPaused = isPaused;
    }
}