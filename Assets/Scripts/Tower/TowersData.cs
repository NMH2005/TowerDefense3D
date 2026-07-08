using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TowersData", menuName = "Scriptable Objects/TowersData")]
public class TowersData : ScriptableObject
{
    [Header("Info")]
    public string TowersName;
    public Sprite Icon;
    public GameObject prefab;

    [Header("Weapon")]
    public GameObject WeaponPrefab;

    [Header("Cost")]
    public int buyCost;
    public int sellValue;

    [Header("Levels")]
    public TowerLevelData[] levels;
}
