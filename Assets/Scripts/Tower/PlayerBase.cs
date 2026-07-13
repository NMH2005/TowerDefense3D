using UnityEngine;


public class PlayerBase : MonoBehaviour {
    public static PlayerBase Instance { get; private set; }

    [SerializeField] private int maxLives = 20;

    public int MaxLives => maxLives;
    public int Lives { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Lives = maxLives;
    }

    private void OnEnable()
    {
        EventManager.OnEnemyReachedEnd += HandleEnemyReachedEnd;
    }

    private void OnDisable()
    {
        EventManager.OnEnemyReachedEnd -= HandleEnemyReachedEnd;
    }

    private void Start()
    {
        EventManager.RaiseLivesChanged(Lives);
    }

    private void HandleEnemyReachedEnd(EnemyManager enemy)
    {
        if (IsGameOver) return;
        TakeDamage(enemy.DamageToBase);
    }

    private void TakeDamage(int amount)
    {
        if (IsGameOver) return;

        Lives -= amount;
        if (Lives < 0) Lives = 0;

        EventManager.RaiseLivesChanged(Lives);

        if (Lives <= 0)
        {
            IsGameOver = true;
            EventManager.RaiseGameOver();
        }
    }
}