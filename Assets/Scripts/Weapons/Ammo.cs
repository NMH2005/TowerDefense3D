using UnityEngine;

public abstract class Ammo : MonoBehaviour
{
    public Transform target;
    public Vector3 dir;
    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetDirection(Vector3 d)
    {
        dir = d.normalized;
    }
}
