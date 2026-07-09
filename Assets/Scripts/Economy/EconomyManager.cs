using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance { get; private set; }

    [SerializeField] private int startingGold = 200;

    public int gold {  get; private set; }
    private void Awake()
    {
        Instance = this;
        gold = startingGold;
    }

    private void Start()
    {
        EventManager.RaiseGoldChanged(gold);
    }

    public bool CanAfford(int amount)
    {
        return gold >= amount;
    } 

    public bool TrySpend(int amount)
    {
        if(!CanAfford(amount)) return false;

        gold-=amount;
        EventManager.RaiseGoldChanged(gold);
        return true;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        EventManager.RaiseGoldChanged(gold);
    }
}
