using UnityEngine;

public class SpawnerManager : MonoBehaviour {
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private float timeInterval = 2f;
    [SerializeField] private int enemiesPerWave = 10;
    [SerializeField] private int maxSandboxWaves = 20;
    [SerializeField] private int wavesPerTierUnlock = 3;

    private float timer;
    private int enemiesSpawnedInWave = 0;
    private int currentWave = 1;
    private bool spawningStopped = false;

    private void Start()
    {
        EventManager.RaiseWaveChanged(currentWave);
    }

    private void OnEnable()
    {
        EventManager.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver -= HandleGameOver;
    }

    private void Update()
    {
        if (spawningStopped) return;

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
        EnemyData data = PickEnemyForCurrentWave();

        GameObject enemyObj = Instantiate(data.prefab, spawnPos, Quaternion.identity);
        EnemyManager enemyManager = enemyObj.GetComponent<EnemyManager>();
        if (enemyManager != null)
            enemyManager.Initialize(data);

        enemiesSpawnedInWave++;

        if (enemiesSpawnedInWave >= enemiesPerWave)
        {
            enemiesSpawnedInWave = 0;
            AdvanceWave();
        }
    }

    private EnemyData PickEnemyForCurrentWave()
    {
        int unlockedCount = 1 + (currentWave - 1) / wavesPerTierUnlock;
        unlockedCount = Mathf.Clamp(unlockedCount, 1, enemies.Length);

        int randomIndex = Random.Range(0, unlockedCount);
        return enemies[randomIndex];
    }

    private void AdvanceWave()
    {
        if (GameSession.SelectedMode == GameMode.Sandbox && currentWave >= maxSandboxWaves)
        {
            spawningStopped = true;
            EventManager.RaiseAllWavesCompleted();
            return;
        }

        currentWave++;
        EventManager.RaiseWaveChanged(currentWave);
    }

    private void HandleGameOver()
    {
        spawningStopped = true;
    }
}