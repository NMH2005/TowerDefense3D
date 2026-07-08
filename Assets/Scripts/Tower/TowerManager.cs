using System;
using UnityEngine;

public class TowerManager : MonoBehaviour {
    [SerializeField] private GameObject basePrefab;
    [SerializeField] private GameObject bottomPrefab;
    [SerializeField] private GameObject middlePrefab;
    [SerializeField] private GameObject buildPrefab;

    private TowersData towerData;
    public TowersData TowerData => towerData;

    public int CurrentLevelIndex { get; private set; }
    public TowerLevelData CurrentLevelData { get; private set; }

    public event Action<TowerLevelData> OnLevelApplied;

    public void Initialize(TowersData data, int startLevelIndex = 0)
    {
        towerData = data;
        CurrentLevelIndex = startLevelIndex;
        ApplyLevel(data.levels[startLevelIndex]);
    }

    public void ApplyLevel(TowerLevelData levelData)
    {
        basePrefab.SetActive(levelData.enableBase);
        bottomPrefab.SetActive(levelData.enableBottom);
        middlePrefab.SetActive(levelData.enableMiddle);
        buildPrefab.SetActive(levelData.enableBuild);

        CurrentLevelData = levelData;

        foreach (var weapon in GetComponentsInChildren<WeaponAttack>())
            weapon.ApplyStats(levelData);

        foreach (var wb in GetComponentsInChildren<WeaponBase>())
            wb.ApplyStats(levelData);


        OnLevelApplied?.Invoke(levelData);
    }

    public bool HasNextLevel => towerData != null && CurrentLevelIndex + 1 < towerData.levels.Length;

    public TowerLevelData GetNextLevelData()
    {
        return HasNextLevel ? towerData.levels[CurrentLevelIndex + 1] : null;
    }


    public bool TryUpgrade()
    {
        if (!HasNextLevel) return false;

        CurrentLevelIndex++;
        ApplyLevel(towerData.levels[CurrentLevelIndex]);
        EventManager.RaiseTowerUpgraded(this);
        return true;
    }
}