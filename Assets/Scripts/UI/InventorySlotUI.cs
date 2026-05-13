using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private TowerStatPanel statsPanel;
    [SerializeField] private UIStatsManager uiStatsManager;
    [SerializeField] private TowersData towerData;
    [SerializeField] private TowerPlace towerPlacer;
    [SerializeField] private Color normalColor = new Color(0f, 0f, 0f, 0.7f);
    [SerializeField] private Color selectedColor = new Color(1f, 0.8f, 0.2f);

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        
        towerPlacer.StartPlacing(towerData);
uiStatsManager.ShowPanel(statsPanel, this);
    }

    public void SetSelected(bool selected)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = selected ? selectedColor : normalColor;
        button.colors = cb;
    }
}
