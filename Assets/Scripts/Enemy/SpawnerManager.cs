using UnityEngine;

public class SpawnerManager : MonoBehaviour {
    [SerializeField] private EnemyData[] enemies;
    [SerializeField] private float timeInterval = 2f;
    [SerializeField] private int maxSandboxWaves = 20;
    [SerializeField] private int wavesPerTierUnlock = 3;
    [SerializeField] private float hpIncreasePer5Waves = 0.2f;
    [SerializeField] private int unlockWave = 1;
    [SerializeField] private Transform[] waypoints;
    private float timer;
    private int enemiesSpawnedInWave = 0;
    private int enemiesAlive = 0;
    private bool waveFinishedSpawning = false;
    private bool spawningStopped = false;
    [SerializeField] private int startingEnemiesPerWave = 1;
    [SerializeField] private int enemyIncreasePerWave = 2;
    [SerializeField] private int maxEnemiesPerWave = 40;
    public int UnlockWave => unlockWave;

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

        if (WaveManager.Instance.CurrentWave < unlockWave)
            return;

        timer += Time.deltaTime;
        if (timer > timeInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    private int GetEnemiesPerWave()
    {
        int count = startingEnemiesPerWave +
                    (WaveManager.Instance.CurrentWave - 1) * enemyIncreasePerWave;

        return Mathf.Min(count, maxEnemiesPerWave);
    }

    private float GetHpMultiplier()
    {
        int bonusTier = (WaveManager.Instance.CurrentWave - 1) / 5;
        return 1f + bonusTier * hpIncreasePer5Waves;
    }

    private void SpawnEnemy()
    {
        if (waveFinishedSpawning)
            return;

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5f);
        EnemyData data = PickEnemyForCurrentWave();

        GameObject enemyObj = Instantiate(data.prefab, spawnPos, Quaternion.identity);
        EnemyManager enemyManager = enemyObj.GetComponent<EnemyManager>();
        if (enemyManager != null)
            enemyManager.Initialize(
                data,
                GetHpMultiplier(),
                waypoints,
                this
            ); enemiesSpawnedInWave++;
        enemiesAlive++;
        if (enemiesSpawnedInWave >= GetEnemiesPerWave())
        {
            waveFinishedSpawning = true;
        }
    }

    private EnemyData PickEnemyForCurrentWave()
    {
        int unlockedCount = 1 + (WaveManager.Instance.CurrentWave - 1) / wavesPerTierUnlock;
        unlockedCount = Mathf.Clamp(unlockedCount, 1, enemies.Length);

        int randomIndex = Random.Range(0, unlockedCount);
        return enemies[randomIndex];
    }

    public void OnEnemyDead()
    {
        enemiesAlive--;

        if (waveFinishedSpawning && enemiesAlive <= 0)
        {
            if (GameSession.SelectedMode == GameMode.Sandbox &&
    WaveManager.Instance.CurrentWave >= maxSandboxWaves)
            {
                spawningStopped = true;
                EventManager.RaiseAllWavesCompleted();
                return;
            }

            WaveManager.Instance.NotifySpawnerFinished();

            enemiesSpawnedInWave = 0;
            enemiesAlive = 0;
            waveFinishedSpawning = false;
        }
    }

    private void HandleGameOver()
    {
        spawningStopped = true;
    }
}