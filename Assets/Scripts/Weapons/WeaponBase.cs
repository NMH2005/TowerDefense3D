using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour {
    [SerializeField] private float range = 5f;
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private TargetMode targetMode = TargetMode.First;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
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

    public void SetTargetMode(TargetMode mode)
    {
        targetMode = mode;
        target = null; 
    }

    public TargetMode GetTargetMode()
    {
        return targetMode;
    }

    public void ApplyStats(TowerLevelData levelData)
    {
        range = levelData.Range;
    }

    private void FindTarget()
    {
        GameObject[] enemyObjs = GameObject.FindGameObjectsWithTag("Enemy");

        List<EnemyManager> inRange = new List<EnemyManager>();

        foreach (var obj in enemyObjs)
        {
            float dis = Vector3.Distance(transform.position, obj.transform.position);
            if (dis > range) continue;

            EnemyManager enemy = obj.GetComponent<EnemyManager>();
            if (enemy != null)
                inRange.Add(enemy);
        }

        if (inRange.Count == 0)
        {
            target = null;
            return;
        }

        EnemyManager chosen = SelectByMode(inRange);
        target = chosen.transform;
    }

    private EnemyManager SelectByMode(List<EnemyManager> candidates)
    {
        switch (targetMode)
        {
            case TargetMode.First:
                return GetBest(candidates, e => e.GetRemainingDistance(), smaller: true);

            case TargetMode.Last:
                return GetBest(candidates, e => e.GetRemainingDistance(), smaller: false);

            case TargetMode.Strongest:
                return GetBest(candidates, e => e.CurrentHp, smaller: false);

            case TargetMode.Weakest:
                return GetBest(candidates, e => e.CurrentHp, smaller: true);

            case TargetMode.Random:
                return candidates[UnityEngine.Random.Range(0, candidates.Count)];

            default:
                return candidates[0];
        }
    }

    private EnemyManager GetBest(List<EnemyManager> list, Func<EnemyManager, float> selector, bool smaller)
    {
        EnemyManager best = list[0];
        float bestVal = selector(best);

        for (int i = 1; i < list.Count; i++)
        {
            float val = selector(list[i]);
            bool isBetter = smaller ? val < bestVal : val > bestVal;

            if (isBetter)
            {
                bestVal = val;
                best = list[i];
            }
        }

        return best;
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