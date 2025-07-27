using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")] [SerializeField] private GameObject gameOverScreen;

    [Header("External References")] [SerializeField]
    private CorruptionEffect corruptionEffect;

    [SerializeField] private LayeredMusicManager musicManager;
    [SerializeField] private GameTimer gameTimer;

    private bool isGameOver = false;
    public static bool IsExternallyPaused { get; private set; } = false;
    public static GameOverManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

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
        IsExternallyPaused = true;

        if (corruptionEffect != null)
            corruptionEffect.LockCorruptionToMax();
        else
            Debug.LogWarning("CorruptionEffect is not assigned!");

        if (musicManager != null)
        {
            if (gameTimer != null && gameTimer.IsFinished)
                musicManager.TriggerFlatline();
            else
                musicManager.FadeOutAll();
        }
        else
        {
            Debug.LogWarning("MusicManager is not assigned!");
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        IsExternallyPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Retry()
    {
        RestartGame();
        // Alias for clarity in UI
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        IsExternallyPaused = false;

#if UNITY_EDITOR
        // If you're running in the Unity Editor, stop play mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
    // If you're running a build, quit the application
    Application.Quit();
#endif
    }


    public static void SetExternalPause(bool isPaused)
    {
        IsExternallyPaused = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }
}