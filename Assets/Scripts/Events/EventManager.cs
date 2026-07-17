using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
    public static event Action OnTowerPlaced;
    public static event Action<TowerManager> OnTowerUpgraded;
    public static event Action<TowerManager> OnTowerSold;
    public static event Action<TowerManager, TowerLevelData> OnLevelApplied;
    public static event Action<int> OnGoldChanged;
    public static event Action<EnemyManager> OnEnemyKilled;
    public static event Action<EnemyManager> OnEnemyReachedEnd;
    public static event Action<int> OnLivesChanged;
    public static event Action OnGameOver;
    public static event Action<TowersData> OnTowerSelectionChanged;
    public static event Action<int> OnWaveChanged;
    public static event Action OnAllWavesCompleted;

    public static void RaiseTowerPlaced()
    {
        OnTowerPlaced?.Invoke();
    }

    public static void RaiseTowerUpgraded(TowerManager tower)
    {
        OnTowerUpgraded?.Invoke(tower);
    }

    public static void RaiseTowerSold(TowerManager tower)
    {
        OnTowerSold?.Invoke(tower);
    }

    public static void RaiseLevelApplied(TowerManager tower, TowerLevelData levelData)
    {
        OnLevelApplied?.Invoke(tower, levelData);
    }

    public static void RaiseGoldChanged(int gold)
    {
        OnGoldChanged?.Invoke(gold);
    }

    public static void RaiseEnemyKilled(EnemyManager enemy)
    {
        OnEnemyKilled?.Invoke(enemy);
    }

    public static void RaiseEnemyReachedEnd(EnemyManager enemy)
    {
        OnEnemyReachedEnd?.Invoke(enemy);
    }

    public static void RaiseLivesChanged(int lives)
    {
        OnLivesChanged?.Invoke(lives);
    }

    public static void RaiseGameOver()
    {
        OnGameOver?.Invoke();
    }

    public static void RaiseTowerSelectionChanged(TowersData data)
    {
        OnTowerSelectionChanged?.Invoke(data);
    }

    public static void RaiseWaveChanged(int waveNumber)
    {
        OnWaveChanged?.Invoke(waveNumber);
    }

    public static void RaiseAllWavesCompleted()
    {
        OnAllWavesCompleted?.Invoke();
    }
}