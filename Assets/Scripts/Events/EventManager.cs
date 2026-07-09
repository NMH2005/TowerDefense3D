using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static event Action OnTowerPlaced;
    public static event Action<TowerManager> OnTowerUpgraded;
    public static event Action<TowerManager> OnTowerSold;
    public static event Action<TowerManager, TowerLevelData> OnLevelApplied;
    public static event Action<int> OnGoldChanged;
    public static void RaiseTowerPlaced( )
    {
        OnTowerPlaced?.Invoke();
    }

    public static void RaiseTowerUpgraded(TowerManager tower)
    {
        OnTowerUpgraded?.Invoke(tower);
    }

    public static void RaiseLevelApplied(TowerManager tower, TowerLevelData levelData)
    {
        OnLevelApplied?.Invoke(tower, levelData);
    }

    public static void RaiseGoldChanged(int gold)
    {
        OnGoldChanged?.Invoke(gold);
    }

    public static void RaiseTowerSold(TowerManager tower)
    {
        OnTowerSold?.Invoke(tower);
    }
}
