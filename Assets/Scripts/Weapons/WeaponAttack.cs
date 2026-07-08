using UnityEngine;

public class WeaponAttack : MonoBehaviour {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private WeaponBase weaponBase;
    [SerializeField] private Transform firePoint;
    private Animator animator;
    private Transform target;
    private bool isAttacking;

    private int damage;
    private float fireRate = 1f; 
    private float projectileSpeed;
    private float cooldownTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        target = weaponBase.GetTarget();

        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        if (target != null && !isAttacking && cooldownTimer <= 0f)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void ApplyStats(TowerLevelData levelData)
    {
        damage = levelData.Damage;
        fireRate = levelData.FireRate;
        projectileSpeed = levelData.ProjectileSpeed;
    }

    public void OnShoot()
    {
        isAttacking = false;
        cooldownTimer = fireRate > 0f ? 1f / fireRate : 0f;
        SpawnBullet(projectilePrefab);
    }

    private void SpawnBullet(GameObject ammoPrefab)
    {
        Vector3 dir = (target.position - transform.position).normalized;
        Vector3 spawnPos = firePoint.position;

        GameObject ammoObj = Instantiate(ammoPrefab, spawnPos, Quaternion.LookRotation(dir));
        Ammo ammo = ammoObj.GetComponent<Ammo>();

        if (ammo != null)
        {
            ammo.SetTarget(target);
            ammo.SetDirection(dir);
        }

    }
}