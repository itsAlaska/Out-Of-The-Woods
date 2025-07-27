using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    private UpgradeData upgradeData;
    private UpgradeManager manager;

    public void Setup(UpgradeData data, UpgradeManager mgr)
    {
        upgradeData = data;
        manager = mgr;

        icon.sprite = data.icon;
        nameText.text = data.upgradeName;
        descriptionText.text = data.description;

        // Force the layout to rebuild immediately
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    public void OnSelect()
    {
        if (upgradeData != null && manager != null)
        {
            manager.SelectUpgrade(upgradeData);
        }
    }
}