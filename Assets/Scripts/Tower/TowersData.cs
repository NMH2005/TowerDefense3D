using UnityEngine;

[CreateAssetMenu(fileName = "TowersData", menuName = "Scriptable Objects/TowersData")]
public class TowersData : ScriptableObject
{
    [Header("Info")]
    public string TowersName;
    public GameObject prefab;

    [Header("Weapon")]
    public GameObject WeaponPrefab;

    [Header("Cost")]
    public int buyCost;
    public int sellValue;

    [Header("Levels")]
    public TowerLevelData[] levels;
}
