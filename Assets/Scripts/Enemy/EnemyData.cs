using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject {
    [Header("Info")]
    public string EnemyName;
    public GameObject prefab;
    public Sprite icon;


    [Header("Stats")]
    public int MaxHp;
    public float Speed;
    public int Damage;        
    public float FireRate;      
    public float ProjectileSpeed;

    [Header("Reward")]
    public int GoldReward;    
}