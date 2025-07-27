using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverScreen;

    private bool isGameOver = false;

    public void TriggerGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over triggered");
        
        if (gameOverScreen != null)
            gameOverScreen.SetActive(true);

        Time.timeScale = 0f; // Freeze time
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}