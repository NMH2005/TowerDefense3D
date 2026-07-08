using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerSelecter : MonoBehaviour
{
    [SerializeField] private TowerUIPanel towerUIPanel;
    [SerializeField] private TowerPlace towerPlace;
    [SerializeField] private LayerMask towerLayer;

    private void Update()
    {
        if(Mouse.current == null || !Mouse.current.leftButton.wasPressedThisFrame) {
            return;
        }

        if (towerPlace != null && towerPlace.IsPlacing) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, towerLayer))
        {
            TowerManager tower = hit.collider.GetComponentInParent<TowerManager>();
            if (tower != null)
            {
                towerUIPanel.Show(tower);
                return;
            }
        }

        towerUIPanel.Hide();
    }
}
