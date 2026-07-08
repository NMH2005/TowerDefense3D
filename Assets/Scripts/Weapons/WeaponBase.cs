using UnityEngine;

public class WeaponBase : MonoBehaviour {
    [SerializeField] private float range = 5f;
    [SerializeField] private float turnSpeed = 5f;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            GetFirstTarget();
            return;
        }

        float dis = Vector3.Distance(transform.position, target.position);

        if (dis > range)
        {
            target = null;
            return;
        }

        LookAtTarget();
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void ApplyStats(TowerLevelData levelData)
    {
        range = levelData.Range;
    }

    private void GetFirstTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDis = Mathf.Infinity;
        Transform closest = null;

        foreach (var enemy in enemies)
        {
            float dis = Vector3.Distance(transform.position, enemy.transform.position);

            if (dis < closestDis && dis <= range)
            {
                closestDis = dis;
                closest = enemy.transform;
            }
        }

        target = closest;
    }

    private void LookAtTarget()
    {
        Vector3 dir = target.position - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}