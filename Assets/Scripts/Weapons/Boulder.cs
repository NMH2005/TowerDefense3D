using UnityEngine;

public class Boulder : Ammo {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float arcHeight = 5f;

    private Vector3 startPos;
    private float progress = 0f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        if (target == null) return;

        MoveToTarget();
    }

    private void MoveToTarget()
    {
progress += Time.deltaTime * speed;

        Vector3 current = Vector3.Lerp(startPos, target.position, progress);

        float distance = Vector3.Distance(startPos, target.position);
        float heightScale = distance * 0.2f;

        float height = arcHeight * heightScale * 4 * (progress - progress * progress);
        current.y += height;
        transform.position = current;
    }
}
