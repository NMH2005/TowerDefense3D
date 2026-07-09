using System;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    private void OnEnable()
    {
        EventManager.OnGoldChanged += UpDateText;
    }

    private void OnDisable()
    {
        EventManager.OnGoldChanged -= UpDateText;
    }

    private void UpDateText(int gold)
    {
        if (goldText != null)
        {
            goldText.text = gold.ToString();
        }
    }
}
