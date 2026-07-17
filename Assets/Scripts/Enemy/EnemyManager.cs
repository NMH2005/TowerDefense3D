using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamagable {
    private EnemyData data;

    private int currentHp;
    private Transform target;
    private int currentIndex = 0;
    private bool isDead = false;
    private bool isAttackingBase = false;
    private int damage;

    private GameObject weaponInstance;
    private EnemyWeaponAttack weaponAttack;

    public int MaxHp => data != null ? data.MaxHp : 0;
    public int CurrentHp => currentHp;
    public int Damage => damage;

    public float GetRemainingDistance()
    {
        if (target == null) return float.MaxValue;

        float dist = Vector3.Distance(transform.position, target.position);

        for (int i = currentIndex; i < Waypoints.waypoints.Length - 1; i++)
            dist += Vector3.Distance(Waypoints.waypoints[i].position, Waypoints.waypoints[i + 1].position);

        return dist;
    }

    public void Initialize(EnemyData enemyData)
    {
        data = enemyData;
        currentHp = data.MaxHp;
        damage = data.Damage;

        weaponAttack = GetComponentInChildren<EnemyWeaponAttack>();
        if (weaponAttack != null)
            weaponAttack.ApplyStats(data);
    }

    private void Start()
    {
        target = Waypoints.waypoints[0];
    }

    private void Update()
    {
        if (isDead || isAttackingBase) return;
        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        float speed = data != null ? data.Speed : 5f;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            FindNextWaypoint();
        }
    }

    private void FindNextWaypoint()
    {
        currentIndex++;

        if (currentIndex >= Waypoints.waypoints.Length)
        {
            StartAttackingBase();
            return;
        }

        target = Waypoints.waypoints[currentIndex];
    }

    private void StartAttackingBase()
    {
        isAttackingBase = true;
        Vector3 lastWaypointPos = Waypoints.waypoints[Waypoints.waypoints.Length - 1].position;
        Debug.Log($"[EnemyManager] StartAttackingBase - enemy tại {transform.position}, waypoint cuối tại {lastWaypointPos}, lệch {Vector3.Distance(transform.position, lastWaypointPos):F2}");
        EventManager.RaiseEnemyReachedEnd(this);

        if (weaponAttack != null)
            weaponAttack.StartAttacking();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHp -= amount;
        if (currentHp <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (EconomyManager.Instance != null && data != null)
            EconomyManager.Instance.AddGold(data.GoldReward);

        EventManager.RaiseEnemyKilled(this);
        Destroy(gameObject);
    }
}