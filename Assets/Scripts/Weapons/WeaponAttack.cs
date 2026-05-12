using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private WeaponBase weaponBase;
    [SerializeField] private Transform firePoint;
    private Animator animator;
    private Transform target;
    private bool isAttacking;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        target = weaponBase.GetTarget();

        if (target != null && !isAttacking)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    public void OnShoot()
    {
        isAttacking = false;
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
