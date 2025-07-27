using UnityEngine;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;

    [Header("XP Settings")]
    public int currentXP = 0;
    public int level = 1;
    public int xpToNextLevel = 10;
    public float xpGrowthFactor = 1.5f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (!FindObjectOfType<UpgradeManager>())
            Debug.LogWarning("No UpgradeManager found in scene!");
        
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthFactor);
        Debug.Log($"Level Up! Now level {level}");

        FindObjectOfType<UpgradeManager>()?.ShowUpgradeChoices();
    }

}