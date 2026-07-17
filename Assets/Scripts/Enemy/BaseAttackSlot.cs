using UnityEngine;

public class BaseAttackSlot : MonoBehaviour {
    public static BaseAttackSlot Instance { get; private set; }

    [SerializeField] private Transform[] slots;
    private bool[] occupied;

    private void Awake()
    {
        Instance = this;
        occupied = new bool[slots.Length];
    }

    public Transform GetFreeSlot(out int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (!occupied[i])
            {
                occupied[i] = true;
                index = i;
                return slots[i];
            }
        }

        index = -1;
        return null;
    }

    public void ReleaseSlot(int index)
    {
        if (index >= 0 && index < occupied.Length)
            occupied[index] = false;
    }
}