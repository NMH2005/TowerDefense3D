using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour {
    [SerializeField] private TowerStatPanel statsPanel;
    [SerializeField] private TowersData towerData;
    [SerializeField] private TowerPlace towerPlacer;
    [SerializeField] private Color normalColor = new Color(0f, 0f, 0f, 0.7f);
    [SerializeField] private Color selectedColor = new Color(1f, 0.8f, 0.2f);

    private Button button;

    public TowersData TowerData => towerData;
    public TowerStatPanel StatsPanel => statsPanel;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {

        towerPlacer.StartPlacing(towerData);
    }

    public void SetSelected(bool selected)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = selected ? selectedColor : normalColor;
        button.colors = cb;
    }
}