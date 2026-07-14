using UnityEngine;

public abstract class Ammo : MonoBehaviour {
    public Transform target;
    public Vector3 dir;

    protected int damage;
    protected float speed = 5f;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetDirection(Vector3 d)
    {
        dir = d.normalized;
    }

    public void SetDamage(int amount)
    {
        damage = amount;
    }

    public void SetSpeed(float value)
    {
        speed = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable damageable = other.GetComponent<IDamagable>();
        if (damageable == null) return;

        damageable.TakeDamage(damage);
        Destroy(gameObject);
    }
}