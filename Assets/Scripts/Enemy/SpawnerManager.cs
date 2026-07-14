using UnityEngine;

public class SpawnerManager : MonoBehaviour {
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private float timeInterval = 2f;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f);
        int randomIndex = Random.Range(0, enemies.Length);
        EnemyData data = enemies[randomIndex];

        GameObject enemyObj = Instantiate(data.prefab, spawnPos, Quaternion.identity);
        EnemyManager enemyManager = enemyObj.GetComponent<EnemyManager>();
        if (enemyManager != null)
            enemyManager.Initialize(data);
    }
}