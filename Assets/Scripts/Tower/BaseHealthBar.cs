using UnityEngine;
using UnityEngine.UI;

public class BaseHealthBar : MonoBehaviour {
    [SerializeField] private Image fillImage;

    private void OnEnable()
    {
        EventManager.OnLivesChanged += UpdateBar;

        if (PlayerBase.Instance != null)
            UpdateBar(PlayerBase.Instance.Lives);
    }

    private void OnDisable()
    {
        EventManager.OnLivesChanged -= UpdateBar;
    }

    private void UpdateBar(int lives)
    {
        if (fillImage == null || PlayerBase.Instance == null) return;

        float ratio = PlayerBase.Instance.MaxLives > 0
            ? (float)lives / PlayerBase.Instance.MaxLives
            : 0f;

        fillImage.fillAmount = Mathf.Clamp01(ratio);
    }
}