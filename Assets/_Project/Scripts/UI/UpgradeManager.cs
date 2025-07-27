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
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}