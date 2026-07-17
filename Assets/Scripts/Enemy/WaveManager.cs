using UnityEngine;

public class WaveManager : MonoBehaviour {
    public static WaveManager Instance { get; private set; }
    private int finishedSpawnerCount = 0;
    public int CurrentWave { get; private set; } = 1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        EventManager.RaiseWaveChanged(CurrentWave);
    }

    public void NextWave()
    {
        CurrentWave++;
        EventManager.RaiseWaveChanged(CurrentWave);
    }

    public void NotifySpawnerFinished()
    {
        finishedSpawnerCount++;

        int activeSpawnerCount = 0;

        foreach (SpawnerManager spawner in FindObjectsByType<SpawnerManager>(FindObjectsSortMode.None))
        {
            if (CurrentWave >= spawner.UnlockWave)
                activeSpawnerCount++;
        }

        if (finishedSpawnerCount >= activeSpawnerCount)
        {
            finishedSpawnerCount = 0;
            NextWave();
        }
    }

}