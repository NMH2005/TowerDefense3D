using UnityEngine;

public class Bullet : Ammo
{
    [SerializeField] private float speed = 5f;

    private void Update()
    {
        if (target == null) return;

        MoveToTarget();
    }
    private void MoveToTarget()
    {
        transform.Translate(dir * speed * Time.deltaTime, Space.World);
        
        if(Vector3.Distance(target.position, transform.position) <= 0.2f )
        {
            Destroy(gameObject);
        } 
    }
}
