using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private UpgradeCardUI[] upgradeCards;
    [SerializeField] private List<UpgradeData> allUpgrades;
    private PlayerStats playerStats;

    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        upgradePanel.SetActive(false);
    }

    public void ShowUpgradeChoices()
    {
        Time.timeScale = 0f;
        GameOverManager.SetExternalPause(true); 
        upgradePanel.SetActive(true);

        // Randomly pick upgrades
        List<UpgradeData> choices = new List<UpgradeData>();
        while (choices.Count < upgradeCards.Length)
        {
            var candidate = allUpgrades[Random.Range(0, allUpgrades.Count)];
            if (!choices.Contains(candidate))
                choices.Add(candidate);
        }

        // Assign data to each card
        for (int i = 0; i < upgradeCards.Length; i++)
        {
            upgradeCards[i].Setup(choices[i], this);
        }
    }

    public void SelectUpgrade(UpgradeData data)
    {
        playerStats.ApplyStatModifier(data.statToModify, data.value);

        if (data.statToModify == "Health")
        {
            PlayerHealth health = playerStats.GetComponent<PlayerHealth>();
            if (health != null)
            {
                int healAmount = Mathf.RoundToInt(health.GetMaxHealth());
                health.Heal(healAmount);
            }
        }

        upgradePanel.SetActive(false);
        GameOverManager.SetExternalPause(false); 
        Time.timeScale = 1f;
    }

}