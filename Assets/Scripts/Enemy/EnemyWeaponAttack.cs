using UnityEngine;

public class EnemyWeaponAttack : MonoBehaviour {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    private Animator animator;
    private bool isEnabled = false;
    private bool isAttacking;

    private int damage;
    private float fireRate = 1f;
    private float projectileSpeed;
    private float cooldownTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ApplyStats(EnemyData data)
    {
        damage = data.Damage;
        fireRate = data.FireRate;
        projectileSpeed = data.ProjectileSpeed;
    }

    public void StartAttacking()
    {
        isEnabled = true;
    }

    private void Update()
    {
        if (!isEnabled || PlayerBase.Instance == null) return;

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (!isAttacking && cooldownTimer <= 0f)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void OnShoot()
    {
        isAttacking = false;
        cooldownTimer = fireRate > 0f ? 1f / fireRate : 0f;

        if (PlayerBase.Instance == null) return;

        SpawnBullet();
    }

    private void SpawnBullet()
    {
        Transform baseTransform = PlayerBase.Instance.transform;
        Vector3 dir = (baseTransform.position - transform.position).normalized;
        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;

        GameObject ammoObj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(dir));
        Ammo ammo = ammoObj.GetComponent<Ammo>();

        if (ammo != null)
        {
            ammo.SetTarget(baseTransform);
            ammo.SetDirection(dir);
            ammo.SetDamage(damage);
            ammo.SetSpeed(projectileSpeed);
        }
    }
}