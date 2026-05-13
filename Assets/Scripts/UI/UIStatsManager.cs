using UnityEngine;

public class UIStatsManager : MonoBehaviour {
    [SerializeField] private TowerStatPanel[] panels;
    [SerializeField] private InventorySlotUI[] slotButtons;

    private InventorySlotUI currentSelected;

    public void ShowPanel(TowerStatPanel panel, InventorySlotUI caller)
    {
        foreach (var p in panels)
            p.gameObject.SetActive(false);

        foreach (var b in slotButtons)
            b.SetSelected(false);

        panel.Show();
        caller.SetSelected(true);
        currentSelected = caller;
    }

    public void OnPanelClosed()
    {
        if (currentSelected != null)
            currentSelected.SetSelected(false);
        currentSelected = null;
    }
}