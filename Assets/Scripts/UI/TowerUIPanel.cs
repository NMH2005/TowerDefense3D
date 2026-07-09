using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUIPanel : MonoBehaviour {
    [SerializeField] private Image Icon;
    [SerializeField] private TMP_Text towerNameText;
    [SerializeField] private TMP_Text levelCurrentText;
    [SerializeField] private TMP_Text levelNextText;
    [SerializeField] private TMP_Text damageCurrentText;
    [SerializeField] private TMP_Text damageNextText;
    [SerializeField] private TMP_Text RateCurrentText;
    [SerializeField] private TMP_Text RateNextText;
    [SerializeField] private TMP_Text CDCurrentText;
    [SerializeField] private TMP_Text CDNextText;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button sellButton;
    [SerializeField] private TMP_Text sellText;
    [SerializeField] private TMP_Text upgradeText;
    [SerializeField] private TMP_Text upgradeCostText;

    private TowerManager currentTower;

    private void Awake()
    {
        gameObject.SetActive(false);
        if (upgradeButton != null)
        {
            upgradeButton.onClick.AddListener(OnUpgradeClicked);
        }

        if(sellButton != null)
        {
            sellButton.onClick.AddListener(OnSellClicked);
        }
    }

    private void OnEnable()
    {
        EventManager.OnGoldChanged += OnGoldChanged;
    }

    private void OnDisable()
    {
        EventManager.OnGoldChanged -= OnGoldChanged;
    }

    private void OnGoldChanged(int gold)
    {
        Refresh();
    }

    public void Show(TowerManager tower)
    {
        if (tower == null) return;
        currentTower = tower;
        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        currentTower = null;
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
        if (currentTower == null || currentTower.TowerData == null || currentTower.CurrentLevelData == null) return;

        TowersData data = currentTower.TowerData;
        TowerLevelData current = currentTower.CurrentLevelData;
        TowerLevelData next = currentTower.GetNextLevelData();
        bool hasNext = next != null;

        if (Icon != null) Icon.sprite = data.Icon;
        if (towerNameText != null) towerNameText.text = $"{data.TowersName} ({currentTower.CurrentLevelIndex + 1})";

        SetPair(levelCurrentText, levelNextText, hasNext, (currentTower.CurrentLevelIndex + 1).ToString(), hasNext ? (currentTower.CurrentLevelIndex + 2).ToString() : null);

        SetPair(damageCurrentText, damageNextText, hasNext, current.Damage.ToString(), hasNext ? next.Damage.ToString() : null);

        SetPair(RateCurrentText, RateNextText, hasNext, current.FireRate.ToString("0.##"), hasNext ? next.FireRate.ToString("0.##") : null);

        SetPair(CDCurrentText, CDNextText, hasNext, GetCooldown(current).ToString("0.00"), hasNext ? GetCooldown(next).ToString("0.00") : null);

        bool canAfford = hasNext && (EconomyManager.Instance == null || EconomyManager.Instance.CanAfford(next.UpgradeCost));

        if (upgradeButton != null)
            upgradeButton.interactable = canAfford;

        if (upgradeText != null)
            upgradeText.text = hasNext ? $"Upgrade: ${next.UpgradeCost}" : "Max";

        if (sellText != null)
            sellText.text = $"Sell: ${current.SellValue}";
    }

    private float GetCooldown(TowerLevelData levelData)
    {
        return levelData.FireRate > 0f ? 1f / levelData.FireRate : 0f;
    }

    private void SetPair(TMP_Text currentField, TMP_Text nextField, bool hasNext, string currentValue, string nextValue)
    {
        if (currentField != null) currentField.text = currentValue;
        if (nextField != null)
        {
            nextField.gameObject.SetActive(hasNext);
            if (hasNext)
            {
                nextField.text = nextValue;
            }
        }
    }

    private void OnUpgradeClicked()
    {
        if (currentTower == null || !currentTower.HasNextLevel) return;

        int cost = currentTower.GetNextLevelData().UpgradeCost;
        if (EconomyManager.Instance != null && !EconomyManager.Instance.TrySpend(cost))
        {
            return;
        }

        if (currentTower.TryUpgrade()) Refresh();
    }

    private void OnSellClicked()
    {
        if (currentTower == null || currentTower.CurrentLevelData == null) return;
        int sellValue = currentTower.CurrentLevelData.SellValue;

        if(EconomyManager.Instance != null)
        {
            EconomyManager.Instance.AddGold(sellValue);
        }

        EventManager.RaiseTowerSold(currentTower);
        Destroy(currentTower.gameObject);
        Hide();
    }
}
