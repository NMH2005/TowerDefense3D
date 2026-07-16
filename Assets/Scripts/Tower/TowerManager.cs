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

    private GameObject weaponInstance;

    public void RegisterWeapon(GameObject weapon)
    {
        weaponInstance = weapon;
    }

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

        RepositionWeapon();

        foreach (var weapon in GetComponentsInChildren<WeaponAttack>())
            weapon.ApplyStats(levelData);

        foreach (var wb in GetComponentsInChildren<WeaponBase>())
            wb.ApplyStats(levelData);

        EventManager.RaiseLevelApplied(this, levelData);
    }

    private void RepositionWeapon()
    {
        if (weaponInstance == null) return;

        Transform activePlace = FindActiveWeaponPlace();
        if (activePlace == null) return;

        weaponInstance.transform.SetParent(activePlace, false);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;
    }

    private Transform FindActiveWeaponPlace()
    {
        GameObject[] partsHighToLow = { buildPrefab, middlePrefab, bottomPrefab, basePrefab };

        foreach (var part in partsHighToLow)
        {
            if (part == null || !part.activeInHierarchy) continue;

            foreach (var t in part.GetComponentsInChildren<Transform>(true))
            {
                if (t.name == "WeaponPlace")
                    return t;
            }
        }
        return null;
    }

    public bool HasNextLevel => towerData != null && CurrentLevelIndex + 1 < towerData.levels.Length;

    public TowerLevelData GetNextLevelData()
    {
        return HasNextLevel ? towerData.levels[CurrentLevelIndex + 1] : null;
    }

    public void SetTargetMode(TargetMode mode)
    {
        foreach (var wb in GetComponentsInChildren<WeaponBase>())
            wb.SetTargetMode(mode);
    }

    public TargetMode GetTargetMode()
    {
        WeaponBase wb = GetComponentInChildren<WeaponBase>();
        return wb != null ? wb.GetTargetMode() : TargetMode.First;
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