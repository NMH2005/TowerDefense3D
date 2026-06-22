using System;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static event Action OnTowerPlaced;

    public static void RaiseTowerPlaced( )
    {
        OnTowerPlaced?.Invoke();
    }
}
