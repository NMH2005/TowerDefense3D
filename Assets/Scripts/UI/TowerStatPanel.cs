using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerStatPanel : MonoBehaviour
{
    [SerializeField] private TowersData towerData;
    [SerializeField] private TextMeshProUGUI towerName;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI damage;
    [SerializeField] private TextMeshProUGUI fireRate;
    [SerializeField] private TextMeshProUGUI fireRange;
    [SerializeField] private TextMeshProUGUI buyCost;
    [SerializeField] private Button closeButton;
    [SerializeField] private UIStatsManager uiStatsManager;


    void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            uiStatsManager.OnPanelClosed();
            gameObject.SetActive(false);
        });
    }

    private void OnEnable()
    {
        EventManager.OnTowerPlaced += Hide;
    }

    private void OnDisable()
    {
        EventManager.OnTowerPlaced -= Hide;
    }


    private void Hide()
    {
        if (!gameObject.activeSelf) return;
        uiStatsManager.OnPanelClosed();
        gameObject.SetActive(false);
    }

    public void Show()
    {
        TowerLevelData lv = towerData.levels[0];
        towerName.text = towerData.TowersName;
        hp.text = $"HP: {lv.MaxHp}";
        damage.text = $"Damage: {lv.Damage}";
        fireRate.text = $"Fire Rate: {lv.FireRate:F1}/s";
        fireRange.text = $"Range: {lv.Range:F1}";
        buyCost.text = $"Cost: {towerData.buyCost}";
        gameObject.SetActive(true);
    }
}
