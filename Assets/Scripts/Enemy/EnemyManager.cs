using UnityEngine;

public class EnemyManager : MonoBehaviour, IDamagable {
    private EnemyData data;
    private int maxHp;
    private int currentHp;
    private Transform target;
    private int currentIndex = 0;
    private bool isDead = false;
    private bool isAttackingBase = false;
    private int damage;
    private Transform[] waypoints;
    private GameObject weaponInstance;
    private EnemyWeaponAttack weaponAttack;
    private int slotIndex = -1;
    private Transform attackSlot;
    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public int Damage => damage;

    public float GetRemainingDistance()
    {
        if (target == null) return float.MaxValue;

        float dist = Vector3.Distance(transform.position, target.position);

        for (int i = currentIndex; i < waypoints.Length - 1; i++)
            dist += Vector3.Distance(
                waypoints[i].position,
                waypoints[i + 1].position
            );

        return dist;
    }

    public void Initialize(EnemyData enemyData, float multiplier, Transform[] path)
    {
        data = enemyData;
        waypoints = path;
        maxHp = Mathf.RoundToInt(data.MaxHp * multiplier);
        currentHp = maxHp;
        damage = data.Damage;

        weaponAttack = GetComponentInChildren<EnemyWeaponAttack>();
        if (weaponAttack != null)
            weaponAttack.ApplyStats(data);
    }

    private void Start()
    {
        target = waypoints[0];
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 3f, Color.red);

        if (isDead) return;

        if (isAttackingBase)
        {
            MoveToAttackSlot();
            return;
        }

        MoveToWaypoint();
    }

    private void RotateTowards(Vector3 destination)
    {
        Vector3 direction = destination - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            10f * Time.deltaTime
        );
    }

    private void MoveToAttackSlot()
    {
        if (attackSlot == null) return;

        float speed = data != null ? data.Speed : 5f;

        transform.position = Vector3.MoveTowards(
            transform.position,
            attackSlot.position,
            speed * Time.deltaTime);

        RotateTowards(PlayerBase.Instance.transform.position);

        if (Vector3.Distance(transform.position, attackSlot.position) <= 0.1f)
        {
            if (weaponAttack != null)
                weaponAttack.StartAttacking();

            attackSlot = null;
        }
    }
    private void MoveToWaypoint()
    {
        float speed = data != null ? data.Speed : 5f;

        RotateTowards(target.position);

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            FindNextWaypoint();
        }
    }

    private void FindNextWaypoint()
    {
        currentIndex++;

        if (currentIndex >= waypoints.Length)
        {
            StartAttackingBase();
            return;
        }

        target = waypoints[currentIndex];
    }

    private void StartAttackingBase()
    {
        isAttackingBase = true;

        EventManager.RaiseEnemyReachedEnd(this);

        attackSlot = BaseAttackSlot.Instance.GetFreeSlot(out slotIndex);

        if (attackSlot == null)
        {
            attackSlot = transform;
        }
    }

    public void TakeDamage(int amount)
    {

        Debug.Log(currentHp);
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
        FindFirstObjectByType<SpawnerManager>()?.OnEnemyDead();

        if (slotIndex != -1)
        {
            BaseAttackSlot.Instance.ReleaseSlot(slotIndex);
        }

        Destroy(gameObject);
    }
}