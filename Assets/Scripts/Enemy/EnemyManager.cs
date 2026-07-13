using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private int maxHp = 10;
    [SerializeField] private int goldReward = 5;
    [SerializeField] private int damageToBase = 1;

    private int currentHp;
    private Transform target;
    private int currentIndex = 0;
    private bool isDead = false;

    public int MaxHp => maxHp;
    public int CurrentHp => currentHp;
    public int DamageToBase => damageToBase;

    private void Start()
    {
        currentHp = maxHp;
        target = Waypoints.waypoints[0];
    }

    private void Update()
    {
        if (isDead) return;
        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
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
            ReachEnd();
            return;
        }

        target = Waypoints.waypoints[currentIndex];
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

        if (EconomyManager.Instance != null)
            EconomyManager.Instance.AddGold(goldReward);

        EventManager.RaiseEnemyKilled(this);
        Destroy(gameObject);
    }

    private void ReachEnd()
    {
        if (isDead) return;
        isDead = true;

        EventManager.RaiseEnemyReachedEnd(this);
        Destroy(gameObject);
    }
}