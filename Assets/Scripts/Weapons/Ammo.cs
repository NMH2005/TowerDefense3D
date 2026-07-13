using UnityEngine;

public abstract class Ammo : MonoBehaviour {
    public Transform target;
    public Vector3 dir;

    protected int damage;
    protected float speed;

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
        EnemyManager enemy = other.GetComponent<EnemyManager>();
        if (enemy == null) return;

        enemy.TakeDamage(damage);
        Destroy(gameObject);
    }
}