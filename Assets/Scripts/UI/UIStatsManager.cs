using UnityEngine;

public class UIStatsManager : MonoBehaviour {
    [SerializeField] private InventorySlotUI[] slotButtons;

    private void OnEnable()
    {
        EventManager.OnTowerSelectionChanged += HandleSelectionChanged;
    }

    private void OnDisable()
    {
        EventManager.OnTowerSelectionChanged -= HandleSelectionChanged;
    }

    private void HandleSelectionChanged(TowersData data)
    {
        if (data == null)
        {
            HideAll();
            return;
        }

        foreach (var slot in slotButtons)
        {
            if (slot.TowerData != data) continue;

            HideAll();
            slot.StatsPanel.Show();
            slot.SetSelected(true);
            return;
        }
    }

    private void HideAll()
    {
        foreach (var slot in slotButtons)
        {
            slot.StatsPanel.Hide();
            slot.SetSelected(false);
        }
    }
}