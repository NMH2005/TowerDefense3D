using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Transform target;
    private int currentIndex = 0;

    private void Start()
    {
        target = Waypoints.waypoints[0];
    }

    private void Update()
    {
        MoveToWaypoint();
    }

    private void MoveToWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, target.position) <= 0.1f)
        {
            FindNextWaypoint();
        }
    }

    private void FindNextWaypoint()
    {
        currentIndex++;
        target = Waypoints.waypoints[currentIndex];
    }
}
