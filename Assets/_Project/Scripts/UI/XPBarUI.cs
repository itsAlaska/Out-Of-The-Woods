using UnityEngine;
using UnityEngine.UI;

public class XPBarUI : MonoBehaviour
{
    [SerializeField] private Slider xpSlider;
    private XPManager xpManager;

    private void Start()
    {
        xpManager = FindObjectOfType<XPManager>();
        if (xpManager == null)
        {
            Debug.LogWarning("No XPManager found in scene.");
            enabled = false;
            return;
        }

        UpdateXP(); // Initialize
    }

    private void Update()
    {
        UpdateXP();
    }

    private void UpdateXP()
    {
        xpSlider.maxValue = xpManager.xpToNextLevel;
        xpSlider.value = xpManager.currentXP;
    }
}