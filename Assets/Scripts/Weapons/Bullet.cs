using UnityEngine;

public class Bullet : Ammo {

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        MoveToTarget();
    }
    private void MoveToTarget()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(target.position, transform.position) <= 0.2f)
        {
            Destroy(gameObject);
        }
    }
}
