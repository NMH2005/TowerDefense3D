using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static event Action OnTowerPlaced;
    public static event Action<TowerManager> OnTowerUpgraded;

    public static void RaiseTowerPlaced( )
    {
        OnTowerPlaced?.Invoke();
    }

    public static void RaiseTowerUpgraded(TowerManager tower)
    {
        OnTowerUpgraded?.Invoke(tower);
    }
}
